
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace WS.Unit06.User.Application
{
    public class AuthServices : IAuthServices
    {
        public string authenticate()
        {
            throw new NotImplementedException();
        }
    }
}
