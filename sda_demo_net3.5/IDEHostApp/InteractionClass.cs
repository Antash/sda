using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Project.Commands;

namespace SharpDevelopIDEHost
{

    /// <summary>
    /// With help of this class IDEhost communicates with #D via SDA
    /// 
    /// 
    /// Main idea of working with SDA 
    /// 
    /// All methods have to be defined twice:
    ///		1. Method with some logic
	///			(So this code would be run in #D application domain)
	///		2. Method invokator
	///			(invokes first mathod through WorkbenchSingleton.SafeThreadAsyncCall)
	///			
    /// </summary>
	public class InteractionClass : MarshalByRefObject
	{
		// Some winapi imports to manipulate #D window.
		#region WINAPI imports

		private const int SW_SHOWNORMAL = 1;
		private const int SW_SHOW = 5;
		private const int SW_HIDE = 0;
		private const int SW_SHOWNOACTIVATE = 4;

		private const int HWND_NOTOPMOST = -2;
		private const int HWND_TOPMOST = -1;

		private const int HWND_TOP = 0;
		private const uint SWP_NOACTIVATE = 0x0010;
		private const uint SWP_SHOWWINDOW = 0x0040;

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		static extern bool SetWindowPos(
			 IntPtr hWnd,           // window handle
			 int hWndInsertAfter,   // placement-order handle
			 int X,					// horizontal position
			 int Y,					// vertical position
			 int cx,				// width
			 int cy,				// height
			 uint uFlags);			// window positioning flags

		[DllImport("user32.dll")]
		private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		#endregion


		public void BringToFront()
		{
			SDIntegration.Instance.IDEIsVisible = true;
			WorkbenchSingleton.SafeThreadAsyncCall(BringToFrontInternal);
		}

		void BringToFrontInternal()
		{
			WorkbenchSingleton.MainForm.ShowInTaskbar = true;
			WorkbenchSingleton.MainForm.Activate();
			WorkbenchSingleton.MainForm.BringToFront();
			//ShowWindowAsync(WorkbenchSingleton.MainForm.Handle, SW_SHOW);
			//WorkbenchSingleton.MainForm.Refresh();

			SetWindowPos(WorkbenchSingleton.MainForm.Handle,
						HWND_TOPMOST,
						WorkbenchSingleton.MainForm.Left,
						WorkbenchSingleton.MainForm.Top,
						WorkbenchSingleton.MainForm.Width,
						WorkbenchSingleton.MainForm.Height,
						SWP_SHOWWINDOW);
			ShowWindowAsync(WorkbenchSingleton.MainForm.Handle, SW_SHOWNORMAL);

			SetWindowPos(WorkbenchSingleton.MainForm.Handle,
						HWND_NOTOPMOST,
						WorkbenchSingleton.MainForm.Left,
						WorkbenchSingleton.MainForm.Top,
						WorkbenchSingleton.MainForm.Width,
						WorkbenchSingleton.MainForm.Height,
						SWP_SHOWWINDOW);

			ShowWindowAsync(WorkbenchSingleton.MainForm.Handle, SW_SHOWNORMAL);

			SetActiveWindow(WorkbenchSingleton.MainForm.Handle);
			//WorkbenchSingleton.MainForm.ShowInTaskbar = true;
			//WorkbenchSingleton.MainForm.Refresh();
		}

		public void Hide()
		{
			SDIntegration.Instance.IDEIsVisible = false;
			WorkbenchSingleton.SafeThreadAsyncCall(HideInternal);
		}

		public void HideInternal()
		{
			WorkbenchSingleton.MainForm.ShowInTaskbar = false;
			ShowWindowAsync(WorkbenchSingleton.MainForm.Handle, SW_HIDE);
		}

		public void Attach(Process process)
		{
			WorkbenchSingleton.SafeThreadAsyncCall(AttachInternal, process);
		}

		void AttachInternal(Process process)
		{
			DebuggerService.CurrentDebugger.Attach(process);
		}

		public void Detach()
		{
			WorkbenchSingleton.SafeThreadAsyncCall(DetachInternal);
		}

		void DetachInternal()
		{
			DebuggerService.CurrentDebugger.Detach();
		}

		public void Build()
		{
			WorkbenchSingleton.SafeThreadAsyncCall(BuildInternal);
		}

		void BuildInternal()
		{
			var build = new SDBuild();
			build.BuildComplete += new EventHandler(build_BuildComplete);
			build.Run();
		}

		void build_BuildComplete(object sender, EventArgs e)
		{
			if (((Build)sender).LastBuildResults.ErrorCount == 0)
			{
				if (((Build)sender).LastBuildResults.BuiltProjects.Count > 0)
				{
					var project = ((Build)sender).LastBuildResults.BuiltProjects[0] as AbstractProject;
					if (project != null)
					{
						SDIntegration.Instance.IsLastBuildSuccess = true;
						SDIntegration.Instance.CopyToIsoStorage(project.OutputAssemblyFullPath);
						SDIntegration.Instance.OnBuildSuccess(false);
						if (SDIntegration.Instance.SaveRequired)
							SDIntegration.Instance.OnProjectSave();
						return;
					}
				}
			}
			SDIntegration.Instance.IsLastBuildSuccess = false;
			SDIntegration.Instance.OnBuildFailure();
			if (SDIntegration.Instance.SaveRequired)
				SDIntegration.Instance.OnProjectSave();
		}

		public void IsSaveRequired()
		{
			if (SDIntegration.Instance.workbenchIsRunning)
				WorkbenchSingleton.SafeThreadCall(IsSaveRequiredInternal);
			else
				StateHolder.Instance.PState = StateHolder.SDAProjectState.ReadyToOpen;
		}

		void IsSaveRequiredInternal()
		{
			if (WorkbenchSingleton.Workbench.ActiveViewContent.IsDirty)
			{
				SDIntegration.Instance.SaveRequired = true;
				BuildInternal();
			}
			else
			{
				StateHolder.Instance.PState = StateHolder.SDAProjectState.ReadyToOpen;
			}
		}
	}
}