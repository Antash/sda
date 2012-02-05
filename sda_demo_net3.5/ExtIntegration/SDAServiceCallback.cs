using CommunicationServices;

namespace ExtIntegration
{
	class SDAServiceCallback : ISDAServiceCallback
	{
		public void ProjectOpened()
		{
			throw new System.NotImplementedException();
		}

		public void ProjectOpenError()
		{
			throw new System.NotImplementedException();
		}

		public void BuildSucceded(string assemblyPath, bool isDebugging)
		{
			throw new System.NotImplementedException();
		}

		public void BuildFailed()
		{
			throw new System.NotImplementedException();
		}

		public void ProjectSaved(bool isLastBuildSuccess)
		{
			throw new System.NotImplementedException();
		}

		public bool IsParentx64()
		{
			throw new System.NotImplementedException();
		}

		public void ShowOpenProjectDialog()
		{
			throw new System.NotImplementedException();
		}

		public void ShowNewProjectDialog()
		{
			throw new System.NotImplementedException();
		}
	}
}
