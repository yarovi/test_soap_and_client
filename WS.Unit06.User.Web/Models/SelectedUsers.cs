


using Newtonsoft.Json;
using WSClient.ApplicationWS;
using WSUseExpenseManagerClient;


namespace WS.Unit06.User.Web.Models
{
	public class SelectedUsers
	{
		[JsonProperty("userdtos")]
		private List<UserGroupDTO> _selectedUsers = new List<UserGroupDTO>();

		public SelectedUsers()
		{
		}

		public void AddUser(int userId, string fullName,string fullNameGroup)
		{
			var user = new UserGroupDTO { Id = userId, fullNameUser = fullName,NameGroup=fullNameGroup };
			if (!_selectedUsers.Any(u => u.Id == userId))
			{
				_selectedUsers.Add(user);
			}
		}

		public void RemoveUser(int userId)
		{
			var userToRemove = _selectedUsers.FirstOrDefault(u => u.Id == userId);
			if (userToRemove != null)
			{
				_selectedUsers.Remove(userToRemove);
			}
		}

		public bool ContainsUser(int userId)
		{
			return _selectedUsers.Any(u => u.Id == userId);
		}

		public List<UserGroupDTO> GetAllUsers()
		{
			return _selectedUsers;
		}
	}
}
