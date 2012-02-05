﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExtIntegration;

namespace ExtApp
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			IDEHostIntegration.Instance.foo();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
