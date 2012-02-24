using System;
using CommunicationServices;

namespace ExtIntegration
{
	class SDAServiceCallback : ISDAServiceCallback
	{
		public void ProjectOpened()
		{
			
		}

		public void ProjectOpenError()
		{
			
		}

		public void BuildSucceded(string assemblyPath, bool isDebugging)
		{
			
		}

		public void BuildFailed()
		{
			
		}

		public void ProjectSaved(bool isLastBuildSuccess)
		{
			
		}

		public bool IsParentx64()
		{
			return IntPtr.Size == 8;
		}

		public void ShowOpenProjectDialog()
		{
			
		}

		public void ShowNewProjectDialog()
		{
			
		}
	}
}
