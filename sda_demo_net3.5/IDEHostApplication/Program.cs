using System;
using System.Windows.Forms;

namespace IDEHostApplication
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			var a = new SDIntegration();

			Application.Run(new ApplicationContext());
		}
	}
}