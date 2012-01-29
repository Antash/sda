namespace SharpDevelopIDEHost
{
    partial class FrmGenerateEventHandler
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGenerateEventHandler));
			this.label1 = new System.Windows.Forms.Label();
			this.lblClassName = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lstObjects = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lstEvents = new System.Windows.Forms.ListBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.txtSignature = new System.Windows.Forms.TextBox();
			this.btnStubToClipboard = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtHandlerName = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// lblClassName
			// 
			resources.ApplyResources(this.lblClassName, "lblClassName");
			this.lblClassName.Name = "lblClassName";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lstObjects);
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// lstObjects
			// 
			this.lstObjects.FormattingEnabled = true;
			resources.ApplyResources(this.lstObjects, "lstObjects");
			this.lstObjects.Name = "lstObjects";
			this.lstObjects.SelectedIndexChanged += new System.EventHandler(this.lstObjects_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lstEvents);
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			// 
			// lstEvents
			// 
			this.lstEvents.FormattingEnabled = true;
			resources.ApplyResources(this.lstEvents, "lstEvents");
			this.lstEvents.Name = "lstEvents";
			this.lstEvents.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstEvents_MouseDoubleClick);
			this.lstEvents.SelectedIndexChanged += new System.EventHandler(this.lstEvents_SelectedIndexChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.txtSignature);
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			// 
			// txtSignature
			// 
			resources.ApplyResources(this.txtSignature, "txtSignature");
			this.txtSignature.Name = "txtSignature";
			this.txtSignature.ReadOnly = true;
			this.txtSignature.TextChanged += new System.EventHandler(this.txtSignature_TextChanged);
			// 
			// btnStubToClipboard
			// 
			resources.ApplyResources(this.btnStubToClipboard, "btnStubToClipboard");
			this.btnStubToClipboard.Name = "btnStubToClipboard";
			this.btnStubToClipboard.UseVisualStyleBackColor = true;
			this.btnStubToClipboard.Click += new System.EventHandler(this.btnStubToClipboard_Click);
			// 
			// btnClose
			// 
			resources.ApplyResources(this.btnClose, "btnClose");
			this.btnClose.Name = "btnClose";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// txtHandlerName
			// 
			resources.ApplyResources(this.txtHandlerName, "txtHandlerName");
			this.txtHandlerName.Name = "txtHandlerName";
			this.txtHandlerName.TextChanged += new System.EventHandler(this.txtHandlerName_TextChanged);
			// 
			// FrmGenerateEventHandler
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtHandlerName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnStubToClipboard);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lblClassName);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmGenerateEventHandler";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtSignature;
        private System.Windows.Forms.Button btnStubToClipboard;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox lstObjects;
        private System.Windows.Forms.ListBox lstEvents;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtHandlerName;
    }
}