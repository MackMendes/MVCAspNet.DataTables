using System;
using System.Collections.Generic;
using System.Linq;
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
                string nomeColuna = request.Params["mDataProp_" + i.ToString()].ToString();
                columnNames.Add(nomeColuna);
            }

            for (int i = 0; i < iColumns; i++)
            {
                if (request.Params["iSortCol_" + i.ToString()] != null)
                {
                    int idxColuna = int.Parse(request.Params["iSortCol_" + i.ToString()]);
                    string nomeColuna = columnNames[idxColuna];
                    string orderDirection = request.Params["sSortDir_" + i.ToString()];
                    columnNamesOrder.Add(nomeColuna, orderDirection);
                }
            }

            if (iDisplayStart > listaDados.Count)
            {
                startExibir = 0;
            }

            if (iDisplayStart + iDisplayLength > listaDados.Count)
            {
                regExibir = listaDados.Count - startExibir;
            }

            IOrderedEnumerable<T> listaPartialSort = null;

            foreach (string columnName in columnNamesOrder.Keys)
            {
                PropertyInfo propert = typeof(T).GetProperty(columnName);

                if (columnNamesOrder[columnName] == "asc")
                {
                    if (listaPartialSort == null)
                    {
                        if (propert.PropertyType == typeof(String))
                        {
                            listaPartialSort = listaDados.OrderBy(GetFuncao<string>(columnName));
                        }
                        else if ((propert.PropertyType == typeof(Boolean)))
                        {
                            listaPartialSort = listaDados.OrderBy(GetFuncao<bool>(columnName));
                        }
                        else if ((propert.PropertyType == typeof(DateTime)))
                        {
                            listaPartialSort = listaDados.OrderBy(GetFuncao<DateTime>(columnName));
                        }
                        else
                        {
                            listaPartialSort = listaDados.OrderBy(GetFuncao<int>(columnName));
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
                            listaPartialSort = listaDados.OrderByDescending(GetFuncao<string>(columnName));
                        }
                        else if (propert.PropertyType == typeof(Boolean))
                        {
                            listaPartialSort = listaDados.OrderByDescending(GetFuncao<bool>(columnName));
                        }
                        else if (propert.PropertyType == typeof(DateTime))
                        {
                            listaPartialSort = listaDados.OrderByDescending(GetFuncao<DateTime>(columnName));
                        }
                        else
                        {
                            listaPartialSort = listaDados.OrderByDescending(GetFuncao<int>(columnName));
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

            IList<T> listaDadosSorted = listaPartialSort.ToList<T>().GetRange(startExibir, regExibir);
            var objeto = this.BuilderObjetoRetorno(listaDados.Count, this.echo, listaDadosSorted);
            return objeto;
        }

        /// <summary>
        /// Pegar a Função do tipo do retorna informado no TipoRetorno
        /// </summary>
        /// <typeparam name="TipoRetorno">Retorno do tipo informado</typeparam>
        /// <param name="propertyName">Nome da Propriedade</param>
        /// <returns>Retorna a função</returns>
        public Func<T, TRetorno> GetFuncao<TRetorno>(string propertyName)
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
