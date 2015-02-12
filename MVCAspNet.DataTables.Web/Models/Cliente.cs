using FizzWare.NBuilder;
using MVCAspNet.DataTables.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCAspNet.DataTables.Web.Models
{
    public sealed class Cliente
    {
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "Por favor, informe o Nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe a idade.")]
        [Range(0, 110)]
        public int Idade { get; set; }

        [Required]
        [SexoEnumValidationAttribute(ErrorMessage = "Por favor, informe o sexo")]
        public EnumSexo? Sexo { get; set; }

        [Required]
        [RegularExpression(@"^\(\d{2}\) \d{4}-\d{4}$", ErrorMessage = "Por favor, informe um telefone válido.")]
        public string Fone { get; set; }

        [Required]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Por favor, informe um e-mail válido.")]
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

    [AttributeUsage(AttributeTargets.Property)]
    public class SexoEnumValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var sexo = (EnumSexo?)value;

            if (sexo.HasValue)
                return true;

            return false;
        }
    }


}