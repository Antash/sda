using System.ServiceModel;

namespace PipeServices
{
	/// <summary>
	/// Interface declares service contract for WCF communication between IDEHost and ITM.
	/// Methdos declared here can be invoked from SharpDevelop to inform ITM about some events.
	/// </summary>
	[ServiceContract]
	public interface ISDAServiceCallback
	{
		[OperationContract(IsOneWay = true)]
		void ProjectOpened();

		[OperationContract(IsOneWay = true)]
		void ProjectOpenError();

		[OperationContract(IsOneWay = true)]
		void BuildSucceded(string assemblyPath, bool isDebugging);

		[OperationContract(IsOneWay = true)]
		void BuildFailed();

		[OperationContract(IsOneWay = true)]
		void ProjectSaved(bool isLastBuildSuccess);

		[OperationContract]
		bool IsParentx64();

		[OperationContract(IsOneWay = true)]
		void ShowOpenProjectDialog();

		[OperationContract(IsOneWay = true)]
		void ShowNewProjectDialog();
	}
}
