
using WSClient.ApplicationWS;
using WSClient.Data.WS;

namespace WS.Unit06.User.Web.Models
{
	public class SelectedUsers
	{
		private List<UserDTO> _selectedUsers;

		public SelectedUsers()
		{
			_selectedUsers = new List<UserDTO>();
		}

		public void AddUser(int userId, string fullName,string fullNameGroup)
		{
			var user = new UserDTO { Id = userId, Name = fullName,fullNameGroup=fullNameGroup };
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

		public List<UserDTO> GetAllUsers()
		{
			return _selectedUsers;
		}
	}
}
