using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace TrackIT.WebApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Error(object sender, EventArgs e)
        {
            BasePage objBasePage = new BasePage();
            HttpContext ctx = HttpContext.Current;
            Exception ex = ctx.Server.GetLastError();
            ExceptionPolicy.HandleException(ex, objBasePage.Log_Only_Policy_Global);
            ctx.Server.ClearError();
            HttpContext.Current.Response.Redirect("~/Error.aspx", false);            
        }

        //private void IsAuthenticated()
        //{
        //    string vFileName = Path.GetFileName(HttpContext.Current.Request.Path);
        //    string vExt = Path.GetExtension(vFileName).ToLower();
        //    if ((vFileName != "Login.aspx") && (vExt == ".aspx"))
        //    {
        //        if (HttpContext.Current.Session["USERID"] == null)
        //        {
        //            HttpContext.Current.Response.Redirect("~/Login.aspx");
        //        }
        //    }
        //}
    }
}