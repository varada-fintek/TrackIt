using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI;
using Infragistics.Documents.Excel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using TrackIT.WebApp.Common;
using TrackIT.WebApp.TrackITEnum;
using TrackIT.Common;


namespace TrackIT.WebApp
{
    public partial class Home : BasePage
    {
 
        protected void Page_Load(object sender, EventArgs e)
        
        {
           try
            {
                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, BasePage.Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
       


    }
}
