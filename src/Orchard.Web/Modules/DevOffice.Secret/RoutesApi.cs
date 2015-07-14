using System.Collections.Generic;
using System.Web.Http;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;
using System.Net.Http;

namespace DevOffice.Secret
{
    /// <summary>
    /// Custom routes for the Web API controllers.
    /// </summary>
    public class RoutesApi : IHttpRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
            {
                routes.Add(routeDescriptor);
            }
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {

            var apiRoute = new HttpRouteDescriptor
            {
                Priority = 5,
                RouteTemplate = "Api",
                Defaults = new
                {
                    area = "DevOffice.Secret",
                    controller = "SecretForm"
                }
            };

            var apiSubmissionRoute = new HttpRouteDescriptor
            {
                Priority = 10,
                RouteTemplate = "ApiRequest",
                Defaults = new
                {
                    area = "DevOffice.Secret",
                    controller = "ActivityFeedAPIForm"
                }
            };

            return new[] {
                apiRoute,
                apiSubmissionRoute
            };
        }
    }
}


