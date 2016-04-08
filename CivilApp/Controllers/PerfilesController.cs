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

    }
}
