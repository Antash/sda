using System;
using System.Diagnostics;
using System.Windows.Forms;
using CommunicationServices;

namespace IDEHostApplication
{
	internal static class Program
	{
		// TODO AA : review guid usage
		public static string AppGuid;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Debugger.Launch();

			if (args.Length == 0)
				return;

			AppGuid = args[0];
			var cs = new CommunicationService(AppGuid);

			SDIntegration.Instance.Foo();

			Application.Run(new ApplicationContext());
		}
	}
}