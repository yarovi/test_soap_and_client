using System.ServiceModel;
using System.Xml.Linq;
using WS.Unit06.user.Data;
using WS.Unit06.User.Data.Model;

namespace WS.Unit06.User.Data.Service.impl
{
    public class DataServices : IDataServices
    {
        public void CreateUser(Users user)
        {
            using (DAOFactory factory = new DAOFactory())
            {
                var newUser = factory.UserDAO.All().
                    FirstOrDefault(p => p.Username == user.Username);

                if (newUser != null)
                    throw new FaultException(
                        new FaultReason("The user already exists!!!"),
                        new FaultCode("400"), "");
                factory.UserDAO.Add(user);
            }
        }

        public void DeleteUser(int id)
        {
            using (DAOFactory factory = new DAOFactory())
            {
                var currentUser = factory.UserDAO.All().
                    FirstOrDefault(p => p.Id == id);

                if (currentUser == null)
                    throw new FaultException(
                        new FaultReason("The user not exists!!!"),
                        new FaultCode("400"), "");
                factory.UserDAO.Remove(currentUser);
            }
        }

        public Users GetOneUser(string name)
        {
            using (DAOFactory factory = new DAOFactory())
            {
                var user = factory.UserDAO.All().
                    FirstOrDefault(p => p.Username == name);
                return user != null ? user : null!;
            }
        }

        public Users GetUserById(int id)
        {
            using (DAOFactory factory = new DAOFactory())
            {
                var user = factory.UserDAO.All().
                    FirstOrDefault(p => p.Id == id);
                return user != null ? user : null!;
            }
        }
        public Users GetUserByNameAndPassowrd(string name,string password)
        {
            using (DAOFactory factory = new DAOFactory())
            {
                var user = factory.UserDAO.All().
                    FirstOrDefault(p => p.Username==name && p.Password==password);
                return user != null ? user : null!;
            }
        }

        public Users[] GetUsers()
        {
            using (DAOFactory factory = new DAOFactory())
            {
                return factory.UserDAO.All().ToArray();
            }
        }

        public void UpdateUser(Users user)
        {
            using (DAOFactory factory = new DAOFactory())
            {
                factory.UserDAO.Update(user);
            }
        }
    }
}
