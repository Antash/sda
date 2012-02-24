using System;
using System.Diagnostics;
using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Project.Commands;

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
				build.BuildComplete += 
					delegate(object sender, EventArgs e)
					{
						if (((Build)sender).LastBuildResults.ErrorCount == 0)
						{
							if (((Build)sender).LastBuildResults.BuiltProjects.Count > 0)
							{
								var project = ((Build)sender).LastBuildResults.BuiltProjects[0] as AbstractProject;
								if (project != null)
								{
									StateHolder.Instance.ProjectBuildState = StateHolder.ProjectBuildStates.Succeded;
									SDIntegration.Instance.CopyToIsoStorage(project.OutputAssemblyFullPath);
									SDIntegration.Instance.OnBuildSuccess(false);
								}
							}
						}
						else
						{
							StateHolder.Instance.ProjectBuildState = StateHolder.ProjectBuildStates.Fault;
							SDIntegration.Instance.OnBuildFailure();
						}
						if (StateHolder.Instance.SaveRequired())
							SDIntegration.Instance.OnProjectSave();
					};
				build.Run();
			});
		}

		public void BringToFront()
		{
			//TODO AA : implement
		}
	}
}