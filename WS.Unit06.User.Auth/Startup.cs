using SoapCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WS.Unit06.User.Application.Services;
using WS.Unit06.User.Application.Services.impl;
using System.ServiceModel;
using WS.Unit06.User.Application.util;
namespace WS.Unit06.User.Auth
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IAuthServices, AuthServices>();
            services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddSoapCore();

            services.AddSoapServiceOperationTuner(new ServiceOperation());

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //TODO: obsoleto
            app.UseSoapEndpoint<IAuthServices>(
            "/AuthServices.svc",
            new BasicHttpBinding(),
            SoapSerializer.DataContractSerializer,
            false, null, null, true, true);

        }
    }
}
