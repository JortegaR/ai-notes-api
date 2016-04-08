using CivilApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CivilApp.Controllers
{
    public class AuthorizationController : Controller
    {
        CivilAppEntities _entity = new CivilAppEntities();        //
        // GET: /Authorization/

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

          var userToValidate =  _entity.Usuarios.Where(user => user.Nombre == User.Identity.Name).FirstOrDefault();

          if (userToValidate.Perfil.PerfilPagina.Where(user => user.Paginas.Action == filterContext.RouteData.Values["Action"].ToString()
              && user.Paginas.Controller == filterContext.RouteData.Values["Controller"].ToString()).Count()  == 0)
              {
                  Response.Redirect("~/HOME/Index");
              }
            
        }
    }
}
