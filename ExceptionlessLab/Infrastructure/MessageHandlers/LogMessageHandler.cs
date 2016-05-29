using Exceptionless;
using Exceptionless.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;

namespace ExceptionlessLab.Infrastructure.MessageHandlers
{
public class LogMessageHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var requestContent = await request.Content.ReadAsStringAsync();

        var response = await base.SendAsync(request, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync();

        Dictionary<string, string> logData = new Dictionary<string, string>();
        logData.Add("iRequest", request.ToString());
        logData.Add("iRequestContent", requestContent.ToString());
        logData.Add("iResponse", response.ToString());
        logData.Add("iResponseContent", responseContent.ToString());

        ExceptionlessClient.Default.CreateLog(
                                source: request.RequestUri.AbsolutePath,
                                message: GetControllerNameAndActionName(request),
                                level: LogLevel.Info)
                            .AddObject(logData)
                            .Submit();
        return response;
    }

    private static string GetControllerNameAndActionName(HttpRequestMessage request)
    {
        var config = request.GetConfiguration();
        var routeData = config.Routes.GetRouteData(request);
        var controllerContext = new HttpControllerContext(config, routeData, request);

        request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
        controllerContext.RouteData = routeData;

        // get controller type
        var controllerDescriptor = new DefaultHttpControllerSelector(config).SelectController(request);
        controllerContext.ControllerDescriptor = controllerDescriptor;

        // get controller name
        var controllerName = controllerDescriptor.ControllerName;

        // get action name
        var actionName = request.GetActionDescriptor().ActionName;

        return string.Format("controller name:{0}，action name:{1}", controllerName, actionName);
    }
}
}