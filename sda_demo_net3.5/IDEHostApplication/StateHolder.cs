using System;

namespace IDEHostApplication
{
	internal class StateHolder
	{
		internal enum IDEHostApplicationStates
		{
			NotInitialized,
			Initializing,
			Initialized,
			Running,
			Suspending,
			Suspended,
			Fault
		}

		internal enum SDWorkbenchWindowStates
		{
			Closed,
			Opened,
			Active,
			Hiden
		}

		internal enum ProjectStates
		{
			Dirty,
			Clean,
			Invalid,
			Closed
		}

		internal enum ProjectBuildStates
		{
			NotBuilded,
			Building,
			Succeded,
			Fault
		}

		internal enum ProjectExecutionStates
		{
			Stopped,
			Running,
			Paused,
		}

		private IDEHostApplicationStates _ideHostAppState;
		private SDWorkbenchWindowStates _sdWorkbenchWindowState;
		private ProjectStates _projectState;
		private ProjectBuildStates _projectBuildState;
		private ProjectExecutionStates _projectExecutionState;

		public string LastProjectOpened { get; set; }

		private static volatile StateHolder _instance = new StateHolder();
		private static readonly object SyncRoot = new Object();

		public static StateHolder Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new StateHolder();
					}
				}

				return _instance;
			}
		}

		protected StateHolder()
		{
			_ideHostAppState = IDEHostApplicationStates.NotInitialized;
			_sdWorkbenchWindowState = SDWorkbenchWindowStates.Closed;
			_projectState = ProjectStates.Closed;
			_projectBuildState = ProjectBuildStates.NotBuilded;
			_projectExecutionState = ProjectExecutionStates.Stopped;
		}

		public IDEHostApplicationStates IDEHostAppState
		{ 
			get { return _ideHostAppState; }
			set { _ideHostAppState = value; }
		}

		public SDWorkbenchWindowStates SDWorkbenchWindowState
		{
			get { return _sdWorkbenchWindowState; }
		}

		public ProjectStates ProjectState
		{
			get { return _projectState; }
			set { _projectState = value; }
		}

		public ProjectBuildStates ProjectBuildState
		{
			get { return _projectBuildState; }
		}

		public ProjectExecutionStates ProjectExecutionState
		{
			get { return _projectExecutionState; }
		}

		public bool CanOpenProject()
		{
			//TODO AA : implement
			return true;
		}

		public bool CanShowIDE()
		{
			//TODO AA : implement
			return true;
		}

		public bool CanInvokeDialogs()
		{
			//TODO AA : implement
			return true;
		}
	}
}
