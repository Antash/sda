using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace PipeServices
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
			Console.WriteLine("Command from pipe: OpenProject({0})", projectFileName);
			SDIntegration.Instance.suppressBuild = false;
			SDIntegration.InvokeDelegateVS inv = SDIntegration.Instance.OpenProject;
			inv.Invoke(projectFileName);
		}


		/// <summary>
		/// Invokes build process
		/// </summary>
		public void Build()
		{
			Console.WriteLine("Command from pipe: Build()");
			SDIntegration.InvokeDelegateV inv = SDIntegration.Instance.RunBuild;
			inv.Invoke();
		}


		/// <summary>
		/// Makes SharpDevelop IDE visible
		/// </summary>
		public void ShowIDE()
		{
			Console.WriteLine("Command from pipe: ShowIDE()");

			SDIntegration.InvokeDelegateV inv = SDIntegration.Instance.ShowIDE;
			inv.Invoke();
		}


		/// <summary>
		/// Closes currently opened solution in SharpDevelop IDE
		/// </summary>
		public void CloseWorkbench()
		{
			Console.WriteLine("Command from pipe: CloseSolution()");

			SDIntegration.InvokeDelegateV inv = SDIntegration.Instance.CloseIDE;
			inv.Invoke();
		}

		/// <summary>
		/// Prepare closing (may be saving of previous project is needed)
		/// </summary>
		public void CloseProject()
		{
			SDIntegration.Instance.CloseProject();
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadyToOpenProject()
		{
			return SDIntegration.Instance.IsReadyToOpenProject();
		}

		/// <summary>
		/// Shuts down IDEHost
		/// </summary>
		public void Shutdown()
		{
			Console.WriteLine("Command from pipe: Shutdown()");

			SDIntegration.InvokeDelegateV inv = Application.Exit;
			inv.Invoke();
		}


		/// <summary>
		/// Stops the debugging process
		/// </summary>
		public void StopDebugging()
		{
			Console.WriteLine("Command from pipe: StopDebugging()");

			SDIntegration.InvokeDelegateV inv = SDIntegration.Instance.StopDebugger;
			inv.Invoke();
		}


		/// <summary>
		/// Processes all existed breakpoints
		/// </summary>
		public void ReinitBreakpoints()
		{
			SDIntegration.Instance.ReinitBreakpoints();
		}

		/// <summary>
		/// Checks wheter #D debugger is currently attached to zebCtlNt.exe
		/// </summary>
		public bool IsAttached()
		{
			return SDIntegration.Instance.IsAttached();
		}

		/// <summary>
		/// Set state to allow opening projects
		/// </summary>
		public void AfterProjectSaving()
		{
			StateHolder.Instance.PState = StateHolder.SDAProjectState.ReadyToOpen;
		}


		/// <summary>
		/// Updates signatures information for "Event-handlers signatures generator wizard"
		/// </summary>
		/// <param name="templateAssemblyPath"></param>
		public void UpdateEvHandlersGeneratorCache(string templateAssemblyPath)
		{
			try
			{
				EventHandlersGenerator.HandlerSignatureCollection.Instance.Initialize(templateAssemblyPath);
			}
			catch (Exception err)
			{
				MessageBox.Show("Event handler generation wizard failed to update its cache\n\n" + err.Message, "SDA Integration", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
