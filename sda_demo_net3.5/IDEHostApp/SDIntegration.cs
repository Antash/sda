using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Sda;
using Microsoft.Win32;
using IDEHostApp.Properties;
using Exception = System.Exception;
using Process = System.Diagnostics.Process;
using Timer = System.Timers.Timer;


namespace SharpDevelopIDEHost
{

	/// <summary>
	/// Here is placed implementatoin of #D integration with IDE host via SDA
	/// </summary>
	internal sealed class SDIntegration
	{
		#region fields and constants

		private const string sdConfigFile = "ICSharpCode.SharpDevelop.addin";

		private string rootPath;
		private string baseDir;
		private string dataDir;
		private string addInDir;
		private string binDir;

		private WorkbenchSettings workbenchSettings;
		private SharpDevelopHost host;
		private StartupSettings startup;
		private Process hostProcess;
		private ISynchronizeInvoke invokeTarget;
		private WCFCommunicationService comminicationService;

		internal bool suppressBuild = false;
		internal bool workbenchIsRunning = false;
		private string addInBinaryFilePath;
		internal volatile bool SaveRequired;
		internal bool IDEIsVisible;
		private string lastProjectOpened;

		internal delegate void InvokeDelegateVS(string param1);
		internal delegate void InvokeDelegateV();

		internal bool IsLastBuildSuccess;

		//Bug #1285 (Timeout to prevent deadlock while loading invalid project)
		private bool isLoadingProjectSuccess = false;
		private const int LoadingProjectTimeout = 5000;
		private System.Timers.Timer loadingProjectTimer;

		#endregion


		#region singletone definition

		private static volatile SDIntegration instance;
		private static object syncRoot = new Object();

		private SDIntegration()
		{
			loadingProjectTimer = new Timer(LoadingProjectTimeout);
			loadingProjectTimer.AutoReset = false;
			loadingProjectTimer.Elapsed += new System.Timers.ElapsedEventHandler(loadingProjectTimer_Elapsed);
		}

