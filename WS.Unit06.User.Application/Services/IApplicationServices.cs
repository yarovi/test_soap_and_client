using System.ServiceModel;
using WS.Unit06.User.Application.Model;


namespace WS.Unit06.User.Application
{
	[ServiceContract(Namespace = "http://ws.unit06.user/application/")]
	public interface IApplicationServices
	{
		[OperationContract]
		public UserDTO GetUser(string name, string email, string pw);

		[OperationContract]
		public UserDTO[] getUsers();

		[OperationContract]
		public void CreateUser(UserDTO userDTO);

		[OperationContract]
		public void UpdateUser(UserDTO userDTO);

		[OperationContract]
		public UserDTO GetUserById(int id);

		[OperationContract]
		public void DeleteUser(int id);
    }
}
