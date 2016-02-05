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


namespace TrackIT.WebApp.UserControls
{
    public partial class LeftMenu : System.Web.UI.UserControl
    {
        private DataTable _dtLeftMenu ;

        public DataTable DtLeftMenu
        {
            get
            {
                return _dtLeftMenu;
            }
            set
            {
                _dtLeftMenu = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadLeftMenuItems();
        }

        public void LoadLeftMenuItems()
        {
            if (DtLeftMenu != null)
            {
                if (DtLeftMenu.Rows.Count > 0)
                {
                    //mnLeftMenu.Items.Clear();
                    for (int i = 0; i < DtLeftMenu.Rows.Count; i++)
                    {
                        MenuItem objLeftMenuItem = new MenuItem();
                        objLeftMenuItem.Text = DtLeftMenu.Rows[i]["Menu_Name"].ToString();
                        objLeftMenuItem.Value = DtLeftMenu.Rows[i]["Screen_ID"].ToString();
                        objLeftMenuItem.NavigateUrl = DtLeftMenu.Rows[i]["Url"].ToString();
                        mnLeftMenu.Items.Add(objLeftMenuItem);
                    }
                }
            }
        }


        protected void mnLeftMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                //if (!StringFunctions.IsNullOrEmpty(e.Item.Value))
                //{
                //    Response.Redirect(e.Item.NavigateUrl,false);
                //}                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, BasePage.Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
    }
}