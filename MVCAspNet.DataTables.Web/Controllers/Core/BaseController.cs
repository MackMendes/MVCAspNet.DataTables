using System.Collections.Generic;
using System.Web.Mvc;

namespace MVCAspNet.DataTables.Web.Controllers.Core
{
    public class BaseController<T> : Controller where T : class
    {
        protected static IList<T> _listT;

        public BaseController(IList<T> listT_)
        {
            if (_listT == null)
                _listT = listT_;

        }
    }
}