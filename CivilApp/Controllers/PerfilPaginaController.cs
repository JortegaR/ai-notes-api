using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CivilApp;

namespace CivilApp.Controllers
{
    public class PerfilPaginaController : Controller
    {
        private CivilAppEntities _entity = new CivilAppEntities();
        //
        // GET: /PerfilPagina/ tan sencillo como esto, soy op XD

        public void addPaginaPerfil(int pe, string lstPage)
        {
            var itm = lstPage.Split('|');
            foreach (var i in itm)
            {
                var item = int.Parse(i);
                var existe = _entity.PerfilPagina.FirstOrDefault(o => o.PerfilID == pe && o.PaginaID == item);
                if (existe == null)
                {
                    using (CivilAppEntities db = new CivilAppEntities())
                    {
                        PerfilPagina ie = new PerfilPagina();
                        ie.PerfilID = pe;
                        ie.PaginaID = item;
                       
                        db.PerfilPagina.Add(ie);
                        db.SaveChanges();
                    }
                }
            }
        }

        public void deletePaginaPerfil(int Id)
        {
            var registro = _entity.PerfilPagina.Find(Id);
            _entity.PerfilPagina.Remove(registro);
            _entity.SaveChanges();
        }

    }
}
