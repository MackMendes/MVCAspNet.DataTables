
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
    }
}