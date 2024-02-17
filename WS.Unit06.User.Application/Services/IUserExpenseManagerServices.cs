using System.ServiceModel;
using WS.Unit06.User.Application.Model;

namespace WS.Unit06.User.Application.Services
{
    [ServiceContract(Namespace = "http://ws.unit06.user/auth/")]
    public interface IUserExpenseManagerServices
    {
        [OperationContract]
        public void createGroup(string name);

        [OperationContract]
        public List<GroupDTO> getAllCroup();
    }
}
