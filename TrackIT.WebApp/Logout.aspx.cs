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
using TrackIT.WebApp.TrackITEnum;
using TrackIT.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace TrackIT.WebApp
{
    public partial class Logout : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
				if (GetSessionValue(SessionItems.User_ID) != null)
				{
					UserBO objResult = new UserBO(this.ConnectionString);
					objResult.LoginSessionID = Session.SessionID.ToString();
					objResult.UsersID = Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.User_ID).ToString());
					objResult.LoginType = "O";
					//UserBLL.InsertLoginDetails(objResult);
				}
				Session.Abandon();
                SetSessionValue(SessionItems.User_ID, null);
                Response.Redirect("~/Login.aspx", false);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Log_Only_Policy");
                Response.Redirect("~/Error.aspx", false);
            }
        }
    }
}
