using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CivilApp;
using CivilApp.Models;

namespace CivilApp.Controllers
{
    public class UsuariosController : Controller
    {
        CivilAppEntities _entity = new CivilAppEntities();
        CivilAppMemberShipProvider MEMBER = new CivilAppMemberShipProvider();
        //
        // GET: /Usuarios/
        [Authorize]
        public ActionResult Index()
        {
            var model = _entity.Usuarios.ToList();
            return View(model);
        }

        //
        // GET: /Usuario/Create
        [Authorize]
        public ActionResult Nuevo()
        {
            return View();
        }

        //
        // POST: /Usuario/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Nuevo(Usuarios model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _entity.Usuarios.Where(o => o.UserLogIn == model.UserLogIn || o.Email == model.Email).FirstOrDefault();

                if (usuario == null)
                {
                    try
                    {
                        // TODO: Add insert logic here
                        model.FechaCreacion = DateTime.Now;
                        model.EstadoUsuarioID = 1;
                        model.Password = MEMBER.EncryptText(model.Password);
                        _entity.Usuarios.Add(model);
                        _entity.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch
                    {
                        return View();
                    }
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

        [Authorize]
        public ActionResult Detalle(int id = 0)
        {
            var u = _entity.Usuarios.Find(id);

            if(u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }

        [Authorize]
        public ActionResult Editar(int id = 0)
        {
            var u = _entity.Usuarios.Find(id);

            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editar(Usuarios model)
        {
            var u = _entity.Usuarios.Find(model.UsuarioID);

            if (ModelState.IsValid)
            {
                u.Nombre = model.Nombre;
                u.Apellido = model.Apellido;
                u.UserLogIn = u.UserLogIn;
                u.Email = u.Email;
                u.Password = MEMBER.EncryptText(model.Password);
                u.EstadoUsuarioID = 1;
                u.PerfilID = model.PerfilID;
                u.FechaCreacion = u.FechaCreacion;
                u.FechaModificacion = DateTime.Now;
                _entity.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [Authorize]
        public ActionResult Borrar(int id = 0)
        {
            var u = _entity.Usuarios.Find(id);

            if (u == null)
            {
                return HttpNotFound();
            }
            return View(u);
        }
        [Authorize]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            var u = _entity.Usuarios.Find(id);

            if (u == null)
            {
                return HttpNotFound();
            }
            _entity.Usuarios.Remove(u);
            _entity.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
