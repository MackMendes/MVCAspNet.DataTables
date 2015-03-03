using MVCAspNet.DataTables.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCAspNet.DataTables.Web.Controllers
{
    public class HomeController : Core.BaseController<Cliente>
    {
        private readonly Cliente _cliente = new Cliente();

        public HomeController()
            : base(new Cliente().GetListFakeCliente(4))
        { }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DataTables()
        {
            return View(_listT);
        }

        [HttpGet]
        public ActionResult DataTablesAjax()
        {
            return View(new Cliente());
        }

        [HttpGet]
        public ActionResult DataTablesHTMLHelper()
        {
            return View(_listT);
        }

        [HttpGet]
        public ActionResult DataTablesAjaxHTMLHelper()
        {
            return View();
        }

        public ActionResult ProcessarDataTables()
        {
            var resultado = _cliente.GetObjectClienteByRequest(_listT, Request);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProcessarAoIniciar()
        {
            var list = _cliente.GetObjectInicioCarregar(_listT);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}
