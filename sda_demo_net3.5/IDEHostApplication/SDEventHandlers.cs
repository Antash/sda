using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Commands;
using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Project.Commands;

/*
 * This file contains a number of event-handlers that modify SharpDevelop IDE behaviour
 * for some commonly used actions.
 * 
 * Code from this file does not used directly by IDEHost. This classes are used by SharpDevelop IDE.
 */

namespace IDEHostApplication
{
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


	/// <summary>
	/// Handles "stop" action.
	/// 
	/// Only detach debugger from Ext. Application.
	/// </summary>
	public class StopDebuggingCommandHandler : StopDebuggingCommand
	{
		public override void Run()
		{
			LoggingService.Info("Debugger Command: Stop");
			DebuggerService.CurrentDebugger.Detach();
		}
	}


	/// <summary>
	/// Pos-Build action.
	/// 
	/// Notifies Ext. Application about successfull\unsuccessfull build.
	/// </summary>
	public class BuildProjectHandler : SDBuild
	{
		public override void AfterBuild()
		{
			if (LastBuildResults.ErrorCount == 0)
			{
				if (LastBuildResults.BuiltProjects.Count > 0)
				{
					var project = LastBuildResults.BuiltProjects[0] as AbstractProject;
					if (project != null)
					{
						SDIntegration.Instance.CopyToIsoStorage(project.OutputAssemblyFullPath);
						SDIntegration.Instance.OnBuildSuccess(false);
					}
				}
			}
			else
			{
				SDIntegration.Instance.OnBuildFailure();
			}
			base.AfterBuild();
		}
	}


	/// <summary>
	/// Save action.
	/// 
	/// 1. Save files
	/// 2. Invoke build process (it is a part of logic - before saving 
	///    we need to check whether script contains compilation errors and inform user about it if any.)
	/// 3. Copy files to zip archive\database\etc in case of user wishes.
	/// </summary>
	public class SaveAllFilesHandler : SaveAllFiles
	{
		public override void Run()
		{
			if (DebuggerService.CurrentDebugger.IsDebugging && !DebuggerService.CurrentDebugger.IsProcessRunning)
				return;
			base.Run();
			StateHolder.Instance.ProjectState = StateHolder.ProjectStates.Dirty;
			SDIntegration.Instance.RunBuild();
		}
	}


	/// <summary>
	/// "Event-handler signature generator wizard" button click
	/// 
	/// Checks if wizard available for currently opened code file and invoke wizard if possible.
	/// </summary>
	public class EventSignatureGenHandler : AbstractCommand
	{
		public override void Run()
		{
			//TODO AA : implement as #D addin
			//try
			//{
			//    bool success = false;

			//    var activeWnd = WorkbenchSingleton.Workbench.ActiveViewContent;
			//    if (activeWnd != null)
			//    {
			//        var className = Path.GetFileNameWithoutExtension(activeWnd.PrimaryFileName);
			//        var gen = EventHandlersGenerator.HandlerSignatureCollection.Instance.GetEvHandlersManager(className);
			//        if (gen != null)
			//        {
			//            success = true;
			//            var frm = new FrmGenerateEventHandler(gen);
			//            frm.ShowDialog();
			//        }
			//    }

			//    if (!success)
			//        MessageBox.Show("Wizard is not available for current code file", "SDA Integration: event-handler signature generation wizard", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

			//}
			//catch (Exception er)
			//{
			//    MessageBox.Show("Error while initializing wizard window.\n\n" + er.Message, "SDA Integration: event-handler signature generation wizard", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//}
		}
	}


	/// <summary>
	/// "Open" action
	/// 
	/// Show "open project" dialog
	/// </summary>
	public class OpenSDAProjectHandler : AbstractCommand
	{
		public override void Run()
		{
			if (StateHolder.Instance.CanInvokeDialogs())
				SDIntegration.Instance.ShowOpenProjectDialog();
		}
	}


	/// <summary>
	/// "New" action
	/// 
	/// Show "new project" dialog
	/// </summary>
	public class NewSDAProjectHandler : AbstractCommand
	{
		public override void Run()
		{
			if (StateHolder.Instance.CanInvokeDialogs())
				SDIntegration.Instance.ShowNewProjectDialog();
		}
	}
}
