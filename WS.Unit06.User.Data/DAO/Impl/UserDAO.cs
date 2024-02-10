using WS.Unit06.User.Data.Model;

namespace WS.Unit06.User.Data.DAO.Impl
{
	public class UserDAO : GenericDAO<Users>, IUserDAO
	{
		public UserDAO(DataContext context) : base(context)
		{
		}
	}
}
