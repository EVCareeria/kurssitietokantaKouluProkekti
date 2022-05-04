using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using kurssitietokanta.Models;
using PagedList;

namespace kurssitietokanta.Controllers
{
    public class loginController : Controller
    {
        private tiimi4Entities1 db = new tiimi4Entities1();
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Authorize(Login LoginModel)
        {
            var LoggedUser = db.Login.SingleOrDefault(x => x.Sähköposti == LoginModel.Sähköposti && x.Salasana == LoginModel.Salasana);
            if (LoggedUser != null)
            {
                ViewBag.LoginMessage = "Kirjauduttu";
                TempData["LoggedStatus"] = "Kirjautunut tunnuksella" + LoginModel.Sähköposti;
                ViewBag.LoginError = 0;
                Session["Sähköposti"] = LoggedUser.Sähköposti;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.LoginMessage = "Kirjautuminen epäonnistui";
                TempData["LoggedStatus"] = "Out";
                ViewBag.LoginError = 1;
                LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana";
                return View("Index", LoginModel);
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            ViewBag.LoggedStatus = "Out";
            return RedirectToAction("Index", "Home");
        }
    }
}