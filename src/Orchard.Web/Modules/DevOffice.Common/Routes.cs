using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace DevOffice.Common
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {

                  new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "ViewCount/Post",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "ViewCount"},
                            {"action", "Put"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },

                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "Feedback/PostFeedback",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "Feedback"},
                            {"action", "PostFeedback"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "code-samples-detail/{id}",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "CodeSamplesFilter"},
                            {"action", "GitHubCodeSamplesDetailPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "hands-on-labs/{id}",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "CodeSamplesFilter"},
                            {"action", "GitHubHandsOnLabsDetailPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },

                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "patterns-and-practices-detail/{id}",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "CodeSamplesFilter"},
                            {"action", "GitHubPatternsAndPracticesDetailPage"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },

                new RouteDescriptor {
                    Priority = 11,
                    Route = new Route(
                        "Search/listmore",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "search"},
                            {"action", "listmore"}
                        },
                        null,
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 10,
                    Route = new Route(
                        "Search",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "search"},
                            {"action", "index"}
                        },
                        null,
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "solutionsfilter/GetSolutionsForHorizontal",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "SolutionsFilter"},
                            {"action", "GetSolutionsForHorizontal"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "solutionsfilter/GetSolutionsForVertical",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "SolutionsFilter"},
                            {"action", "GetSolutionsForVertical"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "ShowMore/GetMoreArticles/{pageNumber}/{count}",
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"},
                            {"controller", "ShowMore"},
                            {"action", "GetArticles"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "DevOffice.Common"}
                        },
                        new MvcRouteHandler())
                }

            };
        }
    }
}