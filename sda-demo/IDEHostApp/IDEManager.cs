using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ICSharpCode.SharpDevelop.Sda;

namespace IDEHostApp
{
	class IDEManager
	{
		private string baseDir;
		private string binDir;
		private string dataDir;
		private string addInDir;

		private WorkbenchSettings workbenchSettings;
		private SharpDevelopHost host;
		private StartupSettings startup;

		public IDEManager()
		{
			baseDir = @"C:\Program Files\SharpDevelop\4.1";
			binDir = baseDir + @"\bin\";
			dataDir = baseDir + @"\data\"; 
			addInDir = baseDir + @"\AddIns\";

			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

			ConfigureEnviorenment();
		}

		Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			Assembly result = null;
			if (args != null && !string.IsNullOrEmpty(args.Name))
			{
				try
				{
					result = Assembly.LoadFrom(FindAssembly(args.Name));
				}
				catch (Exception e)
				{
					Console.Error.WriteLine("SharpDevelop assembly loading error: " + e.Message);
				}
			}

			return result;
		}

		private string FindAssembly(string name)
		{
			var typeName = name.Split(new string[] { "," }, StringSplitOptions.None)[0];
			var assemblyPath = Path.Combine(binDir, string.Format("{0}.dll", typeName));

			var strs = typeName.Split('.');
			var assemblyName = string.Empty;

			for (int j = strs.Length - 1; j >= 0; j--)
			{
				
				//assemblyName += strs[j];
				assemblyPath = Path.Combine(binDir, string.Format("{0}.dll", typeName));
				if (!File.Exists(assemblyPath))
				{
					assemblyPath = Path.ChangeExtension(assemblyPath, "exe");
					if (!File.Exists(assemblyPath))
					{
						var strings = typeName.Split('.');
						var sb = new StringBuilder();
						for (int i = 0; i < strings.Length - 1; i++)
						{
							sb.Append(strings[i]);
							sb.Append('.');
						}
						typeName = sb.ToString();
						var assembls = Directory.GetFiles(addInDir, typeName + "*", SearchOption.AllDirectories);
						if (assembls.Length > 0)
						{
							assemblyPath = Path.Combine(Path.GetDirectoryName(assembls[0]), string.Format("{0}dll", typeName));
							if (!File.Exists(assemblyPath))
							{
								typeName += strings[strings.Length - 1];
								assemblyPath = Path.Combine(Path.GetDirectoryName(assembls[0]), string.Format("{0}.dll", typeName));
							}
							else break;
						}
					}
					else break;
				}
				else break;
				typeName = typeName.Substring(0, typeName.IndexOf(strs[j]) - 1);
				//assemblyName += '.';
			}
			//if (!File.Exists(assemblyPath))
			//{
			//    var dllBasePath = Path.GetDirectoryName(Application.StartupPath) + Path.DirectorySeparatorChar;
			//    var dllPath = dllBasePath + assemblyName + ".dll";
			//    if (File.Exists(dllPath))
			//        assemblyPath = dllPath;
			//    else
			//        assemblyPath = Path.Combine(Path.GetDirectoryName(Application.StartupPath.TrimEnd('\\')), assemblyName + ".dll");
			//}
			return assemblyPath;
		}

		private void ConfigureEnviorenment()
		{
			if (host != null) return;

			startup = new StartupSettings {AllowUserAddIns = true, DataDirectory = dataDir};
			startup.AddAddInsFromDirectory(addInDir);

			host = new SharpDevelopHost(AppDomain.CurrentDomain, startup);

			workbenchSettings = new WorkbenchSettings();

			RunIDE();
		}

		public void RunIDE()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(ThreadedRun);
		}

		void ThreadedRun(object state)
		{
			host.RunWorkbench(workbenchSettings);
		}
	}
}
