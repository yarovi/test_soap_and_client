﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSClientToken
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResponseCustom", Namespace="http://schemas.datacontract.org/2004/07/WS.Unit06.User.Application.util")]
    public partial class ResponseCustom : object
    {
        
        private int codeField;
        
        private string messageCustomField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string messageCustom
        {
            get
            {
                return this.messageCustomField;
            }
            set
            {
                this.messageCustomField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws.unit06.user/auth/", ConfigurationName="WSClientToken.IAuthServices")]
    public interface IAuthServices
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IAuthServices/authenticate", ReplyAction="http://ws.unit06.user/auth/IAuthServices/authenticateResponse")]
        System.Threading.Tasks.Task<WSClientToken.ResponseCustom> authenticateAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IAuthServices/validate", ReplyAction="http://ws.unit06.user/auth/IAuthServices/validateResponse")]
        System.Threading.Tasks.Task<WSClientToken.ResponseCustom> validateAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface IAuthServicesChannel : WSClientToken.IAuthServices, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class AuthServicesClient : System.ServiceModel.ClientBase<WSClientToken.IAuthServices>, WSClientToken.IAuthServices
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public AuthServicesClient() : 
                base(AuthServicesClient.GetDefaultBinding(), AuthServicesClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IAuthServices.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthServicesClient(EndpointConfiguration endpointConfiguration) : 
                base(AuthServicesClient.GetBindingForEndpoint(endpointConfiguration), AuthServicesClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthServicesClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(AuthServicesClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthServicesClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(AuthServicesClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public AuthServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<WSClientToken.ResponseCustom> authenticateAsync()
        {
            return base.Channel.authenticateAsync();
        }
        
        public System.Threading.Tasks.Task<WSClientToken.ResponseCustom> validateAsync()
        {
            return base.Channel.validateAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IAuthServices))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IAuthServices))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:9093/AuthServices.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return AuthServicesClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IAuthServices);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return AuthServicesClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IAuthServices);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IAuthServices,
        }
    }
}
