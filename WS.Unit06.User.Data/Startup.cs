using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SoapCore;
using System.ServiceModel;
using WS.Unit06.User.Data.Service;

namespace WS.Unit06.User.Data
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.TryAddSingleton<IDataServices, DataServices>();
			services.AddMvc(x => x.EnableEndpointRouting = false);
			services.AddSoapCore();
			services.AddDbContext<DataContext>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			using (var serviceScope = app.
				ApplicationServices.
				GetService<IServiceScopeFactory>()!.CreateScope())
			{
				var context = new DataContext();
				context.Database.EnsureCreated();

			}
			app.UseSoapEndpoint<IDataServices>(
				"/DataServices.svc",
				new BasicHttpBinding(),
				SoapSerializer.DataContractSerializer,
				false, null, null, true, true);
			app.UseMvc();
		}
	}
}
