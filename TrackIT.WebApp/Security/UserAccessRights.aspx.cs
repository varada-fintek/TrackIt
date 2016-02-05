using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using ProjMngTrack.BusinessObjects;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace ProjMngTrack.WebApp.Security
{
    public partial class UserAccessRights : BasePage
    {

        #region "Declaration"

        string sSQL = string.Empty;

        #endregion

        #region "Events"

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                   
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion

    }
}