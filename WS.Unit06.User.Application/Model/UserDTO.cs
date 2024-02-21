using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WS.Unit06.User.Application.Model
{
	public class UserDTO
	{
		[JsonProperty("Id")]
		public int Id { get; set; }
		[Display(Name = "Usuario")]
		[JsonProperty("Name")]
		public string Name { get; set; }
		[Display(Name = "Correo")]
		[JsonProperty("Email")]
		public string Email { get;set; }
		[Display(Name = "Contraseña")]
		[JsonProperty("Password")]
		public string Password { get;set; }
		[JsonProperty("fullNameGroup")]
		public string fullNameGroup { get; set; }

	}
}
