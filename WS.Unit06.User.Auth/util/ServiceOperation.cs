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
            if (operation.Name.Equals("authenticate") ||operation.Name.Equals("validate"))
            {
                AuthServices service = serviceInstance as AuthServices;
                var request = httpContext;
                service.httpContext = request;
            }
        }
    }
}
