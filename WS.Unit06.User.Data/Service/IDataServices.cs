using System.ServiceModel;
using WS.Unit06.User.Data.Model;

namespace WS.Unit06.User.Data.Service
{
	[ServiceContract(Namespace = "http://ws.unit06.user/data/")]
	public interface IDataServices
	{
		[OperationContract]
		public Users[] GetUsers();
		[OperationContract]
		public Users GetOneUser(string name);
		[OperationContract]
		public void CreateUser(Users user);
		[OperationContract]
		public void UpdateUser(Users user);
		[OperationContract]
		public Users GetUserById(int id);
		[OperationContract]
		public Users GetUserByNameAndPassowrd(string name, string passowrd);
    }
}
