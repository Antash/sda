using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using CommunicationServices;

namespace ExtIntegration
{
	public class IDEHostIntegration
	{

		private static volatile IDEHostIntegration _instance;
		private static readonly object SyncRoot = new Object();

		private IDEHostIntegration()
		{
			StartIDEHost();
			IDEHostCommunicationInit();
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

		private const string IDEHostApplicationName = "IDEHostApplication.exe";
		private Process _ideHostProcess;

		private void StartIDEHost()
		{
			if (_ideHostProcess != null && !_ideHostProcess.HasExited)
				_ideHostProcess.Kill();

			var ideHostStartupPath = Path.Combine(Application.StartupPath, IDEHostApplicationName);
			var ideHostStartInfo = new ProcessStartInfo(ideHostStartupPath) { UseShellExecute = false, Arguments = Guid.NewGuid().ToString() };

			_ideHostProcess = Process.Start(ideHostStartInfo);

			Application.ApplicationExit += Application_ApplicationExit;
		}

		void Application_ApplicationExit(object sender, EventArgs e)
		{
			Manipulator.Shutdown();
		}

		private void IDEHostCommunicationInit()
		{
			var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { ReceiveTimeout = TimeSpan.FromHours(42), SendTimeout = TimeSpan.FromHours(42) };

			var endpoint = new EndpointAddress(CommunicationService.Address);

			var factory = new ChannelFactory<ISDAService>(binding, endpoint);

			_sdaManipulator = factory.CreateChannel();

			InitCallbackPipe();
		}

		public ISDAService Manipulator
		{
			get { return _sdaManipulator; }
		}

		private ISDAService _sdaManipulator;

		public void InitCallbackPipe()
		{
			var callbackHost = new ServiceHost(typeof(SDAServiceCallback));
			var callbackbinding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { ReceiveTimeout = TimeSpan.FromHours(42), SendTimeout = TimeSpan.FromHours(42) };
			callbackHost.AddServiceEndpoint(typeof(ISDAServiceCallback), callbackbinding, CommunicationService.CallbackAddress);
			callbackHost.Open();
		}

		public void foo()
		{
			
		}
	}
}
