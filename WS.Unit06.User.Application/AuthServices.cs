
using WSClient.Data.WS;
using WS.Unit06.User.Application.util;
using System.Net;

namespace WS.Unit06.User.Application
{
    public class AuthServices : IAuthServices
    {
        public HttpContext httpContext { get; set; }

        public string authenticate()
        {
            var username = httpContext.Request.Headers["username"];
            var password = httpContext.Request.Headers["password"];
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                IDataServices dataws = new DataServicesClient();
                Users user = dataws.GetUserByNameAndPassowrdAsync(username, password).Result;

                if (user != null)
                {
                    var tokenGenerator = new JwtTokenGenerator();
                    string jwtToken = tokenGenerator.GenerateJwtToken(username);
                    httpContext.Response.Headers.Add("token", jwtToken);
                    return "Usuario validado exitosamente";
                }
            }
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "No hay credenciales válidas";
        }

        public string validate()
        {
            var token = httpContext.Request.Headers["token"];
            if (!string.IsNullOrEmpty(token))
            {
                var tokenGenerator = new JwtTokenGenerator();
                var claimsPrincipal = tokenGenerator.ValidateJwtToken(token);
                if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
                    return "Token válido";
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return "Token inválido";
                }
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return "Token hay token";
            }
        }

    }
}
