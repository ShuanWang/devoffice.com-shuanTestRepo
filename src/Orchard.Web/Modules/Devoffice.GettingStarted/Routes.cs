﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace Dev.Office.Com.GettingStarted
{
    public class Routes : IRouteProvider
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
            return new[] {
               new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "GettingStarted/account/{action}", // name of the page url
                        new RouteValueDictionary {
                            {"area", "Devoffice.GettingStarted"}, // name of the module
                            {"controller", "account"},
                            {"action", "{action}"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area","Devoffice.GettingStarted"} // name of the module
                        },
                        new MvcRouteHandler()
                        ) // end new Route
               },
               new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "GettingStarted/proxy/{action}/{id}", // name of the page url
                        new RouteValueDictionary {
                            {"area", "Devoffice.GettingStarted"}, // name of the module
                            {"controller", "proxy"},
                            {"action", "{action}"},
                            {"id", UrlParameter.Optional}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area","Devoffice.GettingStarted"} // name of the module
                        },
                        new MvcRouteHandler()
                        ) // end new Route
               },
               new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "GettingStarted/{controller}/{action}/{param}", // name of the page url
                        new RouteValueDictionary {
                            {"area", "Devoffice.GettingStarted"}, // name of the module
                            {"controller", "{controller}"},
                            {"action", "{action}"},
                            {"param", UrlParameter.Optional}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area","Devoffice.GettingStarted"} // name of the module
                        },
                        new MvcRouteHandler()
                        ) // end new Route
               }            }; // end new[]
        } // end method
    }
}