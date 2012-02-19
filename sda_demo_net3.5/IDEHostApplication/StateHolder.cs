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

		private string _lastProjectOpened;

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
			_lastProjectOpened = null;
			_ideHostAppState = IDEHostApplicationStates.NotInitialized;
			_sdWorkbenchWindowState = SDWorkbenchWindowStates.Closed;
			_projectState = ProjectStates.Closed;
			_projectBuildState = ProjectBuildStates.NotBuilded;
			_projectExecutionState = ProjectExecutionStates.Stopped;
		}

		public bool ChangeIDEHostApplicationState(IDEHostApplicationStates newState)
		{
			switch (newState)
			{
				case IDEHostApplicationStates.Running:
					if (_ideHostAppState != IDEHostApplicationStates.Initialized)
						return false;
					break;
			}
			_ideHostAppState = newState;
			return true;
		}
	}
}
