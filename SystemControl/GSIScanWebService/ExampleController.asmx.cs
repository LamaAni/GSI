using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace GSIScanWebService
{
    /// <summary>
    /// Summary description for ExampleController
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ExampleController : System.Web.Services.WebService
    {
        [WebMethod]
        public bool Goto(double x, double y)
        {
            if (!GlobalInstance.Current.Status.IsStageLoaded)
                return false;
            GlobalInstance.Current.Stage.SetPosition(x, y, true);
            return true;
        }
    }
}
