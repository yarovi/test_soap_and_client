using System.ServiceModel;
using WS.Unit06.User.Application.util;

namespace WS.Unit06.User.Application
{
    [ServiceContract(Namespace = "http://ws.unit06.user/auth/")]
    public interface IAuthServices
    {
        [OperationContract]
        public ResponseCustom authenticate();
        [OperationContract]
        public ResponseCustom validate();
    }
}
