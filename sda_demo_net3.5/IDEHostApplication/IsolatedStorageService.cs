using System.Reflection;
using System.IO;
using System.IO.IsolatedStorage;

namespace IDEHostApplication
{
	/// <summary>
	/// Helper class for using Isolated Storage.
	/// In IS we locate compiled addin files. After each addin rebuild new assembly 
	/// is copied to separate folder in isolated storage. Ext. Application loads addin .dll from this created directory.
	/// 
	/// All this code is used only from IDEHost.
	/// </summary>
	static class IsolatedStorageService
	{
		/// <summary>
		/// Isolated storage instance
		/// </summary>
		private static readonly IsolatedStorageFile Storage = IsolatedStorageFile.GetUserStoreForDomain();

		/// <summary>
		/// Copies pdb and dll files of builded addin project 
		/// from its binaries to isolated storage
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>path to dll addin assembly</returns>
		internal static string CopyFileToStorage(string filename)
		{
			var pdbFilename = ChangeExetention(filename, ".pdb");
			var dir = GetTempDirectory();
			CopyFileToIso(dir, pdbFilename);
			return CopyFileToIso(dir, filename);
		}

		/// <summary>
		/// Copies specified file to specified directory in isolated storage
		/// </summary>
		/// <returns>destination file name</returns>
		private static string CopyFileToIso(string dirname, string filename)
		{
			var stmWriter = new IsolatedStorageFileStream(
				 Path.Combine(dirname, Path.GetFileName(filename)), FileMode.Create, Storage);

			using (var fs = new FileStream(filename, FileMode.Open))
			{
				var reader = new BinaryReader(fs);
				var writer = new BinaryWriter(stmWriter);

				var buffer = new byte[fs.Length];
				reader.Read(buffer, 0, (int)fs.Length);
				reader.Close();
				writer.Write(buffer);
				writer.Close();
			}
			return stmWriter.GetType().GetField("m_FullPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(stmWriter).ToString();
		}

		/// <summary>
		/// Creates new temporary direstory in the isolated storage
		/// </summary>
		/// <rereturns>temp directory name</rereturns>
		private static string GetTempDirectory()
		{
			string dirname = Path.GetRandomFileName();
			Storage.CreateDirectory(dirname);
			return dirname;
		}

		/// <summary>
		/// Changes extention of the specified file
		/// </summary>
		private static string ChangeExetention(string filename, string newExt)
		{
			var folder = Path.GetDirectoryName(filename);
			var file = Path.GetFileNameWithoutExtension(filename) + newExt;
			return string.IsNullOrEmpty(folder) ? file : Path.Combine(folder, file);
		}

		/// <summary>
		/// Clear isolated storage
		/// </summary>
		public static void ClearStorage()
		{
			try
			{
				Storage.Remove();
			}
			catch
			{
				//Waiting need to be sure files are not being in use by Ext. Application                
				System.Threading.Thread.Sleep(3000);
				Storage.Remove();
			}
		}
	}
}
