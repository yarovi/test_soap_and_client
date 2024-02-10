using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoapCore;
using System.IdentityModel.Tokens.Jwt;
using System.ServiceModel;

namespace WS.Unit06.User.Application
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IApplicationServices, ApplicationServices>();
            services.TryAddSingleton<IAuthServices, AuthServices>();
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddSoapCore();



            // Agregar SOAP Core
            services.AddSoapCore();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }



            app.UseSoapEndpoint<IApplicationServices>(
            "/ApplicationServices.svc",
            new BasicHttpBinding(),
            SoapSerializer.DataContractSerializer,
            false, null, null, true, true);





            app.UseSoapEndpoint<IAuthServices>(
            "/AuthServices.svc",
            new BasicHttpBinding(),
            SoapSerializer.DataContractSerializer,
            false, null, null, true, true);

            app.UseMvc();
        }
    }
}
