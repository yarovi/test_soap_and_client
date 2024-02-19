using System.ServiceModel;
using WS.Unit06.User.Application.Model;

namespace WS.Unit06.User.Application.Services
{
    [ServiceContract(Namespace = "http://ws.unit06.user/auth/")]
    public interface IUserExpenseManagerServices
    {
        [OperationContract]
        public int createGroup(string name);

        [OperationContract]
        public List<GroupDTO> getAllCroup();

        [OperationContract]
        public int deleteGroup(int id);
        [OperationContract]
        public int[] associateUserWithGroup(List<int> ids, int groupId);
        [OperationContract]
        public List<UserGroupDTO> getUserGroups();
	}
}
