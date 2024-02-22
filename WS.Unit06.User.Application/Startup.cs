using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoapCore;
using System.ServiceModel;
using WS.Unit06.User.Application.Services;
using WS.Unit06.User.Application.Services.impl;
using WS.Unit06.User.Application.util;
namespace WS.Unit06.User.Application
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IApplicationServices, ApplicationServices>();
            services.TryAddSingleton<IUserExpenseManagerServices, UserExpenseManagerServices>();
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
            app.UseSoapEndpoint<IApplicationServices>(
            "/ApplicationServices.svc",
            new BasicHttpBinding(),
            SoapSerializer.DataContractSerializer,
            false, null, null, true, true);





            app.UseSoapEndpoint<IUserExpenseManagerServices>(
            "/UserExpenseManagerServices.svc",
            new BasicHttpBinding(),
            SoapSerializer.DataContractSerializer,
            false, null, null, true, true);


            app.UseMvc();
        }
    }
}
