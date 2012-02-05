using System;
using System.Diagnostics;
using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Gui;

namespace IDEHostApplication
{
	/// <summary>
	/// With help of this class IDEhost communicates with #D via SDA		
	/// </summary>
	class InteractionClass : MarshalByRefObject
	{
		public void Attach(Process process)
		{
			WorkbenchSingleton.SafeThreadAsyncCall(p => DebuggerService.CurrentDebugger.Attach(p), process);
		}

		public void Detach()
		{
			WorkbenchSingleton.SafeThreadAsyncCall(() => DebuggerService.CurrentDebugger.Detach());
		}

		public void Build()
		{
			WorkbenchSingleton.SafeThreadAsyncCall(() =>
			{
				var build = new SDBuild();
				build.BuildComplete += build_BuildComplete;
				build.Run();
			});
		}

		void build_BuildComplete(object sender, EventArgs e)
		{
		}

		//void build_BuildComplete(object sender, EventArgs e)
		//{
		//    if (((Build)sender).LastBuildResults.ErrorCount == 0)
		//    {
		//        if (((Build)sender).LastBuildResults.BuiltProjects.Count > 0)
		//        {
		//            var project = ((Build)sender).LastBuildResults.BuiltProjects[0] as AbstractProject;
		//            if (project != null)
		//            {
		//                SDIntegration.Instance.IsLastBuildSuccess = true;
		//                SDIntegration.Instance.CopyToIsoStorage(project.OutputAssemblyFullPath);
		//                SDIntegration.Instance.OnBuildSuccess(false);
		//                if (SDIntegration.Instance.SaveRequired)
		//                    SDIntegration.Instance.OnProjectSave();
		//                return;
		//            }
		//        }
		//    }
		//    SDIntegration.Instance.IsLastBuildSuccess = false;
		//    SDIntegration.Instance.OnBuildFailure();
		//    if (SDIntegration.Instance.SaveRequired)
		//        SDIntegration.Instance.OnProjectSave();
		//}

		//public void IsSaveRequired()
		//{
		//    if (SDIntegration.Instance.workbenchIsRunning)
		//        WorkbenchSingleton.SafeThreadCall(IsSaveRequiredInternal);
		//    else
		//        StateHolder.Instance.PState = StateHolder.SDAProjectState.ReadyToOpen;
		//}

		//void IsSaveRequiredInternal()
		//{
		//    if (WorkbenchSingleton.Workbench.ActiveViewContent.IsDirty)
		//    {
		//        SDIntegration.Instance.SaveRequired = true;
		//        BuildInternal();
		//    }
		//    else
		//    {
		//        StateHolder.Instance.PState = StateHolder.SDAProjectState.ReadyToOpen;
		//    }
		//}
	}
}