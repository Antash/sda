﻿using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows.Forms;
using PipeServices;

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

		private Process _ideHostProcess;

		private void StartIDEHost()
		{
			if (_ideHostProcess != null && !_ideHostProcess.HasExited)
				_ideHostProcess.Kill();

			//TODO AA : remove hardcode
			const string p = @"C:\Users\anton\Documents\Visual Studio 2010\Projects\sda_demo\sda_demo_net3.5\IDEHostApplication\bin\Debug\IDEHostApplication.exe";
			var ideHostStartInfo = new ProcessStartInfo(p) { UseShellExecute = false, Arguments = Guid.NewGuid().ToString() };

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

			var endpoint = new EndpointAddress(WCFCommunicationService.address);

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
			callbackHost.AddServiceEndpoint(typeof(ISDAServiceCallback), callbackbinding, WCFCommunicationService.callbackAddress);
			callbackHost.Open();
		}

		public void foo()
		{
			
		}
	}
}
