using RestSharp;
using System.ServiceModel;
using WS.Unit06.User.Application.Model;
using WSClient.Data.WS;
using Microsoft.Extensions.Configuration;

namespace WS.Unit06.User.Application.Services.impl
{
	public class ApplicationServices : IApplicationServices
    {
		public HttpContext httpContext { get; set; }
        private readonly DataServicesClient _dataClient;
        public ApplicationServices(IConfiguration configuration)
        {
            var configuration1 = configuration;
            var globalenv = configuration1["DATA_SERVICE_URL"] ?? configuration1.GetValue<string>("WebSettings:DataServiceURL");
            Console.WriteLine("Data URL constructor: " + globalenv);
            _dataClient =  new DataServicesClient(new BasicHttpBinding(), new EndpointAddress(globalenv));

        }
        public void CreateUser(UserDTO userDTO)
        {
            //IDataServices dataws = new DataServicesClient();
            _dataClient.CreateUserAsync(MapDTOToUser(userDTO));
        }
        public UserDTO GetUser(string name, string email, string pw)
        {
            //IDataServices user = new DataServicesClient();
            var userResult = _dataClient.GetOneUserAsync(name).Result;
            return MapUserToDTO(userResult);
        }
        public UserDTO[] getUsers()
        {
            //IDataServices dataws = new DataServicesClient();
            var usersFromService = _dataClient.GetUsersAsync().Result;
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
            //IDataServices dataws = new DataServicesClient();
            return MapUserToDTO(_dataClient.GetUserByIdAsync(id).Result);
        }

        public void UpdateUser(UserDTO userDTO)
        {
            //IDataServices dataws = new DataServicesClient();
            _dataClient.UpdateUserAsync(MapDTOToUser(userDTO));
        }

        public void DeleteUser(int id)
        {
            //IDataServices dataws = new DataServicesClient();
            _dataClient.DeleteUserAsync(id);
        }

    }
}
