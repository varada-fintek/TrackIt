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

using TrackIT.Common;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace TrackIT.WebApp.UserControls
{
    public partial class TopMenu : BasePage 
    {
        protected DataTable dtTopModules;
        protected DataTable dtLeftMenu;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LoadTopMenuItems();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        protected void LoadTopMenuItems()
        {
            try
            {
                BasePage objPage = Page as BasePage;

                objPage.GetTopandLeftMenuItems();

                if (dtTopModules != null)
                {
                    if (dtTopModules.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTopModules.Rows.Count; i++)
                        {
                            MenuItem objTopMenuItem = new MenuItem();
                            objTopMenuItem.Text = dtTopModules.Rows[i]["Module_Name"].ToString();
                            objTopMenuItem.Value = dtTopModules.Rows[i]["Module_ID"].ToString();
                            mnTopMenu.Items.Add(objTopMenuItem);

                            if (i < dtTopModules.Rows.Count - 1)
                            {
                                MenuItem objLine = new MenuItem();
                                objLine.ImageUrl = "~/images/vert_line_menu.jpg";
                                mnTopMenu.Items.Add(objLine);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

        protected void mnTopMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                if (!StringFunctions.IsNullOrEmpty(e.Item.Value))
                {
                    LoadLeftMenu(e.Item.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        protected void LoadLeftMenu(string strModuleID)
        {
            try
            {
                BasePage objPage = Page as BasePage;

                dtLeftMenu = objPage.GetLeftMenuItems(strModuleID);

                LeftMenu objLeftMenu = new LeftMenu();

                if (dtLeftMenu != null)
                {
                    objLeftMenu.DtLeftMenu = dtLeftMenu;
                    objLeftMenu.LoadLeftMenuItems();
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                throw;
            }
        }
    }
}