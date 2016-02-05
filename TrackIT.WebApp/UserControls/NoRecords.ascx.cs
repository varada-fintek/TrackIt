using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace ProjMngTrack.WebApp.UserControls
{
    public partial class NoRecords : BasePageUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                BasePage objBasePage = (BasePage)this.Page;

                td.Text = objBasePage.RollupText("Common", "NoRecordMsg");          
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
    }
}