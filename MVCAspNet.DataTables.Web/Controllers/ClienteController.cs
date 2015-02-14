using MVCAspNet.DataTables.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var cliente = _listT.FirstOrDefault(x => x.IdCliente == id);
            return View(cliente);
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
            var cliente = _listT.First(x => x.IdCliente == id);
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Editar(Cliente cliente_)
        {
            try
            {
                var cliente = _listT.First(x => x.IdCliente == cliente_.IdCliente);
                _listT.Remove(cliente);
                _listT.Add(cliente_);

                return RedirectToAction("DataTablesAjax", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteCliente(int id)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
