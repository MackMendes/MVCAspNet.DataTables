using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;

namespace MVCAspNet.DataTables.Framework
{
    public sealed class DataTableManager<T> where T : class
    {
        #region Varíaveis Globais

        private IList<T> listaDados;
        private string echo = "";
        private int iColumns = 0;
        private int iDisplayStart = 0;
        private int iDisplayLength = 0;
        private IList<string> columnNames = new List<string>();
        private Dictionary<string, string> columnNamesOrder = new Dictionary<string, string>();

        //Filter by Column
        private Dictionary<string, string> columnNamesFilter = new Dictionary<string, string>();

        private string iSortingCols = "";
        private int regExibir = 0;
        private int startExibir = 0;
        private int totalRegistros = 0;

        #endregion

        /// <summary>
        /// Método construtor
        /// </summary>
        /// <param name="data">Dados</param>
        /// <param name="totalRegistrosSemFiltro">Total de Registros sem Filtro</param>
        public DataTableManager(IList<T> data, int totalRegistrosSemFiltro)
        {
            this.listaDados = data;
            this.totalRegistros = totalRegistrosSemFiltro;
        }

        /// <summary>
        /// Pega o DataTables Objecto
        /// </summary>
        /// <param name="request">HttpRequest da página requisitada</param>
        /// <returns>Retorna o objeto</returns>
        public object GetDataTablesRequest(HttpRequestBase request)
        {
            this.echo = request.Params["sEcho"].ToString();
            this.iColumns = int.Parse(request.Params["iColumns"].ToString());
            this.iDisplayStart = int.Parse(request.Params["iDisplayStart"].ToString());
            this.iDisplayLength = int.Parse(request.Params["iDisplayLength"].ToString());
            this.iSortingCols = request.Params["iSortingCols"].ToString();
            this.regExibir = iDisplayLength;
            this.startExibir = iDisplayStart;

            for (int i = 0; i < iColumns; i++)
            {
                string nomeColuna = request.Params[string.Concat("mDataProp_", i)].ToString();
                columnNames.Add(nomeColuna);
            }

            for (int i = 0; i < iColumns; i++)
            {
                if (request.Params[string.Concat("iSortCol_", i)] != null)
                {
                    int idxColuna = int.Parse(request.Params[string.Concat("iSortCol_", i)]);
                    string nomeColuna = columnNames[idxColuna];
                    string orderDirection = request.Params[string.Concat("sSortDir_", i)];
                    this.columnNamesOrder.Add(nomeColuna, orderDirection);
                }

                if (request.Params[string.Concat("sSearch_", i)] != null)
                {
                    string nomeColuna = request.Params[string.Concat("mDataProp_", i)];
                    string valueFilter = request.Params[string.Concat("sSearch_", i)];
                    if (!string.IsNullOrEmpty(valueFilter) && !this.columnNamesFilter.ContainsKey(nomeColuna))
                        this.columnNamesFilter.Add(nomeColuna, valueFilter);
                }
            }

            if (iDisplayStart > listaDados.Count)
                startExibir = 0;

            if (iDisplayStart + iDisplayLength > listaDados.Count)
                regExibir = listaDados.Count - startExibir;

            //Order a lista
            List<T> listaDadosSorted = GetListPartialSort().ToList<T>();

            // Filtra
            listaDadosSorted = this.FilterByColumn(listaDadosSorted);
            var totalRegistroDisplay = listaDadosSorted.Count;

            if (totalRegistroDisplay > regExibir)
                listaDadosSorted = listaDadosSorted.GetRange(startExibir, regExibir); // Pega o range para ser visualizado no página

            var objeto = this.BuilderObjetoRetorno(totalRegistroDisplay, this.echo, listaDadosSorted);
            return objeto;
        }

