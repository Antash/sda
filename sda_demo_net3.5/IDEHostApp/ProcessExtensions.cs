using System.Diagnostics;

namespace SharpDevelopIDEHost
{
    /// <summary>
    /// Helper class for work with Windows processes
    /// 
	/// This code provides a nice interface for finding the Parent process object 
	/// and takes into account the possibility of multiple processes with the same name
    /// </summary>
	public static class ProcessExtensions
	{

        /// <summary>
        /// Returns indexed process name for specified PID
        /// (Index is needed when there are more than one process with the same name)
        /// </summary>
		private static string FindIndexedProcessName(int pid)
		{
			var processName = Process.GetProcessById(pid).ProcessName;
			var processesByName = Process.GetProcessesByName(processName);
			string processIndexdName = null;

			for (var index = 0; index < processesByName.Length; index++)
			{
				processIndexdName = index == 0 ? processName : processName + "#" + index;
				var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
				if ((int)processId.NextValue() == pid)
				{
					return processIndexdName;
				}
			}

			return processIndexdName;
		}

		/// <summary>
		/// Gets parent process for specified indexed process name
		/// </summary>
		private static Process FindPidFromIndexedProcessName(string indexedProcessName)
		{
			var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
			return Process.GetProcessById((int)parentId.NextValue());
		}

		/// <summary>
		/// Returns parent process instance for specified process
		/// </summary>
		public static Process Parent(this Process process)
		{
			return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
		}
	}
}
