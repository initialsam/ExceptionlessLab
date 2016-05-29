using Exceptionless;
using ExceptionlessLab.Infrastructure.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ExceptionlessLab
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ExceptionlessClient.Default.RegisterWebApi(config);
            config.MessageHandlers.Add(new LogMessageHandler());

            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
