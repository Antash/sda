using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ICSharpCode.SharpDevelop.Debugging;

namespace IDEHostApplication
{
	/// <summary>
	/// Class which handles additional #D breakpoints events
	/// </summary>
	class SDBreakpointService
	{
		// processed breakpoints collection
		private HashSet<object> _preparedBreakpointSet;

		/// <summary>
		/// Reassign custom breakpointHit event handler to all existed breakpoints
		/// </summary>
		internal void ReinitBreakpoints()
		{
			_preparedBreakpointSet = new HashSet<object>();
			DebuggerService.BreakPointAdded += DebuggerServiceBreakPointAdded;
			UpdateBreakpoints();
		}

		private void DebuggerServiceBreakPointAdded(object sender, BreakpointBookmarkEventArgs e)
		{
			UpdateBreakpoints();
		}

		public void UpdateBreakpoints()
		{
			// Get current #D debugger instance 
			var fieldInfo = DebuggerService.CurrentDebugger.GetType().GetField("debugger", BindingFlags.Instance | BindingFlags.NonPublic);
			if (fieldInfo == null) return;

			var debugger = fieldInfo.GetValue(DebuggerService.CurrentDebugger);
			// Get breakpoint collection from debugger
			var field = debugger.GetType().GetField("breakpointCollection", BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null) return;

			var breakPointColl = (IEnumerable)field.GetValue(debugger);

			foreach (var bp in breakPointColl)
			{
				// delegate witch handles breakpointhit event
				var bpDebuggerBreakpointHit = new EventHandler<Debugger.BreakpointEventArgs>(
					delegate
					{
						SDIntegration.Instance.BringToFrontIDE();
					});

				// If current breakpoint is not processed do it
				if (!_preparedBreakpointSet.Contains(bp))
				{
					_preparedBreakpointSet.Add(bp);
					// handler assignment
					bp.GetType().GetEvent("Hit").AddEventHandler(bp, bpDebuggerBreakpointHit);
				}
			}
		}
	}
}
