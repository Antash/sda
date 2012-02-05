using System.ServiceModel;
using CommunicationServices;
using System.Windows.Forms;

namespace IDEHostApplication
{
	/// <summary>
	/// Implementation of service.
	/// This class contains methods that can be invoked from ITM.
	/// All these methods are only used in IDEHost.
	/// 
	/// All invokes are non-blocking.
	/// For each method invoke new class instance is created, so you should not store here any private data.
	/// 
	/// Design of this class is such that there are no implementation details. Each method of this class 
	/// call some other method (ore set some property) of SDIntegration class. It allowed us to keep the main 
	/// idea of achitecture while translation from VSTA to SDA.
	/// </summary>
	[ServiceBehavior(
		InstanceContextMode = InstanceContextMode.PerCall,
		ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class SDAService : ISDAService
	{
		/// <summary>
		/// Opens existing project in SharpDevelop
		/// </summary>
		/// <param name="projectFileName">Path to project\solution file</param>
		public void OpenProject(string projectFileName)
		{
			SDIntegration.Instance.OpenProject(projectFileName);
		}

		/// <summary>
		/// Invokes build process
		/// </summary>
		public void Build()
		{
		}

		/// <summary>
		/// Makes SharpDevelop IDE visible
		/// </summary>
		public void ShowIDE()
		{
		}

		/// <summary>
		/// Closes currently opened solution in SharpDevelop IDE
		/// </summary>
		public void CloseWorkbench()
		{
		}

		/// <summary>
		/// Prepare closing (may be saving of previous project is needed)
		/// </summary>
		public void CloseProject()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadyToOpenProject()
		{
			return false;
		}

		/// <summary>
		/// Shuts down IDEHost
		/// </summary>
		public void Shutdown()
		{
			Application.Exit();
		}

		/// <summary>
		/// Stops the debugging process
		/// </summary>
		public void StopDebugging()
		{
		}

		/// <summary>
		/// Processes all existed breakpoints
		/// </summary>
		public void ReinitBreakpoints()
		{
		}

		/// <summary>
		/// Checks wheter #D debugger is currently attached to zebCtlNt.exe
		/// </summary>
		public bool IsAttached()
		{
			return true;
		}

		/// <summary>
		/// Set state to allow opening projects
		/// </summary>
		public void AfterProjectSaving()
		{
		}

		/// <summary>
		/// Updates signatures information for "Event-handlers signatures generator wizard"
		/// </summary>
		/// <param name="templateAssemblyPath"></param>
		public void UpdateEvHandlersGeneratorCache(string templateAssemblyPath)
		{
		}
	}
}
