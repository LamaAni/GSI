using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSIGateway
{
    /// <summary>
    /// Summary description for HandleSimpleCommand
    /// </summary>
    public class HandleSimpleCommand : IHttpHandler
    {

        private ExampleRefrence.ExampleControllerSoapClient m_Client;

        public ExampleRefrence.ExampleControllerSoapClient Client
        {
            get
            {
                if (m_Client == null)
                {
                    m_Client = new ExampleRefrence.ExampleControllerSoapClient();
                    Client.Open();
                }
                return m_Client;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            double x = double.Parse(context.Request.Params["xv"]);
            double y = double.Parse(context.Request.Params["yv"]);
            Client.Goto(x, y);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}