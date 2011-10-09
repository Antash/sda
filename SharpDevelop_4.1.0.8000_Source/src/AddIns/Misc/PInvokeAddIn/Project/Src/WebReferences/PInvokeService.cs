﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by wsdl, Version=2.0.50727.42.
// 
namespace ICSharpCode.PInvokeAddIn.WebServices {
	using System;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="PInvokeServiceSoap", Namespace="http://www.pinvoke.net/webservices/")]
    public partial class PInvokeService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetResultsForFunctionOperationCompleted;
        
        private System.Threading.SendOrPostCallback ContributeSignaturesAndTypesOperationCompleted;
        
        /// <remarks/>
        public PInvokeService() {
            this.Url = "http://www.pinvoke.net/pinvokeservice.asmx";
        }
        
        /// <remarks/>
        public event GetResultsForFunctionCompletedEventHandler GetResultsForFunctionCompleted;
        
        /// <remarks/>
        public event ContributeSignaturesAndTypesCompletedEventHandler ContributeSignaturesAndTypesCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.pinvoke.net/webservices/GetResultsForFunction", RequestNamespace="http://www.pinvoke.net/webservices/", ResponseNamespace="http://www.pinvoke.net/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute(IsNullable=false)]
        public SignatureInfo[] GetResultsForFunction(string functionName, string moduleName) {
            object[] results = this.Invoke("GetResultsForFunction", new object[] {
                        functionName,
                        moduleName});
            return ((SignatureInfo[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetResultsForFunction(string functionName, string moduleName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetResultsForFunction", new object[] {
                        functionName,
                        moduleName}, callback, asyncState);
        }
        
        /// <remarks/>
        public SignatureInfo[] EndGetResultsForFunction(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((SignatureInfo[])(results[0]));
        }
        
        /// <remarks/>
        public void GetResultsForFunctionAsync(string functionName, string moduleName) {
            this.GetResultsForFunctionAsync(functionName, moduleName, null);
        }
        
        /// <remarks/>
        public void GetResultsForFunctionAsync(string functionName, string moduleName, object userState) {
            if ((this.GetResultsForFunctionOperationCompleted == null)) {
                this.GetResultsForFunctionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetResultsForFunctionOperationCompleted);
            }
            this.InvokeAsync("GetResultsForFunction", new object[] {
                        functionName,
                        moduleName}, this.GetResultsForFunctionOperationCompleted, userState);
        }
        
        private void OnGetResultsForFunctionOperationCompleted(object arg) {
            if ((this.GetResultsForFunctionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetResultsForFunctionCompleted(this, new GetResultsForFunctionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.pinvoke.net/webservices/ContributeSignaturesAndTypes", RequestNamespace="http://www.pinvoke.net/webservices/", ResponseNamespace="http://www.pinvoke.net/webservices/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string ContributeSignaturesAndTypes(string code, string language, string userName) {
            object[] results = this.Invoke("ContributeSignaturesAndTypes", new object[] {
                        code,
                        language,
                        userName});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginContributeSignaturesAndTypes(string code, string language, string userName, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("ContributeSignaturesAndTypes", new object[] {
                        code,
                        language,
                        userName}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndContributeSignaturesAndTypes(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void ContributeSignaturesAndTypesAsync(string code, string language, string userName) {
            this.ContributeSignaturesAndTypesAsync(code, language, userName, null);
        }
        
        /// <remarks/>
        public void ContributeSignaturesAndTypesAsync(string code, string language, string userName, object userState) {
            if ((this.ContributeSignaturesAndTypesOperationCompleted == null)) {
                this.ContributeSignaturesAndTypesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnContributeSignaturesAndTypesOperationCompleted);
            }
            this.InvokeAsync("ContributeSignaturesAndTypes", new object[] {
                        code,
                        language,
                        userName}, this.ContributeSignaturesAndTypesOperationCompleted, userState);
        }
        
        private void OnContributeSignaturesAndTypesOperationCompleted(object arg) {
            if ((this.ContributeSignaturesAndTypesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ContributeSignaturesAndTypesCompleted(this, new ContributeSignaturesAndTypesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.pinvoke.net/webservices/")]
    public partial class SignatureInfo {
        
        private string signatureField;
        
        private string languageField;
        
        private string lastAuthorField;
        
        private System.DateTime lastModifiedField;
        
        private string moduleField;
        
        private string urlField;
        
        private string alternativeManagedAPIField;
        
        private string summaryField;
        
        private string signatureCommentsField;
        
        /// <remarks/>
        public string Signature {
            get {
                return this.signatureField;
            }
            set {
                this.signatureField = value;
            }
        }
        
        /// <remarks/>
        public string Language {
            get {
                return this.languageField;
            }
            set {
                this.languageField = value;
            }
        }
        
        /// <remarks/>
        public string LastAuthor {
            get {
                return this.lastAuthorField;
            }
            set {
                this.lastAuthorField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime LastModified {
            get {
                return this.lastModifiedField;
            }
            set {
                this.lastModifiedField = value;
            }
        }
        
        /// <remarks/>
        public string Module {
            get {
                return this.moduleField;
            }
            set {
                this.moduleField = value;
            }
        }
        
        /// <remarks/>
        public string Url {
            get {
                return this.urlField;
            }
            set {
                this.urlField = value;
            }
        }
        
        /// <remarks/>
        public string AlternativeManagedAPI {
            get {
                return this.alternativeManagedAPIField;
            }
            set {
                this.alternativeManagedAPIField = value;
            }
        }
        
        /// <remarks/>
        public string Summary {
            get {
                return this.summaryField;
            }
            set {
                this.summaryField = value;
            }
        }
        
        /// <remarks/>
        public string SignatureComments {
            get {
                return this.signatureCommentsField;
            }
            set {
                this.signatureCommentsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void GetResultsForFunctionCompletedEventHandler(object sender, GetResultsForFunctionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetResultsForFunctionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetResultsForFunctionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public SignatureInfo[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((SignatureInfo[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void ContributeSignaturesAndTypesCompletedEventHandler(object sender, ContributeSignaturesAndTypesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ContributeSignaturesAndTypesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ContributeSignaturesAndTypesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
}