        /// <summary>
        /// Método para filtrar por coluna, a lista informada no parâmetro de entrada, com base no tipo da coluna
        /// </summary>
        /// <param name="listaDadosFiltered">Lista informada para ser filtrada</param>
        /// <returns>Resultado da lista filtrada</returns>
        private List<T> FilterByColumn(IList<T> listaDadosFiltered)
        {
            IQueryable<T> listInQueryable = listaDadosFiltered.AsQueryable();

            string queryWhere = string.Empty;
            foreach (string columnName in this.columnNamesFilter.Keys)
            {
                PropertyInfo propert = typeof(T).GetProperty(columnName);

                switch (Type.GetTypeCode(propert.PropertyType))
                {
                    case TypeCode.Boolean:
                        queryWhere = string.Concat(columnName, "==Boolean?");
                        break;
                    case TypeCode.DBNull:
                        queryWhere = string.Concat(columnName, "==null");
                        break;
                    case TypeCode.DateTime:
                        queryWhere = string.Concat(columnName, "==DateTime?");
                        break;
                    case TypeCode.Decimal:
                        queryWhere = string.Concat(columnName, "==Decimal?");
                        break;
                    case TypeCode.Double:
                        queryWhere = string.Concat(columnName, "==Double?");
                        break;
                    case TypeCode.UInt16:
                    case TypeCode.Int16:
                        queryWhere = string.Concat(columnName, "==Int16?");
                        break;
                    case TypeCode.UInt32:
                    case TypeCode.Int32:
                        queryWhere = string.Concat(columnName, "==Int32?");
                        break;
                    case TypeCode.UInt64:
                    case TypeCode.Int64:
                        queryWhere = string.Concat(columnName, "==Int64?");
                        break;
                    case TypeCode.Char:
                    case TypeCode.String:
                        queryWhere = string.Concat(columnName, ".ToLower().Contains");
                        break;
                    default:
                        queryWhere = string.Empty;
                        break;
                }

                listInQueryable = listInQueryable.Where(string.Concat(queryWhere, "(@0)"), this.ParseValueInObject(this.columnNamesFilter[columnName], propert.PropertyType));
            }

            return listInQueryable.ToList<T>();
        }

        /// <summary>
        /// Converte o valor informando no parâmetro de entrada (string), para o tipo informando no parâmetro de entrada
        /// </summary>
        /// <param name="valor">Valor informando no parâmetro de entrada para ser convertido</param>
        /// <param name="propertyType">Tipo do valor a ser convertido</param>
        /// <returns>Objeto com o valor convertido</returns>
        private object ParseValueInObject(string valor, Type propertyType)
        {
            switch (Type.GetTypeCode(propertyType))
            {
                case TypeCode.Boolean:
                    Boolean bl;
                    if (Boolean.TryParse(valor, out bl)) return bl;
                    goto default;
                case TypeCode.DateTime:
                    DateTime dt;
                    if (DateTime.TryParse(valor, out dt)) return dt;
                    goto default;
                case TypeCode.Decimal:
                    Decimal dc;
                    if (Decimal.TryParse(valor, out dc)) return dc;
                    goto default;
                case TypeCode.Double:
                    Double db;
                    if (Double.TryParse(valor, out db)) return db;
                    goto default;
                case TypeCode.UInt16:
                case TypeCode.Int16:
                    Int16 it16;
                    if (Int16.TryParse(valor, out it16)) return it16;
                    goto default;
                case TypeCode.UInt32:
                case TypeCode.Int32:
                    Int32 it32;
                    if (Int32.TryParse(valor, out it32)) return it32;
                    goto default;
                case TypeCode.UInt64:
                case TypeCode.Int64:
                    Int64 it64;
                    if (Int64.TryParse(valor, out it64)) return it64;
                    goto default;
                case TypeCode.Char:
                case TypeCode.String:
                    return valor.ToLower().ToString();
                default:
                    return null;
            }
        }

