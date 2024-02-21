using WS.Unit06.User.Application.Model;
using WSAuthClient;
using WSClient.Data.WS;

namespace WS.Unit06.User.Application.Services.impl
{
    public class ApplicationServices : IApplicationServices
    {
        public void CreateUser(UserDTO userDTO)
        {
            IDataServices dataws = new DataServicesClient();
            dataws.CreateUserAsync(MapDTOToUser(userDTO));
        }
        public UserDTO GetUser(string name, string email, string pw)
        {
            IDataServices user = new DataServicesClient();
            var userResult = user.GetOneUserAsync(name).Result;
            return MapUserToDTO(userResult);
        }
        public UserDTO[] getUsers()
        {
            IDataServices dataws = new DataServicesClient();
            var usersFromService = dataws.GetUsersAsync().Result;
            UserDTO[] userDTOs = new UserDTO[usersFromService.Length];

            for (int i = 0; i < usersFromService.Length; i++)
            {
                userDTOs[i] = MapUserToDTO(usersFromService[i]);
            }

            return userDTOs;
        }
        private UserDTO MapUserToDTO(Users user)
        {
            UserDTO userDTO = new UserDTO
            {
                Id = user.Id,
                Name = user.Username,
                Email = user.Email,
                Password = user.Password,
            };

            return userDTO;
        }
        private Users MapDTOToUser(UserDTO userDTO)
        {
            Users user = new Users
            {
                Id = userDTO.Id,
                Username = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password,
            };

            return user;
        }

        public UserDTO GetUserById(int id)
        {
            IDataServices dataws = new DataServicesClient();
            return MapUserToDTO(dataws.GetUserByIdAsync(id).Result);
        }

        public void UpdateUser(UserDTO userDTO)
        {
            IDataServices dataws = new DataServicesClient();
            dataws.UpdateUserAsync(MapDTOToUser(userDTO));
        }

        public void DeleteUser(int id)
        {
            IDataServices dataws = new DataServicesClient();
            dataws.DeleteUserAsync(id);
        }


        public ResponseCustom validate()
        {
            throw new NotImplementedException();
        }
    }
}
