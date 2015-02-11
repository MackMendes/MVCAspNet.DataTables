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
            if (_listCliente == null)
                _listCliente = new Cliente().GetListFakeCliente(4);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CadastrarCliente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadastrarCliente(Cliente _cliente)
        {
            try
            {
                _cliente.IdCliente = (_listCliente.Count + 1);
                _listCliente.Add(_cliente);

                return RedirectToAction("DataTables");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DataTables()
        {
            return View(_listCliente);
        }

        [HttpGet]
        public ActionResult DataTablesAjax()
        {
            return View(_listCliente);
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