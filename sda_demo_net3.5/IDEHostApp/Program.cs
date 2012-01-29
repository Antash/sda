using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SharpDevelopIDEHost
{
    /// <summary>
    /// SharpDevelopIDEHost entry point
    /// </summary>
	class Program
	{
		[STAThread]
		public static void Main(string[] args)
		{

			// Bug #1346
			// If parent application is not ITM than exit
			if (!Process.GetCurrentProcess().Parent().ProcessName.ToLower().Contains("zeb"))
				return;

			Console.WriteLine("Welcome to #D IDE Host application!");
			SDIntegration.Instance.Init(Application.StartupPath);

			Application.EnableVisualStyles();
			Application.Run(new IDEHostApplicationContext());
		}


		class IDEHostApplicationContext : ApplicationContext
		{
			internal IDEHostApplicationContext()
			{
				Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

				var timer = new System.Timers.Timer(1000);
				timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
				timer.Enabled = true;
			}


            /// <summary>
            /// Check whether ITM that started this instance of IDEHost is alive. Otherwise exit from IDEHost.
            /// It is needed to kill IDEHost that was started by ITM that suddenly crashed.
            /// </summary>
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