using MVCAspNet.DataTables.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MVCAspNet.DataTables.Web.Controllers
{
    public class ClienteController : Core.BaseController<Cliente>
    {
        public ClienteController()
            : base(new List<Cliente>())
        { }

        // GET: Cliente
        public ActionResult Index()
        {
            return View();
        }

        // GET: Cliente/Detalhe/5
        public ActionResult Detalhe(int id)
        {
            return GetClienteById(id);
        }

        [HttpGet]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(Cliente cliente_)
        {
            if (!ModelState.IsValid)
                return View("Cadastrar", cliente_);

            try
            {
                cliente_.IdCliente = (_listT.Count + 1);
                _listT.Add(cliente_);

                return RedirectToAction("DataTables", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Editar(int id)
        {
            return GetClienteById(id);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Editar(Cliente cliente_)
        {
            try
            {
                var cliente = _listT.FirstOrDefault(x => x.IdCliente == cliente_.IdCliente);
                _listT.Remove(cliente);
                _listT.Add(cliente_);

                return RedirectToAction("DataTables", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return GetClienteById(id);
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteCliente(int id)
        {
            try
            {
                var cliente = _listT.First(x => x.IdCliente == id);
                _listT.Remove(cliente);
                return RedirectToAction("DataTables", "Home");
            }
            catch
            {
                return View();
            }
        }

        private ViewResult GetClienteById(int id)
        {
            var cliente = _listT.First(x => x.IdCliente == id);
            return View(cliente);
        }
    }
}
