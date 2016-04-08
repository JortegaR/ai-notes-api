using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CivilApp.Models;
using System.Security;
using System.Web.Security;
using System.Globalization;
using CivilApp;

namespace CivilApp.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        CivilAppMemberShipProvider MEMBER = new CivilAppMemberShipProvider();
        CivilAppEntities _entity = new CivilAppEntities();

        [AllowAnonymous]      
        public ActionResult LogIn()
        {
            return View();
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult LogIn(Account User, string ReturnUrl)
        {
            if (MEMBER.ValidateUser(User.UserName, User.password))
            {
                FormsAuthentication.SetAuthCookie(MEMBER.GetUserRealyName(User.UserName), User.IsPersisten);
                Session["IDUsuario"] = MEMBER.UserAuthenticateNow(User.UserName);

                var u = _entity.Usuarios.Where(x => x.UserLogIn == User.UserName).FirstOrDefault();
                return Redirect(ValidateUrl(ReturnUrl, u));
            }

            ModelState.AddModelError(string.Empty, "Usuario Invalido !");
            return View("LogIn");
        }

        [AllowAnonymous]
        public ActionResult LogOn() 
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn");
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SignIn(Usuarios u)
        {
            if (ModelState.IsValid)
            {
                var usuario = _entity.Usuarios.Where(o => o.UserLogIn == u.UserLogIn || o.Email == u.Email).FirstOrDefault();

                if (usuario == null)
                {
                    u.FechaCreacion = DateTime.Now;
                    u.EstadoUsuarioID = 1;
                    u.Password = MEMBER.EncryptText(u.Password);
                    _entity.Usuarios.Add(u);
                    _entity.SaveChanges();
                    return RedirectToAction("confirm", new { id = u.UsuarioID });
                }
                else
                {
                    ViewData["usuarioExiste"] = "El nombre de usuario o el correo no estan disponible. Intente con otro.";
                    return View();
                }
            }
            else
            {
                ViewData["usuarioExiste"] = "Datos invalidos. Vuelva a intentarlo otra vez.";
                return View();
            }
        }

        public ActionResult confirm(int id = 0)
        {
            if (id != 0)
            {
                ViewData["Confirm"] = "Ha sido registrado exitosamente";
                return View();
            }
            else
            { 
                ViewData["Confirm"] = "";
                return View();
            }            
        }
        public string ValidateUrl(string Url, Usuarios u) 
        {
            switch (u.PerfilID)
            {
                case null:
                    return "~/Home/Index";
                case 4:
                    return "~/DashboardAdmin/Index";
                case 2:
                    return "~/Home/Index";
                case 3:
                    return "~/Home/Index";
                default:
                    return Url;
                    
            }
        
        }

    }
}
