﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.1433.
// 
#pragma warning disable 1591

namespace CodeFormatServiceClient.ICSharpCode.CodeFormat {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CodeFormatServiceSoap", Namespace="http://codeconverter.sharpdevelop.net/")]
    public partial class CodeFormatService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback FormatOperationCompleted;
        
        private System.Threading.SendOrPostCallback RetrieveAvailableHighlightersOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CodeFormatService() {
            this.Url = global::CodeFormatServiceClient.Properties.Settings.Default.CodeFormatServiceClient_ICSharpCode_CodeFormat_CodeFormatService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event FormatCompletedEventHandler FormatCompleted;
        
        /// <remarks/>
        public event RetrieveAvailableHighlightersCompletedEventHandler RetrieveAvailableHighlightersCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://codeconverter.sharpdevelop.net/Format", RequestNamespace="http://codeconverter.sharpdevelop.net/", ResponseNamespace="http://codeconverter.sharpdevelop.net/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Format(string Document, string HighlighterName, bool IncludeLineNumbers) {
            object[] results = this.Invoke("Format", new object[] {
                        Document,
                        HighlighterName,
                        IncludeLineNumbers});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void FormatAsync(string Document, string HighlighterName, bool IncludeLineNumbers) {
            this.FormatAsync(Document, HighlighterName, IncludeLineNumbers, null);
        }
        
        /// <remarks/>
        public void FormatAsync(string Document, string HighlighterName, bool IncludeLineNumbers, object userState) {
            if ((this.FormatOperationCompleted == null)) {
                this.FormatOperationCompleted = new System.Threading.SendOrPostCallback(this.OnFormatOperationCompleted);
            }
            this.InvokeAsync("Format", new object[] {
                        Document,
                        HighlighterName,
                        IncludeLineNumbers}, this.FormatOperationCompleted, userState);
        }
        
        private void OnFormatOperationCompleted(object arg) {
            if ((this.FormatCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.FormatCompleted(this, new FormatCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://codeconverter.sharpdevelop.net/RetrieveAvailableHighlighters", RequestNamespace="http://codeconverter.sharpdevelop.net/", ResponseNamespace="http://codeconverter.sharpdevelop.net/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string[] RetrieveAvailableHighlighters() {
            object[] results = this.Invoke("RetrieveAvailableHighlighters", new object[0]);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void RetrieveAvailableHighlightersAsync() {
            this.RetrieveAvailableHighlightersAsync(null);
        }
        
        /// <remarks/>
        public void RetrieveAvailableHighlightersAsync(object userState) {
            if ((this.RetrieveAvailableHighlightersOperationCompleted == null)) {
                this.RetrieveAvailableHighlightersOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRetrieveAvailableHighlightersOperationCompleted);
            }
            this.InvokeAsync("RetrieveAvailableHighlighters", new object[0], this.RetrieveAvailableHighlightersOperationCompleted, userState);
        }
        
        private void OnRetrieveAvailableHighlightersOperationCompleted(object arg) {
            if ((this.RetrieveAvailableHighlightersCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RetrieveAvailableHighlightersCompleted(this, new RetrieveAvailableHighlightersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void FormatCompletedEventHandler(object sender, FormatCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class FormatCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal FormatCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void RetrieveAvailableHighlightersCompletedEventHandler(object sender, RetrieveAvailableHighlightersCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RetrieveAvailableHighlightersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RetrieveAvailableHighlightersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591