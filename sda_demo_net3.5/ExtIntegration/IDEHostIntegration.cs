using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using SharpDevelopIDEHost;

namespace ExtIntegration
{
	class IDEHostIntegration
	{
		
		private static volatile IDEHostIntegration _instance;
		private static readonly object SyncRoot = new Object();

		private IDEHostIntegration()
		{
		}

		public static IDEHostIntegration Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (SyncRoot)
					{
						if (_instance == null)
							_instance = new IDEHostIntegration();
					}
				}
				return _instance;
			}
		}

		private ISDAService m_SDAManipulator;

		private ServiceHost callbackHost;
		public void InitCallbackPipe()
		{
			callbackHost = new ServiceHost(typeof(SDAServiceCallback));
			NetNamedPipeBinding callbackbinding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
			callbackbinding.ReceiveTimeout = TimeSpan.FromHours(42);
			callbackbinding.SendTimeout = TimeSpan.FromHours(42);
			callbackHost.AddServiceEndpoint(typeof(ISDAServiceCallback), callbackbinding, WCFCommunicationService.callbackAddress);

			// ERROR: Not supported in C#: OnErrorStatement

			callbackHost.Open();
		}

	}
}
