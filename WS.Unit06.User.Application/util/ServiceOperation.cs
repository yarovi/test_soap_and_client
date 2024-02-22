using Microsoft.Extensions.Primitives;
using SoapCore.Extensibility;
using SoapCore.ServiceModel;
using WS.Unit06.User.Application.Services.impl;

namespace WS.Unit06.User.Application.util
{
    public class ServiceOperation : IServiceOperationTuner
    {
        public void Tune(HttpContext httpContext, object serviceInstance, OperationDescription operation)
        {
			if (serviceInstance is UserExpenseManagerServices userExpenseService)
			{
				userExpenseService.httpContext = httpContext;
			}
			else if (serviceInstance is ApplicationServices applicationService)
			{
				applicationService.httpContext = httpContext;
			}
			/*if (operation.Name.Equals("authenticate") ||operation.Name.Equals("validate"))
            {
                UserExpenseManagerServices service = serviceInstance as UserExpenseManagerServices;
                if (service != null)
                {
                    ApplicationServices service = serviceInstance as ApplicationServices;
				}
                var request = httpContext;
                service.httpContext = request;
            }*/
        }
    }
}
