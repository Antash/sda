using System.ServiceModel;

namespace CommunicationServices
{
	/// <summary>
	/// Interface declares service contract for WCF communication between IDEHost and ITM.
	/// Methdos declared here can be invoked from ITM to manipulate SharpDevelop IDE
	/// </summary>
	[ServiceContract]
	public interface ISDAService
	{
		[OperationContract(IsOneWay = true)]
		void OpenProject(string projectFileName);

		[OperationContract(IsOneWay = true)]
		void Build();

		[OperationContract(IsOneWay = true)]
		void ShowIDE();

		[OperationContract(IsOneWay = true)]
		void CloseWorkbench();

		[OperationContract(IsOneWay = true)]
		void CloseProject();

		[OperationContract]
		bool IsReadyToOpenProject();

		[OperationContract(IsOneWay = true)]
		void Shutdown();

		[OperationContract(IsOneWay = true)]
		void UpdateEvHandlersGeneratorCache(string templateAssemblyPath);

		[OperationContract(IsOneWay = true)]
		void StopDebugging();

		[OperationContract(IsOneWay = true)]
		void ReinitBreakpoints();

		[OperationContract]
		bool IsAttached();

		[OperationContract(IsOneWay = true)]
		void AfterProjectSaving();
	}
}
