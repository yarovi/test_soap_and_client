using System.ServiceModel;

namespace WS.Unit06.User.Application
{
    [ServiceContract(Namespace = "http://ws.unit06.user/auth/")]
    public interface IAuthServices
    {
        [OperationContract]
        public string authenticate();   
    }
}
