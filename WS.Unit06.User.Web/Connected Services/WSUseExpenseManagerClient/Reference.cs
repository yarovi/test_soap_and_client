﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WSUseExpenseManagerClient
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GroupDTO", Namespace="http://schemas.datacontract.org/2004/07/WS.Unit06.User.Application.Model")]
    public partial class GroupDTO : object
    {
        
        private int IdField;
        
        private string NameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name
        {
            get
            {
                return this.NameField;
            }
            set
            {
                this.NameField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserGroupDTO", Namespace="http://schemas.datacontract.org/2004/07/WS.Unit06.User.Application.Model")]
    public partial class UserGroupDTO : object
    {
        
        private int IdField;
        
        private string NameGroupField;
        
        private string fullNameUserField;
        
        private int userIdField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id
        {
            get
            {
                return this.IdField;
            }
            set
            {
                this.IdField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string NameGroup
        {
            get
            {
                return this.NameGroupField;
            }
            set
            {
                this.NameGroupField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string fullNameUser
        {
            get
            {
                return this.fullNameUserField;
            }
            set
            {
                this.fullNameUserField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int userId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="HistoryDTO", Namespace="http://schemas.datacontract.org/2004/07/WS.Unit06.User.Application.Model")]
    public partial class HistoryDTO : object
    {
        
        private string expenseField;
        
        private int idHistoryField;
        
        private string individualTotalField;
        
        private string nameGroupField;
        
        private string nameUserField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string expense
        {
            get
            {
                return this.expenseField;
            }
            set
            {
                this.expenseField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int idHistory
        {
            get
            {
                return this.idHistoryField;
            }
            set
            {
                this.idHistoryField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string individualTotal
        {
            get
            {
                return this.individualTotalField;
            }
            set
            {
                this.individualTotalField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nameGroup
        {
            get
            {
                return this.nameGroupField;
            }
            set
            {
                this.nameGroupField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string nameUser
        {
            get
            {
                return this.nameUserField;
            }
            set
            {
                this.nameUserField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://ws.unit06.user/auth/", ConfigurationName="WSUseExpenseManagerClient.IUserExpenseManagerServices")]
    public interface IUserExpenseManagerServices
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/createGroup", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/createGroupResponse")]
        System.Threading.Tasks.Task<int> createGroupAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/getAllCroup", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/getAllCroupResponse")]
        System.Threading.Tasks.Task<WSUseExpenseManagerClient.GroupDTO[]> getAllCroupAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/deleteGroup", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/deleteGroupResponse")]
        System.Threading.Tasks.Task<int> deleteGroupAsync(int id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/associateUserWithGroup", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/associateUserWithGroupResp" +
            "onse")]
        System.Threading.Tasks.Task<int[]> associateUserWithGroupAsync(int[] ids, int groupId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/getUserGroupsByUserId", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/getUserGroupsByUserIdRespo" +
            "nse")]
        System.Threading.Tasks.Task<WSUseExpenseManagerClient.UserGroupDTO[]> getUserGroupsByUserIdAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/getAllGroupByUser", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/getAllGroupByUserResponse")]
        System.Threading.Tasks.Task<WSUseExpenseManagerClient.GroupDTO[]> getAllGroupByUserAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/createTransaction", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/createTransactionResponse")]
        System.Threading.Tasks.Task<int> createTransactionAsync(int idGroup, string description, float expense);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/getHistoryTransaction", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/getHistoryTransactionRespo" +
            "nse")]
        System.Threading.Tasks.Task<WSUseExpenseManagerClient.HistoryDTO[]> getHistoryTransactionAsync(int idGroup);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://ws.unit06.user/auth/IUserExpenseManagerServices/isOwner", ReplyAction="http://ws.unit06.user/auth/IUserExpenseManagerServices/isOwnerResponse")]
        System.Threading.Tasks.Task<bool> isOwnerAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public interface IUserExpenseManagerServicesChannel : WSUseExpenseManagerClient.IUserExpenseManagerServices, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
    public partial class UserExpenseManagerServicesClient : System.ServiceModel.ClientBase<WSUseExpenseManagerClient.IUserExpenseManagerServices>, WSUseExpenseManagerClient.IUserExpenseManagerServices
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public UserExpenseManagerServicesClient() : 
                base(UserExpenseManagerServicesClient.GetDefaultBinding(), UserExpenseManagerServicesClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IUserExpenseManagerServices.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UserExpenseManagerServicesClient(EndpointConfiguration endpointConfiguration) : 
                base(UserExpenseManagerServicesClient.GetBindingForEndpoint(endpointConfiguration), UserExpenseManagerServicesClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UserExpenseManagerServicesClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(UserExpenseManagerServicesClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UserExpenseManagerServicesClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(UserExpenseManagerServicesClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public UserExpenseManagerServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<int> createGroupAsync(string name)
        {
            return base.Channel.createGroupAsync(name);
        }
        
        public System.Threading.Tasks.Task<WSUseExpenseManagerClient.GroupDTO[]> getAllCroupAsync()
        {
            return base.Channel.getAllCroupAsync();
        }
        
        public System.Threading.Tasks.Task<int> deleteGroupAsync(int id)
        {
            return base.Channel.deleteGroupAsync(id);
        }
        
        public System.Threading.Tasks.Task<int[]> associateUserWithGroupAsync(int[] ids, int groupId)
        {
            return base.Channel.associateUserWithGroupAsync(ids, groupId);
        }
        
        public System.Threading.Tasks.Task<WSUseExpenseManagerClient.UserGroupDTO[]> getUserGroupsByUserIdAsync()
        {
            return base.Channel.getUserGroupsByUserIdAsync();
        }
        
        public System.Threading.Tasks.Task<WSUseExpenseManagerClient.GroupDTO[]> getAllGroupByUserAsync()
        {
            return base.Channel.getAllGroupByUserAsync();
        }
        
        public System.Threading.Tasks.Task<int> createTransactionAsync(int idGroup, string description, float expense)
        {
            return base.Channel.createTransactionAsync(idGroup, description, expense);
        }
        
        public System.Threading.Tasks.Task<WSUseExpenseManagerClient.HistoryDTO[]> getHistoryTransactionAsync(int idGroup)
        {
            return base.Channel.getHistoryTransactionAsync(idGroup);
        }
        
        public System.Threading.Tasks.Task<bool> isOwnerAsync()
        {
            return base.Channel.isOwnerAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUserExpenseManagerServices))
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUserExpenseManagerServices))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:9091/UserExpenseManagerServices.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return UserExpenseManagerServicesClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IUserExpenseManagerServices);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return UserExpenseManagerServicesClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IUserExpenseManagerServices);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IUserExpenseManagerServices,
        }
    }
}
