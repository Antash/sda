using System.Windows.Forms;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using SharpDevelopIDEHost;

namespace ClassModules
{

	/// <summary>
	/// Implementation of processing calback calls from IDE host
	/// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Reentrant)]
	public class SDAServiceCallback : ISDAServiceCallback
	{

		/// <summary>
		/// If project opened successfully it is needed to initialise it's building
		/// </summary>
		public void ProjectOpened()
		{
			//MessageBox.Show("Message from #d: Project Opened")
			zebAppAPC.Instance.SDAManipulator.Build();
		}

		/// <summary>
		/// Trying to load invalid project file
		/// </summary>
		public void ProjectOpenError()
		{
			//FLS AA : closing cursor zebCursor.Instance.ShowWithText(My.Resources.str18430) from APCprojectIO.Load
			zebCursor.Instance.Close();
			MessageBox.Show("Error while SDA Project loading!", "SDA Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		/// <summary>
		/// If addin project was built successfully
		/// </summary>
		public void BuildSucceded(string assemblyPath, bool isDebugging)
		{
			//MessageBox.Show("Message from #d: Build succeeded" & vbCrLf & vbCrLf & assemblyPath)
			//FLS AA : closing cursor zebCursor.Instance.ShowWithText(My.Resources.str18430) from APCprojectIO.Load
			zebCursor.Instance.Close();

			zebApcProject actProject = zebAPCIntegration.Instance.Project;

			actProject.IsLastBuilsSucceeded = true;
			actProject.AddInProjectPath = assemblyPath;

			//If project is preparing to being debugged we need to wait for 
			//#D debugger to be attached
			//timer without autoreset is used to prevent deadlocks
			if (isDebugging)
			{
				System.Timers.Timer loadTimer = new System.Timers.Timer(42);
				loadTimer.Elapsed += loadCallback;
				loadTimer.AutoReset = false;
				loadTimer.Start();
			}
			else
			{
				actProject.LoadAddInAssembly();
			}

		}

		public void loadCallback(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Debugger.IsAttached)
			{
				//If #d is already attached we need to reprocess breakpoints and load
				//addin project
				zebAppAPC.Instance.SDAManipulator.ReinitBreakpoints();
				zebAPCIntegration.Instance.Project.LoadAddInAssembly();
			}
			else
			{
				((System.Timers.Timer)sender).Start();
			}
		}

		public void BuildFailed()
		{
			//MessageBox.Show("Message from #d: Build failed")

			//FLS AA : closing cursor zebCursor.Instance.ShowWithText(My.Resources.str18430) from APCprojectIO.Load
			zebCursor.Instance.Close();

			zebApcProject actProject = zebAPCIntegration.Instance.Project;

			actProject.IsLastBuilsSucceeded = false;
		}

		public void ProjectSaved(bool isLastBuildSuccess)
		{
			//MessageBox.Show("Message from #d: Project save" & vbCrLf & vbCrLf & isLastBuildSuccess)

			zebApcProject actProject = zebAPCIntegration.Instance.Project;

			actProject.SDAProjectSave();
		}

		/// <summary>
		/// Need to determin target architecture
		/// </summary>
		public bool IsParentx64()
		{
			return IntPtr.Size == 8;
		}

		public void ShowOpenProjectDialog()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(runeventload);
		}

		public void ShowNewProjectDialog()
		{
			System.Threading.ThreadPool.QueueUserWorkItem(runeventnew);
		}

		#region "implementation of APCUI dialog invokction"

		private void runeventnew()
		{
			runEvent(LoadTools.CreateObject<zebBaseAction>("zebApcUI.ClassModules.zebActNew").indexKey);
		}

		private void runeventload()
		{
			runEvent(LoadTools.CreateObject<zebBaseAction>("zebApcUI.ClassModules.zebActLoad").indexKey);
		}

		/// <summary>
		/// Invokes specified by key action
		/// </summary>
		private void runEvent(string key)
		{
			Form mf = (Form)zebApplication.Instance.mainForm;
			mf.Invoke(new Action<Form, string>(runEventImpl), mf, key);
		}

		/// <summary>
		/// Processes specific event in separated thread
		/// </summary>
		private void runEventImpl(Form mf, string key)
		{
			//Bring to fromt ITM main form
			mf.TopMost = true;
			mf.Show();
			zebBaseAction act = zebApplication.Instance.Item(key);
			zebIDispatcher disp = zebApplication.Instance;
			zebActionEvent aev = new zebActionEvent();
			aev.state = zebActionEvent.zebActionState.eExecute;
			//Executing event
			disp.Item(act.indexKey).execute(aev);
			mf.TopMost = false;
			//After action executing bring #D IDE back to front
			System.Threading.ThreadPool.QueueUserWorkItem(zebAppAPC.Instance.SDAManipulator.ShowIDE);
		}

		#endregion

	}

}
