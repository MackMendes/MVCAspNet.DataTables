using MVCAspNet.DataTables.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCAspNet.DataTables.Web.Controllers
{
    public class HomeController : Controller
    {
        private static IList<Cliente> _listCliente;
        private readonly Cliente _cliente = new Cliente();

        public HomeController()
        {
            _listCliente = new Cliente().GetListFakeCliente(20);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DataTables()
        {
            var cliente = _listCliente;
            return View(cliente);
        }

        public ActionResult ProcessarDataTables()
        {
            var resultado = _cliente.GetObjectClienteByRequest(_listCliente, Request);
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ProcessarAoIniciar()
        {
            var list = _cliente.GetObjectInicioCarregar(_listCliente);
            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}