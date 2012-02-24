using ICSharpCode.SharpDevelop.Debugging;
using ICSharpCode.SharpDevelop.Project.Commands;

namespace IDEHostApplication
{
	/// <summary>
	/// Redefine SharpDevelop IDE behaviour for command "Build".
	/// Detach attached to ITM debugger (if any) before invoking add-in build process.
	/// </summary>
	public class SDBuild : Build
	{
		public override void Run()
		{
			if (DebuggerService.CurrentDebugger.IsDebugging)
				DebuggerService.CurrentDebugger.Detach();
			base.Run();
		}
	}
}
