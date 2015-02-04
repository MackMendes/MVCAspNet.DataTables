using FizzWare.NBuilder;
using MVCAspNet.DataTables.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCAspNet.DataTables.Web.Models
{
    public sealed class Cliente
    {
        public int IdCliente { get; set; }

        public string Nome { get; set; }

        public int Idade { get; set; }

        public EnumSexo Sexo { get; set; }

        public string Fone { get; set; }

        public string Email { get; set; }

        public object GetObjectClienteByRequest(IList<Cliente> clientes_, HttpRequestBase request_)
        {
            int countRegistros = clientes_.Count();
            string sSearch = request_.Params["sSearch"].ToString();

            if (sSearch.Equals("undefined"))
                sSearch = "";

            IList<Cliente> clientesFiltrados = clientes_.Where(x =>
                (x.Nome.ToString().Contains(sSearch.ToUpper())) ||
                (x.Idade.ToString().Contains(sSearch.ToUpper())) ||
                (x.Sexo.GetHashCode().ToString().Contains(sSearch.ToUpper())) ||
                ((string.IsNullOrEmpty(x.Fone) ? string.Empty : x.Fone.ToUpper()).Contains(sSearch.ToUpper())) ||
                (x.Email.ToUpper().Contains(sSearch.ToUpper()))).ToList<Cliente>();

            DataTableManager<Cliente> DataTable = new DataTableManager<Cliente>(clientesFiltrados, countRegistros);
            return DataTable.GetDataTablesRequest(request_);
        }


        public object GetObjectInicioCarregar(IList<Cliente> clientesCadastrados_)
        {
            int countRegistros = clientesCadastrados_.Count;

            DataTableManager<Cliente> DataTable = new DataTableManager<Cliente>(clientesCadastrados_, countRegistros);
            return DataTable.BuilderObjetoRetorno(countRegistros, "", clientesCadastrados_);
        }

        public IList<Cliente> GetListFakeCliente(int quantidadeList_)
        {
            // Através do NBuilder, criamos uma lista FAKE de cliente, para iniciar nossa aplicação com algum cliente para listar.
            return Builder<Cliente>.CreateListOfSize(quantidadeList_).Build();
        }
    }
}