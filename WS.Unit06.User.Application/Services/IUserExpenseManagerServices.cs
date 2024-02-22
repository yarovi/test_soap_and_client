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
        public GroupDTO[] getAllCroup();

        [OperationContract]
        public int deleteGroup(int id);
        [OperationContract]
        public int[] associateUserWithGroup(int[] ids, int groupId);
        [OperationContract]
        public UserGroupDTO[] getUserGroups();

        [OperationContract]
        public GroupDTO[] getAllGroupByUser();

        [OperationContract]
        public int createTransaction(int idGroup, string description, float expense);

        [OperationContract]
        public TransactionDTO[] getAllTransaction();

        [OperationContract]
        public HistoryDTO[] getHistoryTransaction(int idGroup);
	}
}