        /// <summary>
        /// Método que retorna um objeto de Ordenação para ser aplicada
        /// </summary>
        /// <returns></returns>
        private IOrderedEnumerable<T> GetListPartialSort()
        {
            IOrderedEnumerable<T> listaPartialSort = null;

            foreach (string columnName in this.columnNamesOrder.Keys)
            {
                PropertyInfo propert = typeof(T).GetProperty(columnName);

                if (this.columnNamesOrder[columnName] == "asc")
                {
                    if (listaPartialSort == null)
                    {
                        if (propert.PropertyType == typeof(String))
                        {
                            listaPartialSort = this.listaDados.OrderBy(GetFuncao<string>(columnName));
                        }
                        else if ((propert.PropertyType == typeof(Boolean)))
                        {
                            listaPartialSort = this.listaDados.OrderBy(GetFuncao<bool>(columnName));
                        }
                        else if ((propert.PropertyType == typeof(DateTime)))
                        {
                            listaPartialSort = this.listaDados.OrderBy(GetFuncao<DateTime>(columnName));
                        }
                        else
                        {
                            listaPartialSort = this.listaDados.OrderBy(GetFuncao<int>(columnName));
                        }
                    }
                    else
                    {
                        if (propert.PropertyType == typeof(String))
                        {
                            listaPartialSort = listaPartialSort.ThenBy(GetFuncao<string>(columnName));
                        }
                        else if ((propert.PropertyType == typeof(Boolean)))
                        {
                            listaPartialSort = listaPartialSort.ThenBy(GetFuncao<bool>(columnName));
                        }
                        else if ((propert.PropertyType == typeof(DateTime)))
                        {
                            listaPartialSort = listaPartialSort.ThenBy(GetFuncao<DateTime>(columnName));
                        }
                        else
                        {
                            listaPartialSort = listaPartialSort.ThenBy(GetFuncao<int>(columnName));
                        }
                    }
                }
                else
                {
                    if (listaPartialSort == null)
                    {
                        if (propert.PropertyType == typeof(String))
                        {
                            listaPartialSort = this.listaDados.OrderByDescending(GetFuncao<string>(columnName));
                        }
                        else if (propert.PropertyType == typeof(Boolean))
                        {
                            listaPartialSort = this.listaDados.OrderByDescending(GetFuncao<bool>(columnName));
                        }
                        else if (propert.PropertyType == typeof(DateTime))
                        {
                            listaPartialSort = this.listaDados.OrderByDescending(GetFuncao<DateTime>(columnName));
                        }
                        else
                        {
                            listaPartialSort = this.listaDados.OrderByDescending(GetFuncao<int>(columnName));
                        }
                    }
                    else
                    {
                        if (propert.PropertyType == typeof(String))
                        {
                            listaPartialSort = listaPartialSort.ThenByDescending(GetFuncao<string>(columnName));
                        }
                        else if (propert.PropertyType == typeof(Boolean))
                        {
                            listaPartialSort = listaPartialSort.ThenByDescending(GetFuncao<bool>(columnName));
                        }
                        else if (propert.PropertyType == typeof(DateTime))
                        {
                            listaPartialSort = listaPartialSort.ThenByDescending(GetFuncao<DateTime>(columnName));
                        }
                        else
                        {
                            listaPartialSort = listaPartialSort.ThenByDescending(GetFuncao<int>(columnName));
                        }
                    }
                }
            }

            return listaPartialSort;
        }

        /// <summary>
        /// Pegar a Função do tipo do retorna informado no TipoRetorno
        /// </summary>
        /// <typeparam name="TipoRetorno">Retorno do tipo informado</typeparam>
        /// <param name="propertyName">Nome da Propriedade</param>
        /// <returns>Retorna a função</returns>
        private Func<T, TRetorno> GetFuncao<TRetorno>(string propertyName)
        {
            PropertyInfo propert = typeof(T).GetProperty(propertyName);
            Func<T, TRetorno> funcao = (Func<T, TRetorno>)Delegate.CreateDelegate(typeof(Func<T, TRetorno>), null, propert.GetGetMethod());

            return funcao;
        }

        /// <summary>
        /// Método que Construi o Objeto que o plugin DataTable espera quando o atributo "sAjaxSource" esta setado
        /// </summary>
        /// <param name="totalRegistroDisplay">Total de Registro que vai mostrar na página para o usuário</param>
        /// <param name="echo">Identificador da requisição realizada</param>
        /// <param name="listDadosDisplay">Lista de Dados que será mostrado a página</param>
        /// <returns>Objeto que o plugin espera</returns>
        public object BuilderObjetoRetorno(int totalRegistroDisplay, string echo, IList<T> listDadosDisplay)
        {
            return new
            {
                iTotalRecords = totalRegistros,
                iTotalDisplayRecords = totalRegistroDisplay,
                sEcho = echo,
                aaData = listDadosDisplay
            };
        }
    }
}
