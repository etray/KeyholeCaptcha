using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class SampleController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Index.cshtml");
        }

        public ActionResult BusinessLogic(string id)
        {
            // If captcha was sucesssful, the client will pass-in the authenticated id.
            // Here we check to ensure that the guid is valid before allowing access to business logic.
            if (KeyholeCaptcha.Core.Validator.IsValidatedGuid(id))
            {
                // business logic
                ViewBag.welcomeMessage = "Welcome, human!";
                ViewBag.success = true;
            }
            else
            {
                ViewBag.welcomeMessage = "Begone, machine!";
                ViewBag.success = false;
            }
            return View("~/Views/BusinessLogic.cshtml");
        }
    }
}
