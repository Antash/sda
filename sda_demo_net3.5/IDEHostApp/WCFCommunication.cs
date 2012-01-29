using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace SharpDevelopIDEHost
{
    /// <summary>
    /// Interface declares service contract for WCF communication between IDEHost and ITM.
    /// Methdos declared here can be invoked from ITM to manipulate SharpDevelop IDE
    /// </summary>
    [ServiceContract]
    public interface ISDAService
    {
        [OperationContract]
        string TestService(string name);

        [OperationContract(IsOneWay = true)]
        void OpenProject(string projectFileName );

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
        /// Test method.
        /// Needed for testing WCF functionality.
        /// For debug purposes only.
        /// </summary>
        /// <param name="testVal">dummy parameter</param>
        /// <returns>dummy return value</returns>
		public string TestService(string testVal)
		{
            Console.WriteLine("Testing service, input string {0}", testVal);
            return "Service tested. " + testVal;
		}


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



    /// <summary>
    /// Class contains initialization code for WCF communication mechanism.
    /// This code is used only in IDEHost.
    /// </summary>
	public class WCFCommunicationService
	{
		private ServiceHost host;
		private NetNamedPipeBinding binding;
		public const string address = "net.pipe://localhost/sda";
		public const string callbackAddress = "net.pipe://localhost/sdaCallback";
		private static ISDAServiceCallback m_SDACallback;


        /// <summary>
        /// Constructor
        /// </summary>
		public WCFCommunicationService()
		{
			init();
			initCallback();
		}


        /// <summary>
        /// Returns communication object that allows user to send events from IDEHost to ITM
        /// </summary>
		public static ISDAServiceCallback SdaCallback
		{
			get
			{
				return m_SDACallback;
			}
		}


        /// <summary>
        /// Initialize service. Starts communication server that will handle requests from ITM.
        /// </summary>
        public void init()
        {
            host = new ServiceHost(typeof(SDAService));
            binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
			binding.ReceiveTimeout = TimeSpan.FromHours(42);
			binding.SendTimeout = TimeSpan.FromHours(42);
            host.AddServiceEndpoint(typeof(ISDAService), binding, address);
            host.Open();
        }



        /// <summary>
        /// Initialize channel for callback mechanism that will raise events from IDEHost to ITM.
        /// </summary>
		public void initCallback()
		{
			binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            binding.ReceiveTimeout = TimeSpan.FromHours(42);
			binding.SendTimeout = TimeSpan.FromHours(42);
			var endpoint = new EndpointAddress(callbackAddress);
			var factory = new ChannelFactory<ISDAServiceCallback>(binding, endpoint);
			m_SDACallback = factory.CreateChannel();
		}
	}
}