		public static SDIntegration Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new SDIntegration();
					}
				}

				return instance;
			}
		}

		#endregion


		#region initialisation

		public void Init(string startupPath, ISynchronizeInvoke itarget)
		{
			Init(startupPath);
			invokeTarget = itarget;
		}

		/// <summary>
		/// Preparing IDE host enviorenment
		/// </summary>
		internal void Init(string startupPath)
		{
			Console.WriteLine("Init()");

			// get #D installation path from registry
			rootPath = (string)Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\SharpDevelop.exe").GetValue("");

			baseDir = Path.GetDirectoryName(Path.GetDirectoryName(rootPath).TrimEnd('\\'));

			dataDir = baseDir + @"\data\";
			addInDir = baseDir + @"\AddIns\AddIns\";
			binDir = baseDir + @"\bin\";

			workbenchIsRunning = false;

			comminicationService = new WCFCommunicationService();
			hostProcess = Process.GetCurrentProcess().Parent();
			ConfigureEnviorenment();
		}

		/// <summary>
		/// Preparing IDE host enviorenment part 2
		/// </summary>
		private void ConfigureEnviorenment()
		{
			Console.WriteLine("ConfigureEnviorenment()");
			if (host == null)
			{
				startup = new StartupSettings
							{
								ApplicationName = "zebSharpDevelop",
								AllowUserAddIns = true,
								ConfigDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
															   "ICSharpCode/SharpDevelop3.0"),
								DataDirectory = dataDir,
								ApplicationRootPath = baseDir
							};

				startup.AddAddInsFromDirectory(addInDir);
				//Loading customised #D config file
				startup.AddAddInFile(Path.Combine(Application.StartupPath, sdConfigFile));

				var currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyResolve += LoadAssemlbyFromProductInstallationFolder;

				host = new SharpDevelopHost(AppDomain.CurrentDomain, startup) { InvokeTarget = invokeTarget };

				assignHandlers();

				workbenchSettings = new WorkbenchSettings();
			}
		}

		private void assignHandlers()
		{
			FileService.FileRenaming += new EventHandler<FileRenamingEventArgs>(FileService_FileRenaming);
			host.BeforeRunWorkbench += new EventHandler(host_BeforeRunWorkbench);
			host.WorkbenchClosed += new EventHandler(host_WorkbenchClosed);
			host.SolutionLoaded += new EventHandler(host_SolutionLoaded);
			host.StartBuild += new EventHandler(host_StartBuild);
			host.EndBuild += new EventHandler(host_EndBuild);
		}

		/// <summary>
		/// Supress renaming pjoject or solution files (bug #1347)
		/// </summary>
		void FileService_FileRenaming(object sender, FileRenamingEventArgs e)
		{
			if (e.SourceFile.ToLower().Contains("zebCtlNTAddIn.".ToLower()))
				e.Cancel = true;
		}


		void RunWorkbench()
		{
			if (!workbenchIsRunning)
				System.Threading.ThreadPool.QueueUserWorkItem(ThreadedRun);
		}

		void ThreadedRun(object state)
		{
			Console.WriteLine("Running workbench...");
			host.RunWorkbench(workbenchSettings);
		}

		#endregion

		/* 
		 * This code is needed to load references from correct place
		 * 
		 *	1. #D references from its installation path
		 *		-binaries
		 *		-addins
		 *	
		 * It just works. Don't change it. :)
		 * 
		 */
		#region asembly resolving

		public Assembly LoadAssemlbyFromProductInstallationFolder(object sender, ResolveEventArgs args)
		{
			Assembly result = null;
			if (args != null && !string.IsNullOrEmpty(args.Name))
			{
				try
				{
					var assemblyPath = FindSDAssembly(args.Name);
					if (!String.IsNullOrEmpty(assemblyPath))
						result = Assembly.LoadFrom(assemblyPath);
				}
				catch (Exception e)
				{
					//TODO MessageBox.Show(e.Message, Resources.SDIntegration_LoadAssemlbyFromProductInstallationFolder_SharpDevelop_assembly_loading_error, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			return result;
		}

		private string FindSDAssembly(string name)
		{
			var typeName = name.Split(new string[] { "," }, StringSplitOptions.None)[0];
			var assemblyPath = Path.Combine(binDir, string.Format("{0}.dll", typeName));

			var strs = typeName.Split('.');

			for (int j = strs.Length - 1; j >= 0; j--)
			{
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
				if (typeName.IndexOf(strs[j]) > 0)
					typeName = typeName.Substring(0, typeName.IndexOf(strs[j]) - 1);
			}
			if (!File.Exists(assemblyPath))
			{
				assemblyPath = string.Empty;
			}
			return assemblyPath;
		}

		#endregion


		#region SDA event handlers

		void host_SolutionLoaded(object sender, EventArgs e)
		{
			Console.WriteLine("Solution loaded at " + DateTime.Now.ToLongTimeString());

			isLoadingProjectSuccess = true;

			if (suppressBuild)
			{
				BringToFrontIDE();
			}
			else
			{
				suppressBuild = true;
				if (IDEIsVisible)
					BringToFrontIDE();
				else
					HideIDE();
				WCFCommunicationService.SdaCallback.ProjectOpened();
			}
		}

		void host_WorkbenchClosed(object sender, EventArgs e)
		{
			Console.WriteLine("Workbench closed at " + DateTime.Now.ToLongTimeString());

			workbenchIsRunning = false;
		}

		void host_BeforeRunWorkbench(object sender, EventArgs e)
		{
			Console.WriteLine("Workbench closed at " + DateTime.Now.ToLongTimeString());

			workbenchIsRunning = true;
		}

		void host_EndBuild(object sender, EventArgs e)
		{
			Console.WriteLine("Build ended at " + DateTime.Now.ToLongTimeString());
		}

		void host_StartBuild(object sender, EventArgs e)
		{
			Console.WriteLine("Build started at " + DateTime.Now.ToLongTimeString());
		}

		#endregion


		#region SDA iteraction calls

		public void AttachToHost()
		{
			if (WCFCommunicationService.SdaCallback.IsParentx64())
			{
				//TODO 
				//MessageBox.Show(Resources.SDIntegration_AttachToHost_Sorry__you_can_not_debug_x64_process, Resources.SDIntegration_AttachToHost_SDA_debug_error,
				//    MessageBoxButtons.OK,
				//    MessageBoxIcon.Error);
			}
			else
			{
				InteractionClass obj;
				obj = host.CreateInstanceInTargetDomain<InteractionClass>();
				obj.Attach(hostProcess);
			}
		}

		public void StopDebugger()
		{
			InteractionClass obj;
			obj = host.CreateInstanceInTargetDomain<InteractionClass>();
			obj.Detach();
		}

		public void RunBuild()
		{
			InteractionClass obj;
			obj = host.CreateInstanceInTargetDomain<InteractionClass>();
			obj.Build();
		}

		public void BringToFrontIDE()
		{
			host.WorkbenchVisible = true;
			InteractionClass obj;
			obj = host.CreateInstanceInTargetDomain<InteractionClass>();
			obj.BringToFront();
		}

		public void HideIDE()
		{
			Console.WriteLine("HideIDE()");
			InteractionClass obj;
			obj = host.CreateInstanceInTargetDomain<InteractionClass>();
			obj.Hide();
		}

		public void PrepareOpening()
		{
			InteractionClass obj;
			obj = host.CreateInstanceInTargetDomain<InteractionClass>();
			obj.IsSaveRequired();
		}

		#endregion


		#region Calls to WCFcommunication

		public void OpenProject(string filename)
		{

			isLoadingProjectSuccess = false;

			Console.WriteLine("Open project({0})", filename);
			if (!workbenchIsRunning)
			{
				workbenchSettings.InitialFileList.Clear();
				if (!host.IsSolutionOrProject(filename))
					WCFCommunicationService.SdaCallback.ProjectOpenError();
				else
				{
					workbenchSettings.InitialFileList.Add(filename);
					RunWorkbench();
					loadingProjectTimer.Start();
				}
			}
			else
			{
				if (host.IsSolutionOrProject(filename))
				{
					loadingProjectTimer.Start();
					host.OpenProject(filename);
				}
				else
					WCFCommunicationService.SdaCallback.ProjectOpenError();
			}
			lastProjectOpened = filename;
		}

		internal void OnBuildSuccess(bool isDebugging)
		{
			WCFCommunicationService.SdaCallback.BuildSucceded(addInBinaryFilePath, isDebugging);
		}

		public void OnBuildFailure()
		{
			WCFCommunicationService.SdaCallback.BuildFailed();
		}

		public void OnProjectSave()
		{
			SaveRequired = false;
			WCFCommunicationService.SdaCallback.ProjectSaved(IsLastBuildSuccess);
		}

		public void ShowNewProjectDialog()
		{
			WCFCommunicationService.SdaCallback.ShowNewProjectDialog();
		}

		public void ShowOpenProjectDialog()
		{
			WCFCommunicationService.SdaCallback.ShowOpenProjectDialog();
		}

		void loadingProjectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (!isLoadingProjectSuccess)
			{
				//StateHolder.Instance.PState = StateHolder.SDAProjectState.ReadyToOpen;
				WCFCommunicationService.SdaCallback.ProjectOpenError();
				BringToFrontIDE();
			}
		}

		#endregion


		#region other logic

		public void CloseProject()
		{
			StateHolder.Instance.PState = StateHolder.SDAProjectState.Opened;
			// Bug #1344
			// If previously opened project needs save - do it.
			PrepareOpening();
		}

		public bool IsReadyToOpenProject()
		{
			return StateHolder.Instance.PState == StateHolder.SDAProjectState.ReadyToOpen;
		}

		public Process HostProcess
		{
			get { return hostProcess; }
		}

		public void ShowIDE()
		{
			Console.WriteLine("ShowIDE()");

			if (!string.IsNullOrEmpty(lastProjectOpened))
			{
				if (!workbenchIsRunning)
				{
					OpenProject(lastProjectOpened);
				}
				else
				{
					BringToFrontIDE();
				}
			}
			else
			{
				//TODO 
				//MessageBox.Show(Resources.SDIntegration_ShowIDE_No_loaded_SDA_code_, Resources.SDIntegration_ShowIDE_SDA, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		public void CloseIDE()
		{
			if (!host.CloseWorkbench(false))
			{
				//TODO 
				//if (DialogResult.Yes == MessageBox.Show(Resources.SDIntegration_CloseIDE_Force_close_, Resources.SDIntegration_CloseIDE_Force, MessageBoxButtons.YesNo))
				//{
				//    host.CloseWorkbench(true);
				//}
			}
		}

		public void CopyToIsoStorage(string outputAssemblyFullPath)
		{
			addInBinaryFilePath = IsolatedStorageService.CopyFileToStorage(outputAssemblyFullPath);
		}

		/// <summary>
		/// Insertion generated event handler stub
		/// </summary>
		public void PasteEventtHandlerStub(string signatureStub)
		{
			WorkbenchSingleton.Workbench.ActiveViewContent.PrimaryFile.SaveToDisk();

			string content;

			using (var sr = new StreamReader(WorkbenchSingleton.Workbench.ActiveViewContent.PrimaryFileName))
			{
				content = sr.ReadToEnd();
			}

			var i = content.ToLower().LastIndexOf("end class");
			if (i == -1)
				i = content.Length;

			var handlerName = signatureStub.Trim().Split(new[] { ' ', '(', ')' })[2];
			if (content.ToLower().IndexOf(handlerName.ToLower()) == -1)
			{
				content = content.Insert(i, String.Format("\r\n{0}\r\n\r\n", signatureStub));

				using (var sw = new StreamWriter(WorkbenchSingleton.Workbench.ActiveViewContent.PrimaryFileName))
				{
					sw.Write(content);
				}

				WorkbenchSingleton.Workbench.ActiveViewContent.PrimaryFile.ReloadFromDisk();
			}
		}

		/// <summary>
		/// Bug #861
		/// Reassign custom breakpointHit event handler to all existed breakpoints
		/// </summary>
		public void ReinitBreakpoints()
		{
			PreparedBreakpointSet = new HashSet<object>();
			DebuggerService.BreakPointAdded += new EventHandler<BreakpointBookmarkEventArgs>(DebuggerService_BreakPointAdded);
			UpdateBreakpoints();
		}

		void DebuggerService_BreakPointAdded(object sender, BreakpointBookmarkEventArgs e)
		{
			UpdateBreakpoints();
		}

		// processed breakpoints collection
		private HashSet<object> PreparedBreakpointSet;

		public void UpdateBreakpoints()
		{
			/* Bug #861
			 * 
			 * In order to assign custom event handlers to internal #D
			 * breakpointhit event we need to get type "Debugger.BreakpointEventArgs"
			 * declared in Deburrer.Core dll which is not strongname. (while ITM assemblies are strongname)
			 * 
			 * To make reference to it we decided to create copy of SharpDevelopIDEHost project without strongname
			 * and exclude this block of code from it using preprocessor directives
			 * 
			 * So we have two copies of IDEhost project that shares(!) sources
			 *	1. Signed is used only as ITM's reference (x86 or x64) (no code is executed from it during runtime)
			 *	2. Unsigned one (x86 only(!)) does all work.
			 *	
			 */
#if SdIDEHostApp_unsigned
			// Get current #D debugger instance 
			var debugger = DebuggerService.CurrentDebugger.GetType().GetField("debugger", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(DebuggerService.CurrentDebugger);
			// Get breakpoint collection from debugger
			var breakPointColl = (IEnumerable)debugger.GetType().GetField("breakpointCollection", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(debugger);

			foreach (var bp in breakPointColl)
			{
				// delegate witch handles breakpointhit event
				var bp_debugger_BreakpointHit = new EventHandler<Debugger.BreakpointEventArgs>(
					delegate
					{
						//	MessageBox.Show("handler added");
						BringToFrontIDE();
					});

				// If current breakpoint is not processed do it
				if (!PreparedBreakpointSet.Contains(bp))
				{
					PreparedBreakpointSet.Add(bp);
					// handler assignment
					bp.GetType().GetEvent("Hit").AddEventHandler(bp, bp_debugger_BreakpointHit);
				}
			}
#endif
		}

		/// <summary>
		/// Supresses calls to ITM APC dialogs while debugging process
		/// </summary>
		internal bool ReadyToInvokeDialogs()
		{
			if (DebuggerService.CurrentDebugger.IsDebugging)
			{
				//TODO 
				//if (MessageBox.Show(
				//    Resources.SDIntegration_ReadyToInvokeDialogs_,
				//    Resources.SDIntegration_ShowIDE_SDA, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				//    return false;
				DebuggerService.CurrentDebugger.Detach();
			}
			return true;
		}

		internal bool IsAttached()
		{
			return DebuggerService.CurrentDebugger.IsDebugging;
		}

		#endregion

	}
}
