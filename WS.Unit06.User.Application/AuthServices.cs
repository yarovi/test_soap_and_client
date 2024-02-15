
using WSClient.Data.WS;
using WS.Unit06.User.Application.util;
using System.Net;

namespace WS.Unit06.User.Application
{
    public class AuthServices : IAuthServices
    {
        public HttpContext httpContext { get; set; }

        public ResponseCustom authenticate()
        {
            var username = httpContext.Request.Headers["username"];
            var password = httpContext.Request.Headers["password"];
            var response = new ResponseCustom();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                IDataServices dataws = new DataServicesClient();
                Users user = dataws.GetUserByNameAndPassowrdAsync(username, password).Result;

                if (user != null)
                {
                    var tokenGenerator = new JwtTokenGenerator();
                    string jwtToken = tokenGenerator.GenerateJwtToken(username);
                    httpContext.Response.Headers.Add("token", jwtToken);
                   
                    response.code = 200;
                    response.messageCustom = "Usuario validado exitosamente";
                    return response;
                }
            }
           // httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.code = 401;
            response.messageCustom = "No hay credenciales válidas";
            return response;
        }

        public ResponseCustom validate()
        {
            var token = httpContext.Request.Headers["token"];
            var response = new ResponseCustom();
            if (!string.IsNullOrEmpty(token))
            {
                var tokenGenerator = new JwtTokenGenerator();
                var claimsPrincipal = tokenGenerator.ValidateJwtToken(token);
                if (claimsPrincipal != null && claimsPrincipal.Identity.IsAuthenticated)
                {
                    //response.code = 200;
                   // response.messageCustom = "Usuario validado exitosamente";
                   // return response;
                    response.code = 401;
                    response.messageCustom = "Usuario validado exitosamente";
                    return response;
                }
                else
                {
                    //httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    // return "Token inválido";
                    response.code = 401;
                    response.messageCustom = "Usuario validado exitosamente";
                    return response;
                }
            }
            else
            {
                //httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //return "Token hay token";
                response.code = 401;
                response.messageCustom = "No hay credenciales válidas";
                return response;
            }
        }

    }
}
