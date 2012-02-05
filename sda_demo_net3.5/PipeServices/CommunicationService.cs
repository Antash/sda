using System;
using System.ServiceModel;

namespace CommunicationServices
{
	public static class CommunicationService
	{
		public const string Address = "net.pipe://localhost/sda";
		public const string CallbackAddress = "net.pipe://localhost/sdaCallback";

		public static ISDAServiceCallback SdaCallback;

		static CommunicationService()
		{
        	var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
        	          	{ReceiveTimeout = TimeSpan.FromHours(42), SendTimeout = TimeSpan.FromHours(42)};
			var endpoint = new EndpointAddress(CallbackAddress);
			var factory = new ChannelFactory<ISDAServiceCallback>(binding, endpoint);
			SdaCallback = factory.CreateChannel();
		}
	}
}
