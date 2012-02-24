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
			new CommunicationService(AppGuid);

			SDIntegration.Instance.foo();

			Application.Run(new IDEHostApplicationContext());
		}

		class IDEHostApplicationContext : ApplicationContext
		{
			internal IDEHostApplicationContext()
			{
				Application.ApplicationExit += Application_ApplicationExit;

				var timer = new System.Timers.Timer(1000);
				timer.Elapsed += timer_Elapsed;
				timer.Enabled = true;
			}

			void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
			{
				try
				{
					var p = Process.GetProcessById(SDIntegration.Instance.HostProcess.Id);
					if (p.ProcessName != SDIntegration.Instance.HostProcess.ProcessName)
					{
						Application.Exit();
					}
				}
				catch (Exception)
				{
					Application.Exit();
				}
			}


			void Application_ApplicationExit(object sender, EventArgs e)
			{
				try
				{
					// remove all addin assemblies from isolated storage
					IsolatedStorageService.ClearStorage();

					// close SharpDevelop IDE
					SDIntegration.Instance.CloseIDE();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Something goes wrong while closing application" + ex.Message);
				}
			}
		}

	}
}