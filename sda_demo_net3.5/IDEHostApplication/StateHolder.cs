using System;

namespace IDEHostApplication
{
	internal class StateHolder
	{
		enum IDEHostApplicationState
		{
			Suspended,
			Initializing,
			Running,
			Suspending,
			Fault
		}

		enum SDWorkbenchWindowState
		{
			Closed,
			Opened,
			Active,
			Hiden
		}

		enum ProjectState
		{
			Dirty,
			Clean,
			Invalid,
			Closed
		}

		enum ProjectBuildState
		{
			NotBuilded,
			Succeded,
			Fault
		}

		enum ProjectExecutionState
		{
			Stopped,
			Running,
			Paused,
		}

		private IDEHostApplicationState _ideHostAppState;
		private SDWorkbenchWindowState _sdWorkbenchWindowState;
		private ProjectState _projectState;
		private ProjectBuildState _projectBuildState;
		private ProjectExecutionState _projectExecutionState;

		private static volatile StateHolder _instance;
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
	}
}
