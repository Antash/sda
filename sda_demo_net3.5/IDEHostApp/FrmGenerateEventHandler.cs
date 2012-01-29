using System;
using System.Windows.Forms;

namespace SharpDevelopIDEHost
{

    /// <summary>
    /// GUI of "event-hendler signetures generation wizard" form.
    /// All this code is used from IDEHost.
    /// This class contains only GUI-connected code. Business logic of this wizard
    /// is located in EventHandlersGenerator.cs.
    /// </summary>
    public partial class FrmGenerateEventHandler : Form
    {
        private EventHandlersGenerator.HandlerSignatureManager signatureGenerator;


        public FrmGenerateEventHandler(EventHandlersGenerator.HandlerSignatureManager sigManager)
        {
            InitializeComponent();
            this.lblClassName.Text = sigManager.BaseClassName;
            this.signatureGenerator = sigManager;
            InitObjectsList();
        }


        void InitObjectsList()
        {
            foreach (var it in signatureGenerator.GetAllInstancedObjects())
                lstObjects.Items.Add(it);
        }


        private void fillEventsList(string instObject)
        {
            lstEvents.Items.Clear();
            foreach (var it in signatureGenerator.GetAllEvents(instObject))
                lstEvents.Items.Add(it);
        }


        private void prepareHandlerSignature(string instObject, string selectedEvent)
        {
            var argsList = signatureGenerator.GetEventSignature(instObject, selectedEvent);
            txtSignature.Text = String.Format("    Private Sub {0}{1}\r\n       ' ...\r\n    End Sub", txtHandlerName.Text, argsList);
        }


        // = = = = GUI event handlers

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void lstObjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillEventsList(lstObjects.SelectedItem as string);
        	txtSignature.Clear();
        }


        private void lstEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = lstObjects.SelectedItem as string;
            var ev = lstEvents.SelectedItem as string;
            if (obj == null || ev == null)
                return;
            txtHandlerName.Text = obj + "_" + ev;
            prepareHandlerSignature(obj, ev);
        }


        private void txtHandlerName_TextChanged(object sender, EventArgs e)
        {
            var obj = lstObjects.SelectedItem as string;
            var ev = lstEvents.SelectedItem as string;

            if (obj == null || ev == null)
                return;

            prepareHandlerSignature(obj, ev);
        }


        private void btnStubToClipboard_Click(object sender, EventArgs e)
        {
        	SDIntegration.Instance.PasteEventtHandlerStub(txtSignature.Text);
        }

		private void lstEvents_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			int index = lstEvents.IndexFromPoint(e.Location);
			if (index != ListBox.NoMatches)
			{
				btnStubToClipboard_Click(null, null);
			}
		}

		private void txtSignature_TextChanged(object sender, EventArgs e)
		{
			btnStubToClipboard.Enabled = !String.IsNullOrEmpty(txtSignature.Text);
		}

    }
}
