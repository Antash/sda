using System;
using System.IO;
using System.Reflection;
using ICSharpCode.SharpDevelop.Sda;
using Microsoft.Win32;

namespace IDEHostApplication
{
	class SDIntegration
	{
		private static string _sdRootDir;
		private static string _sdDataDir;
		private static string _sdAddInDir;
		private static string _sdBinDir;

		private static readonly string[] AssemblyExtentions = new[] { "dll", "exe" };

		private WorkbenchSettings _workbenchSettings;
		private SharpDevelopHost _sdHost;
		private StartupSettings _startupSettings;
		private bool workbenchIsRunning;

		public SDIntegration()
		{
			workbenchIsRunning = false;

			Init();
		}

		/// <summary>
		/// Preparing IDE host enviorenment
		/// </summary>
		private void Init()
		{
			string sdBase;
			// get #D installation path from registry (TODO AA : works with 3.0 only!)
			var sdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\SharpDevelop.exe");
			if (sdKey != null)
			{
				sdBase = (string)sdKey.GetValue("");
			}
			else
			{
				throw new Exception("no #D installation!");
			}

			_sdRootDir = Path.GetDirectoryName(Path.GetDirectoryName(sdBase).TrimEnd('\\'));
			_sdDataDir = Path.Combine(_sdRootDir, "data");
			_sdAddInDir = Path.Combine(_sdRootDir, "AddIns");
			_sdBinDir = Path.Combine(_sdRootDir, "bin");

			ConfigureEnviorenment();
		}

		private void ConfigureEnviorenment()
		{
			if (_sdHost != null) return;

			//TODO AA : review initialisation
			_startupSettings = new StartupSettings
								{
									ApplicationName = "CustomSharpDevelop",
									AllowUserAddIns = true,
									ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ICSharpCode/SharpDevelop3.0"),
									DataDirectory = _sdDataDir,
									ApplicationRootPath = _sdRootDir
								};
			_startupSettings.AddAddInsFromDirectory(_sdAddInDir);

			//Loading customised #D config file
			//_startupSettings.AddAddInFile(Path.Combine(Application.StartupPath, sdConfigFile));

			AppDomain.CurrentDomain.AssemblyResolve += LoadAssemlbyFromProductInstallationFolder;

			_sdHost = new SharpDevelopHost(AppDomain.CurrentDomain, _startupSettings);
			_workbenchSettings = new WorkbenchSettings();
		}

		private void RunWorkbench()
		{
			if (!workbenchIsRunning)
				System.Threading.ThreadPool.QueueUserWorkItem(ThreadedRun);
		}

		private void ThreadedRun(object state)
		{
			_sdHost.RunWorkbench(_workbenchSettings);
		}

		/* 
		 * This code is needed to load references from correct place
		 * 
		 *	1. #D references from its installation path
		 *		-binaries
		 *		-addins
		 */
		#region asembly resolving

		private static Assembly LoadAssemlbyFromProductInstallationFolder(object sender, ResolveEventArgs args)
		{
			Assembly result = null;

			if (args != null && !string.IsNullOrEmpty(args.Name))
			{
				var typeName = args.Name.Split(',')[0];
				var assemblyPath = FindAssembly(typeName);
				if (!String.IsNullOrEmpty(assemblyPath))
					result = Assembly.LoadFrom(assemblyPath);
			}

			return result;
		}

		private static string FindAssembly(string typeName)
		{
			return FindSDBinAssembly(typeName) ?? FindSDAddInAssembly(typeName);
		}

		private static string FindSDBinAssembly(string typeName)
		{
			for (var tmpn = typeName; ; tmpn = tmpn.Substring(0, tmpn.LastIndexOf(".")))
			{
				foreach (var ext in AssemblyExtentions)
				{
					var assemblyPath = Path.Combine(_sdBinDir, String.Format("{0}.{1}", tmpn, ext));
					if (File.Exists(assemblyPath))
						return assemblyPath;
				}
				if (!tmpn.Contains("."))
					return null;
			}
		}

		private static string FindSDAddInAssembly(string typeName)
		{
			for (var tmpn = typeName; ; tmpn = tmpn.Substring(0, tmpn.LastIndexOf(".")))
			{
				foreach (var ext in AssemblyExtentions)
				{
					var assembls = Directory.GetFiles(_sdAddInDir, String.Format("{0}.{1}", tmpn, ext), SearchOption.AllDirectories);
					if (assembls.Length > 0)
					{
						return assembls[0];
					}
				}
				if (!tmpn.Contains("."))
					return null;
			}
		}

		#endregion

	}
}
