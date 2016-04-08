using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CivilApp;

namespace CivilApp.Controllers
{
    public class PerfilesController : Controller
    {

        CivilAppEntities _entity = new CivilAppEntities();
        //
        // GET: /Perfiles/

        public ActionResult Index()
        {
            var model = _entity.Perfil.ToList();
            return View(model);
        }

        //
        // GET: /PerfilUsuario/Create

        public ActionResult Nuevo()
        {
            return View();
        }

        //
        // POST: /PerfilUsuario/Create

        [HttpPost]
        public ActionResult Nuevo(Perfil model)
        {
            try
            {
                // TODO: Add insert logic here
                _entity.Perfil.Add(model);
                _entity.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetItemSelected(int[] id)
        {
            CivilAppEntities db = new CivilAppEntities();
            var data = from r in db.Paginas
                       where id.Contains(r.PaginaID)
                       select new { r.PaginaID, Descripcion = r.Descripcion };
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        [Authorize]
        public ActionResult Borrar(int id = 0)
        {
            Perfil p = _entity.Perfil.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        //
        // POST: /Perfil/Delete/5

        [HttpPost, ActionName("Borrar")]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Perfil p = _entity.Perfil.Find(id);
            var comp = p.PerfilPagina.ToList();
            if (comp.Count > 0)
            {
                foreach (var co in comp)
                {
                    p.PerfilPagina.Remove(co);
                }
            }
            _entity.Perfil.Remove(p);
            _entity.SaveChanges();
            return RedirectToAction("Index", "Perfiles");
        }


    }
}
