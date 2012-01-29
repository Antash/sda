using System;

namespace SharpDevelopIDEHost
{
	/// <summary>
	/// This class holds current state of the SDA project to syncronize actions with it
	/// </summary>
	internal class StateHolder
	{

		public enum SDAProjectState
		{
			NoState,
			Opened,
			ReadyToOpen
		}

		private static volatile StateHolder instance;
		private static object syncRoot = new Object();

		private StateHolder()
		{
			PState = SDAProjectState.ReadyToOpen;
		}

		public static StateHolder Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new StateHolder();
					}
				}

				return instance;
			}
		}

		internal volatile SDAProjectState PState;

	}
}
