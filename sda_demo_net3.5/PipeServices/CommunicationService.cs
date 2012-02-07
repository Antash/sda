using System;
using System.ServiceModel;

namespace CommunicationServices
{
	public class CommunicationService
	{
		public const string AddressTemplate = "net.pipe://localhost/{0}";
		public const string CallbackAddressTemplate = "net.pipe://localhost/{0}_Callback";

		public static ISDAServiceCallback SdaCallback;

		public CommunicationService(string appGuid)
		{
        	var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
        	              	{
        	              		ReceiveTimeout = TimeSpan.FromHours(42),
								SendTimeout = TimeSpan.FromHours(42)
        	              	};
			var endpoint = new EndpointAddress(String.Format(CallbackAddressTemplate, appGuid));
			var factory = new ChannelFactory<ISDAServiceCallback>(binding, endpoint);
			SdaCallback = factory.CreateChannel();
		}
	}
}
