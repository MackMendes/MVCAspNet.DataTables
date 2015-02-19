using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Reflection;

namespace MVCAspNet.DataTables.HtmlHelpers.Grid
{
    public static class DataTablesHtmlHelper
    {
        // http://www.devmedia.com.br/html-helpers-criando-componentes-web-customizados-em-asp-net-mvc/27703
        // http://www.linhadecodigo.com.br/artigo/3010/aspnet-mvc-custom-helpers.aspx

        public static MvcHtmlString DataTablesJquery<T>(this HtmlHelper html, IEnumerable<T> listT, bool lengthchange, bool filter)
        {
            var stringTable = new StringBuilder();

            stringTable.Append("<table class=\"_tbDataTables\" lengthchange=\"");
            stringTable.Append(lengthchange);
            stringTable.Append("\" filter=\"");
            stringTable.Append(filter);
            stringTable.Append("\">");

            stringTable.Append(BuildeTHead<T>());

            stringTable.Append("<tbody>");

            foreach (var itemT in listT)
            {
                stringTable.Append("<tr>");
                


                stringTable.Append("</tr>");
            }

            stringTable.Append("</tbody>");



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
