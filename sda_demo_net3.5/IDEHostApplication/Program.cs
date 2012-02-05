using System;
using System.Diagnostics;
using System.Windows.Forms;
using PipeServices;

namespace IDEHostApplication
{
	internal static class Program
	{
		public static string Guid;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Debugger.Launch();

			if (args.Length == 0)
				return;

			Guid = args[0];

			var comminicationService = new WCFCommunicationService();

			Application.Run(new ApplicationContext());
		}
	}
}