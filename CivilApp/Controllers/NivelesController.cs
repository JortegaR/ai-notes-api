using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CivilApp;

namespace CivilApp.Controllers
{
    public class NivelesController : Controller
    {
        CivilAppEntities _entity = new CivilAppEntities();
        //
        // GET: /Niveles/
        [Authorize]
        public ActionResult Index()
        {
            var model = _entity.Niveles.ToList();
            return View(model);
        }

        //
        // GET: /Niveles/Create
        [Authorize]
        public ActionResult Nuevo()
        {
            Niveles n = new Niveles();
            return View(n);
        }

        //
        // POST: /Niveles/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Nuevo(Niveles model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // TODO: Add insert logic here
                    _entity.Niveles.Add(model);
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
                return View();
            }
            
        }

        [Authorize]
        public ActionResult Detalle(int id = 0)
        {
            var n = _entity.Niveles.Find(id);

            if (n == null)
            {
                return HttpNotFound();
            }
            return View(n);
        }

        [Authorize]
        public ActionResult Editar(int id = 0)
        {
            var n = _entity.Niveles.Find(id);

            if (n == null)
            {
                return HttpNotFound();
            }
            return View(n);
        }

        [HttpPost]
        public ActionResult Editar(Niveles model)
        {
            var n = _entity.Niveles.Find(model.NivelID);
            n.Descripcion = model.Descripcion;
            _entity.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]       
        public ActionResult Borrar(int id = 0)
        {
            var n = _entity.Niveles.Find(id);

            if(n == null)
            {
                return HttpNotFound();
            }
            return View(n);
        }

        [Authorize]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            var n = _entity.Niveles.Find(id);
            _entity.Niveles.Remove(n);
            _entity.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
