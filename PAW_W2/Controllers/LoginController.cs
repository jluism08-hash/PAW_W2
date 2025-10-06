using PAW_W2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAW_W2.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(string user, string pass)
        {
            var userValidar = UsuarioDao.Login(user, pass);
            if (userValidar == null)
            {
                ViewBag.Msg = "Usuario o clave incorrecta";
                return View();
            }

            Session["UserId"] = userValidar.User;
            Session["UserName"] = String.IsNullOrWhiteSpace(userValidar.Nombre) ? userValidar.User : userValidar.Nombre;
            return RedirectToAction("Index", "Tarea");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}