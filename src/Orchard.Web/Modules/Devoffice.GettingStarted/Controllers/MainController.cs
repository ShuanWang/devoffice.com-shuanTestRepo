<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Devoffice.GettingStarted.Models;
using Devoffice.GettingStarted.Utils;
namespace Devoffice.GettingStarted.Controllers
{
    [HandleError]
    public class MainController : Controller
    {
        public ActionResult Rest(string app)
        {
            if (string.IsNullOrEmpty(Request.Url.Query) == false)
            {
                // Parse the query string variables into a NameValueCollection.
                System.Collections.Specialized.NameValueCollection qscoll = HttpUtility.ParseQueryString(Request.Url.Query);
                if (qscoll.Count > 0)
                {
                    ViewBag.SigninError = qscoll["error_msg"];
                }
            }
            if (!string.IsNullOrEmpty(app))
            {
                Session[Constants.appNameTagStr] = app;
            }
            return View();
        }


        public ActionResult Addin(string app)
        {
            if (!string.IsNullOrEmpty(app))
            {
                Session[Constants.appNameTagStr] = app;
            }
            return View();
        }

        [HttpPost]
        /// <summary>
        /// Sets up the platform for the current session
        /// </summary>
        /// <param name="platformName">name of the platform</param>
        
        public void Platform(string param)
        {
            if(InputValidation.isValidPlatform(param))
            {
                Session[Constants.platformNameTagStr] = param;
            }
        }
    }
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Devoffice.GettingStarted.Models;
using Devoffice.GettingStarted.Utils;
namespace Devoffice.GettingStarted.Controllers
{
    [HandleError]
    public class MainController : Controller
    {
        public ActionResult Rest(string app)
        {
            if (string.IsNullOrEmpty(Request.Url.Query) == false)
            {
                // Parse the query string variables into a NameValueCollection.
                System.Collections.Specialized.NameValueCollection qscoll = HttpUtility.ParseQueryString(Request.Url.Query);
                if (qscoll.Count > 0)
                {
                    ViewBag.SigninError = qscoll["error_msg"];
                }
            }
            if (!string.IsNullOrEmpty(app))
            {
                Session[Constants.appNameTagStr] = app;
            }
            return View();
        }


        public ActionResult Addin(string app)
        {
            if (!string.IsNullOrEmpty(app))
            {
                Session[Constants.appNameTagStr] = app;
            }
            return View();
        }

        [HttpPost]
        /// <summary>
        /// Sets up the platform for the current session
        /// </summary>
        /// <param name="platformName">name of the platform</param>
        
        public void Platform(string param)
        {
            string platformName = param;
            if (!string.IsNullOrEmpty(platformName))
            {
                Session[Constants.platformNameTagStr] = platformName;
            }
        }
    }
>>>>>>> upstream/devx
}