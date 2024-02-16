using System.ComponentModel.DataAnnotations;

namespace WS.Unit06.User.Application.Model
{
	public class UserDTO
	{
		public int Id { get; set; }
		[Display(Name = "Usuario")]
		public string Name { get; set; }
		[Display(Name = "Correo")]
		public string Email { get;set; }
		[Display(Name = "Contraseña")]
		public string Password { get;set; }

	}
}
