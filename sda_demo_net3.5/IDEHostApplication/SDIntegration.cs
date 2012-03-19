using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using CommunicationServices;
using ICSharpCode.SharpDevelop.Sda;
using Microsoft.Win32;
using System.Windows.Forms;

namespace IDEHostApplication
{
	class SDIntegration
	{
		private static string _sdRootDir;
		private static string _sdDataDir;
		private static string _sdAddInDir;
		private static string _sdBinDir;

		private const string SDConfigFile = "ICSharpCode.SharpDevelop.addin";

		private static readonly string[] AssemblyExtentions = new[] { "dll", "exe" };

		private WorkbenchSettings _workbenchSettings;
		private SharpDevelopHost _sdHost;
		private StartupSettings _startupSettings;

		public Process HostProcess { get; private set; }

		private string _addInBinaryFilePath;

		#region Singletone definition

		private static volatile SDIntegration _instance;
		private static readonly object SyncRoot = new Object();

		public static SDIntegration Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new SDIntegration();
					}
				}

				return _instance;
			}
		}

		#endregion

		#region Initialization

		private SDIntegration()
		{
			StateHolder.Instance.IDEHostAppState = StateHolder.IDEHostApplicationStates.Initializing;

			HostProcess = Process.GetCurrentProcess().Parent();

			InitCommunicationService();
			InitVariables();
			ConfigureEnviorenment();

			StateHolder.Instance.IDEHostAppState = StateHolder.IDEHostApplicationStates.Initialized;
		}

		private void InitCommunicationService()
		{
			var host = new ServiceHost(typeof(SDAService));
			var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
							{
								ReceiveTimeout = TimeSpan.FromHours(42),
								SendTimeout = TimeSpan.FromHours(42)
							};
			host.AddServiceEndpoint(typeof(ISDAService), binding, String.Format(CommunicationService.AddressTemplate, Program.AppGuid));
			host.Open();
		}

		private void InitVariables()
		{
			string sdBase;
			// get #D installation path from registry
			//var sdKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\SharpDevelop.exe");
			//if (sdKey != null)
			//{
			//    sdBase = (string)sdKey.GetValue("");
			//}
			//else
			//{
			//    throw new Exception("no #D installation!");
			//}
			// TODO AA : remove hardcode!
			sdBase = @"C:\Program Files (x86)\SharpDevelop\3.0";
			//sdBase = @"C:\Program Files\SharpDevelop\3.0";

			//_sdRootDir = Path.GetDirectoryName(Path.GetDirectoryName(sdBase).TrimEnd('\\'));
			_sdRootDir = sdBase;
			_sdDataDir = Path.Combine(_sdRootDir, "data");
			_sdAddInDir = Path.Combine(_sdRootDir, "AddIns");
			_sdBinDir = Path.Combine(_sdRootDir, "bin");
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
			//TODO AA : implement loading custom addin configuration
			_startupSettings.AddAddInsFromDirectory(Path.Combine(_sdAddInDir, "AddIns"));

			//Loading customised #D config file
			_startupSettings.AddAddInFile(Path.Combine(Application.StartupPath, SDConfigFile));

			AppDomain.CurrentDomain.AssemblyResolve += LoadAssemlbyFromProductInstallationFolder;

			_sdHost = new SharpDevelopHost(AppDomain.CurrentDomain, _startupSettings);
			_workbenchSettings = new WorkbenchSettings();

			assignHandlers();
		}

		private void assignHandlers()
		{
			_sdHost.BeforeRunWorkbench += new EventHandler(_sdHost_BeforeRunWorkbench);
			_sdHost.WorkbenchClosed += new EventHandler(_sdHost_WorkbenchClosed);
			_sdHost.SolutionLoaded += new EventHandler(_sdHost_SolutionLoaded);
			_sdHost.StartBuild += new EventHandler(_sdHost_StartBuild);
			_sdHost.EndBuild += new EventHandler(_sdHost_EndBuild);
		}

		#endregion

		#region SharpDevelopHost event handlers

		void _sdHost_BeforeRunWorkbench(object sender, EventArgs e)
		{
			StateHolder.Instance.IDEHostAppState = StateHolder.IDEHostApplicationStates.Running;
		}

		void _sdHost_EndBuild(object sender, EventArgs e)
		{
			// TODO AA : implement
		}

		void _sdHost_StartBuild(object sender, EventArgs e)
		{
			// TODO AA : implement
		}

		void _sdHost_SolutionLoaded(object sender, EventArgs e)
		{
			// TODO AA : implement
		}

		void _sdHost_WorkbenchClosed(object sender, EventArgs e)
		{
			StateHolder.Instance.IDEHostAppState = StateHolder.IDEHostApplicationStates.Suspended;
		}

		#endregion

		/* 
		 * This code is needed to load references from correct place
		 * 
		 *	1. #D references from its installation path
		 *		-binaries
		 *		-addins
		 */
		#region Assembly resolving

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

		private void RunWorkbench()
		{
			if (StateHolder.Instance.IDEHostAppState == StateHolder.IDEHostApplicationStates.Initialized)
				System.Threading.ThreadPool.QueueUserWorkItem(state => _sdHost.RunWorkbench(_workbenchSettings));
		}

		internal void OpenProject(string fileName)
		{
			if (!_sdHost.IsSolutionOrProject(fileName))
			{
				CommunicationService.SdaCallback.ProjectOpenError();
				return;
			}

			if (!StateHolder.Instance.CanOpenProject())
			{
				// TODO AA : exception?
				return;
			}
			if (StateHolder.Instance.IDEHostAppState == StateHolder.IDEHostApplicationStates.Running)
			{
				_sdHost.OpenProject(fileName);
			}
			else
			{
				_workbenchSettings.InitialFileList.Clear();
				_workbenchSettings.InitialFileList.Add(fileName);
				RunWorkbench();
			}
		}

		internal void CloseIDE()
		{
			if (!_sdHost.CloseWorkbench(false))
			{
				//TODO AA : force close
			}
		}

		internal void foo()
		{
		}

		internal void ShowIDE()
		{
			//TODO AA : implement
			if (!StateHolder.Instance.CanShowIDE())
				return;
			if (!string.IsNullOrEmpty(StateHolder.Instance.LastProjectOpened))
			{
				//if (StateHolder.Instance.ProjectState)
				//{
				//    OpenProject(lastProjectOpened);
				//}
				//else
				//{
				//    BringToFrontIDE();
				//}
			}
			else
			{
			}

			RunWorkbench();
		}

		internal void CopyToIsoStorage(string outputAssemblyFullPath)
		{
			_addInBinaryFilePath = IsolatedStorageService.CopyFileToStorage(outputAssemblyFullPath);
		}

		internal void OnBuildSuccess(bool isDebugging)
		{
			CommunicationService.SdaCallback.BuildSucceded(_addInBinaryFilePath, isDebugging);
		}

		internal void OnBuildFailure()
		{
			CommunicationService.SdaCallback.BuildFailed();
		}

		internal void AttachToHost()
		{
			if (CommunicationService.SdaCallback.IsParentx64())
			{
				//TODO AA : x64 support
			}
			else
			{
				_sdHost.CreateInstanceInTargetDomain<InteractionClass>().Attach(HostProcess);
			}
		}

		internal void ShowNewProjectDialog()
		{
			CommunicationService.SdaCallback.ShowNewProjectDialog();
		}

		internal void ShowOpenProjectDialog()
		{
			CommunicationService.SdaCallback.ShowOpenProjectDialog();
		}

		internal void RunBuild()
		{
			_sdHost.CreateInstanceInTargetDomain<InteractionClass>().Build();
		}

		internal void BringToFrontIDE()
		{
			_sdHost.WorkbenchVisible = true;
			_sdHost.CreateInstanceInTargetDomain<InteractionClass>().BringToFront();
		}

		internal void HideIDE()
		{

		}

		public void OnProjectSave()
		{
			
		}
	}
}
