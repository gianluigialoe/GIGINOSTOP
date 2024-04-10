using GIGINOSTOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace GIGINOSTOP.Controllers
{
    public class AuthController : Controller
    {

        DBContext db = new DBContext();

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Utenti user, bool? keepLogged)
        {
            bool keepUserLogged = keepLogged.HasValue && keepLogged.Value;
            var loggedUser = db.Utenti.FirstOrDefault(u => u.email == user.email);
            if (loggedUser != null)
            {
                bool validPassword = BCrypt.Net.BCrypt.Verify(user.password, loggedUser.password);
                if (validPassword)
                {
                    FormsAuthentication.SetAuthCookie(loggedUser.id.ToString(), keepUserLogged);
                    return RedirectToAction("Index", "Home");
                }
            }

            TempData["ErrorLogin"] = true;
            return RedirectToAction("Login");
        }

        public ActionResult Register()
        {

            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "nome,cognome,email,password,confermapassword")] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Utenti.FirstOrDefault(u => u.email == model.email);
                if (existingUser == null)
                {
                    
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.password);

                    var newUser = new Utenti
                    {
                        nome = model.nome,
                        cognome = model.cognome,
                        email = model.email,
                       password = hashedPassword,
                        


                     ruolo = "User",
                     
                    };
                    db.Utenti.Add(newUser);
                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(newUser.id.ToString(), false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "An account with this email already exists.");
                }
            }
            return View(model);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}