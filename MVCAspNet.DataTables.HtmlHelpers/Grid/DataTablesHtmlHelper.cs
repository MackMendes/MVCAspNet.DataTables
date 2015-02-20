using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.Mvc;

namespace MVCAspNet.DataTables.HtmlHelpers.Grid
{
    public static class DataTablesHtmlHelper
    {
        /// <summary>
        /// HTML Helper do DataTables Jquery
        /// </summary>
        /// <typeparam name="T">Tipo da Entidade </typeparam>
        /// <param name="html"HTML Helper></param>
        /// <param name="listT">Lista com os dados</param>
        /// <param name="lengthchange">Se vai mostrar o "Alterar Tamanho de dados por página"</param>
        /// <param name="filter">Se vai mostrar o campo textbox Filtro</param>
        /// <returns></returns>
        public static MvcHtmlString DataTablesJquery<T>(this HtmlHelper html, IEnumerable<T> listT, bool lengthchange, bool filter)
        {
            var stringTable = new StringBuilder();

            stringTable.Append("<table class=\"_tbDataTables\" lengthchange=\"");
            stringTable.Append(lengthchange.ToString().ToLower());
            stringTable.Append("\" filter=\"");
            stringTable.Append(filter.ToString().ToLower());
            stringTable.Append("\">");

            stringTable.Append(BuildeTHead<T>());

            stringTable.Append("<tbody>");

            foreach (var itemT in listT)
            {
                stringTable.Append("<tr>");
                var typeT = typeof(T);
                PropertyInfo[] properties = typeT.GetProperties();

                foreach (var propertyItem in properties)
                {
                    stringTable.Append("<td>");
                    stringTable.Append(propertyItem.GetValue(itemT));
                    stringTable.Append("</td>");
                }

                stringTable.Append("</tr>");
            }

            stringTable.Append("</tbody></table>");

            return new MvcHtmlString(stringTable.ToString());
        }


        public static MvcHtmlString DataTablesJqueryAjax<T>(this HtmlHelper html, string ajaxSource, [Optional]bool serverSide, [Optional]bool destroy, bool lengthChange = true, bool filter = true, bool processing = true, string serverMethod = "GET")
        {
            var stringTable = new StringBuilder();

            stringTable.Append("<table class=\"_tbDataTables\" serverside=\"");
            stringTable.Append(serverSide.ToString().ToLower());
            stringTable.Append("\" lengthchange=\"");
            stringTable.Append(lengthChange.ToString().ToLower());
            stringTable.Append("\" filter=\"");
            stringTable.Append(filter.ToString().ToLower());
            stringTable.Append("\" ajaxsource=\"");
            stringTable.Append(ajaxSource);
            stringTable.Append("\" servermethod=\"");
            stringTable.Append(serverMethod);
            stringTable.Append("\" processing=\"");
            stringTable.Append(processing.ToString().ToLower());
            stringTable.Append("\" destroy=\"");
            stringTable.Append(destroy.ToString().ToLower());
            stringTable.Append("\">");

            stringTable.Append(BuildeTHead<T>());

            stringTable.Append("<tbody></tbody></table>");

            return new MvcHtmlString(stringTable.ToString());
        }


        /// <summary>
        /// Construir o thead da Table
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildeTHead<T>()
        {
            var stringThead = new StringBuilder();

            var typeT = typeof(T);
            PropertyInfo[] properties = typeT.GetProperties();

            stringThead.Append("<thead><tr>");

            foreach (var propertyInfo in properties)
            {
                stringThead.Append("<th column=\"");
                stringThead.Append(propertyInfo.Name);
                stringThead.Append("\">");
                stringThead.Append(propertyInfo.Name);
                stringThead.Append("</th>");
            }

            stringThead.Append("</tr></thead>");

            return stringThead.ToString();
        }
    }
}
