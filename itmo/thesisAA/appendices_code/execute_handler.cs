/// <summary>
/// Execute event ("Play" button on SharpDevelop toolbox).
/// 
/// Firstly, invokes Build process. 
/// If there are compilation errors, notify Ext. Application about this fact.
/// Otherwise, attaches SDA debugger to Ext. Application and notify it about successfull add-in build.
/// </summary>
class ExecuteHandler : Execute
{
	public override void Run()
	{
		var build = new SDBuild();
		build.BuildComplete += 
			delegate
			{
				if (build.LastBuildResults.ErrorCount == 0)
				{
					if (build.LastBuildResults.BuiltProjects.Count > 0)
					{
						var project = build.LastBuildResults.BuiltProjects[0] as AbstractProject;
						if (project != null)
						{
							// If build successfull we need to copy assembly and debug info to iso storage
							// Than attach debugger and finaly notify Ext. Application to start loading addin assembly
							// This order is needed to debug addin from the very beginning.
							SDIntegration.Instance.CopyToIsoStorage(project.OutputAssemblyFullPath);
							SDIntegration.Instance.AttachToHost();
							SDIntegration.Instance.OnBuildSuccess(true);
						}
					}
					LoggingService.Info("Debugger Command: Start (withDebugger=" + withDebugger + ")");
				}
				else
				{
					SDIntegration.Instance.OnBuildFailure();
				}
			};
		build.Run();
	}
}