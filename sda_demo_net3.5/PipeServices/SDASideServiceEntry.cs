using System;
using System.ServiceModel;

namespace PipeServices
{
    /// <summary>
    /// Class contains initialization code for WCF communication mechanism.
    /// This code is used only in IDEHost.
    /// </summary>
	public class WCFCommunicationService
	{
		private ServiceHost host;
		private NetNamedPipeBinding binding;
		public const string address = "net.pipe://localhost/sda";
		public const string callbackAddress = "net.pipe://localhost/sdaCallback";
		private static ISDAServiceCallback m_SDACallback;


        /// <summary>
        /// Constructor
        /// </summary>
		public WCFCommunicationService()
		{
			init();
			initCallback();
		}

        /// <summary>
        /// Returns communication object that allows user to send events from IDEHost to ITM
        /// </summary>
		public static ISDAServiceCallback SdaCallback
		{
			get
			{
				return m_SDACallback;
			}
		}

        /// <summary>
        /// Initialize service. Starts communication server that will handle requests from ITM.
        /// </summary>
        public void init()
        {
            host = new ServiceHost(typeof(SDAService));
            binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
			binding.ReceiveTimeout = TimeSpan.FromHours(42);
			binding.SendTimeout = TimeSpan.FromHours(42);
            host.AddServiceEndpoint(typeof(ISDAService), binding, address);
            host.Open();
        }

        /// <summary>
        /// Initialize channel for callback mechanism that will raise events from IDEHost to ITM.
        /// </summary>
		public void initCallback()
		{
			binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.ReceiveTimeout = TimeSpan.FromHours(42);
			binding.SendTimeout = TimeSpan.FromHours(42);
			var endpoint = new EndpointAddress(callbackAddress);
			var factory = new ChannelFactory<ISDAServiceCallback>(binding, endpoint);
			m_SDACallback = factory.CreateChannel();
		}
	}
}
