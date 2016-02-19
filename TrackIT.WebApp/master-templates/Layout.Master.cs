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

//using TrackIT.BusinessObjects;
using TrackIT.WebApp.TrackITEnum;
using TrackIT.Common;

using TrackIT.WebApp.Common;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Text;

namespace TrackIT.WebApp.master_templates
{
    public partial class Layout : System.Web.UI.MasterPage
    {

        #region "Declaration"

        protected DataTable dtTopModules;
        protected DataTable dtLeftModules;
        public string sTitleCaption = Utilities.GetKey("ApplicationTitle");
        public Int32 iShowingTopMenuCount = Convert.ToInt32(Utilities.GetKey("ShowingTopMenuCount"));
        public string GET_TREEVIEW_MENU_DETAILS = "Security_GetTreeViewMenuDetails";
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();

        //DashboardBO objDashBoardBO;
        //IDashboard objBase;
        //MyTaskBO objMyTaskBO;
        //IMyTask objTask;
        //CreateRequestBO objCreateRequestBO;
        //ICreateRequest objRequest;
        BasePage objBasePagePL;
        #endregion

        #region "Events"
        protected void Page_Load(object sender, EventArgs e)
        {
            objBasePagePL = Page as BasePage;

            try
            {
                //Checking If Session is Expired or Not
                if ((StringFunctions.IsNullOrEmpty(objBasePagePL.GetSessionValue(SessionItems.User_ID))) || (objBasePagePL.GetSessionValue(SessionItems.User_ID) == null))
                   Response.Redirect("~/Login.aspx", false);

                if (!IsPostBack)
                {
                    iTitle.Text = (objBasePagePL.GetSessionValue(SessionItems.Module_Name) != null) ? sTitleCaption + " :: " + objBasePagePL.GetSessionValue(SessionItems.Module_Name).ToString() : sTitleCaption;
                    //iTitle.Text = (objBasePagePL.GetSessionValue(SessionItems.Module_Name) != null) ? sTitleCaption + " :: " + objBasePagePL.GetSessionValue(SessionItems.Module_Name).ToString() : sTitleCaption;
                    objBasePagePL.SetSessionValue(SessionItems.Top_Menu, null);
                    objBasePagePL.SetSessionValue(SessionItems.Left_Menu, null);
                    objBasePagePL.GetTopandLeftMenuItems();
                    lbl_currentDate.Text = Convert.ToString(DateTime.Now);
                    ((HtmlImage)imgUserPhoto).Src = objBasePagePL.RollupText("Common", "NoimagePath");

                    lblLoginUserName.Text = objBasePagePL.LoggedInUserDisplayName;
                    lblLoginUserRoleName.Text = objBasePagePL.LoggedInUserRoleName;

                   
                }
                LoadMenuWithUL(objBasePagePL.LoggedInUserId, "0");

                if (int.Parse(objBasePagePL.GetSessionValue(SessionItems.Is_First_Login).ToString()) == 1)
                {                
                    HtmlAnchor MyLnk = (HtmlAnchor)this.FindControl("sidebarLeftToggle");
                    if (MyLnk != null)
                    { 
                        MyLnk.Visible = false;
                        divleftMenu.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, BasePage.Log_Only_Policy);
                //Response.Redirect("~/Error.aspx", false);
                Response.Write(ex.Message.ToString());

            }
            finally
            {
                if (objBasePagePL != null) objBasePagePL = null;
            }
        }
        #endregion


        protected void lnkLogout_Click(object sender, System.EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);    
        }
        
        protected void lbMenu_Click(object sender, EventArgs e)
        {
            BasePage objBasePageMTM = Page as BasePage;
            try
            {
                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, BasePage.Log_Only_Policy);
                //Response.Redirect("~/Error.aspx", false);
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                if (objBasePageMTM != null) objBasePageMTM = null;
            }
        }

        protected void LoadMenuWithUL(Int64? guidUserID, string sModuleID)
        {
            BasePage objBasePageLTV = Page as BasePage;
            DataSet dsTest = null;
            try
            {
                UserAccessBO objUserAccessBO = new UserAccessBO(objBasePageLTV.ConnectionString);
                objUserAccessBO.RoleID = objBasePageLTV.LoggedInUserRoleId;
                objUserAccessBO.ModuleID = Convert.ToInt32(sModuleID);

                SqlParameter[] objParams = { 
                        new SqlParameter ("@Role_ID",  objUserAccessBO.RoleID) 
                    };

                dsTest = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, GET_TREEVIEW_MENU_DETAILS, objParams);
                //dsTest = TrackIT.Security.UserAccessBLL.GetTreeViewMenuDetails(objUserAccessBO);



                if (dsTest != null)
                {
                    
                    HtmlGenericControl ul, ulchild, liSub, li, anchor, italic1, italic2, span;
                    Image img = null;
                    ul = new HtmlGenericControl("ul");
                    ul.Attributes.Add("class", "sidebar-nav");
                    //lbAnchor = new HtmlGenericControl("a");
                    LinkButton lbAnchor;


                    if (dsTest.Tables[0].Rows.Count > 0)
                    {
                       
                        for (int i = 0; i < dsTest.Tables[0].Rows.Count; i++)
                        {
                            liSub = new HtmlGenericControl("li");
                            ulchild = new HtmlGenericControl("ul");
                            if (dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() == "17")
                            {
                                ViewState["modules"] = (DataSet)dsTest;
                                //users
                                userdrp.Visible = true;
                                DataRow[] foundRoles;
                                DataRow[] foundUsers;
                                foundRoles = dsTest.Tables[1].Select("Screen_ID = '817'");
                                foundUsers = dsTest.Tables[1].Select("Screen_ID = '816'");
                                if (foundRoles.Length == 0)
                                {
                                    itemrole.Visible = false;
                                }
                                else
                                    itemrole.Visible = true;
                                if (foundUsers.Length ==0)
                                    itemuser.Visible = false;
                                else
                                    itemuser.Visible = true;
                                
                            }
                            else
                            {
                                //userdrp.Visible = false;
                            }

                            if (dsTest.Tables[0].Rows[i]["Is_Parent_Only"].ToString().Trim() == "True")
                            {

                               DataRow[] drRowArray = dsTest.Tables[1].Select(" Module_ID = '" + dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() + "' ");
                                foreach (DataRow drRow in drRowArray)
                                {
                                    if (dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() != "17")
                                    {
                                    anchor = new HtmlGenericControl("a");
                                   // anchor.InnerHtml = "<img src='../images/" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString().Replace(" ", "_") + ".png'>&nbsp;&nbsp;&nbsp;&nbsp;" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                                    anchor.Attributes.Add("href", "javascript:void(0)");
                                    anchor.Attributes.Add("onclick", "mainmenu();");
                                    anchor.Attributes.Add("data-toggle", "collapse");
                                    anchor.Attributes.Add("data-target", "#" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString());

                                   anchor.Attributes.Add("href", drRow["Url"].ToString() + "&Mode=N");
                                    if (drRow["Url"].ToString().Contains("?"))
                                        anchor.Attributes.Add("href", drRow["Url"].ToString() + "&Mode=N");
                                    else
                                       anchor.Attributes.Add("href", drRow["Url"].ToString() + "?Mode=N");

                                    italic1 = new HtmlGenericControl("i");
                                    italic1.Attributes.Add("class", dsTest.Tables[0].Rows[i]["Menu_Icon_CSS_Name"].ToString());
                                    span = new HtmlGenericControl("span");
                                    span.InnerHtml = dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                                    italic2 = new HtmlGenericControl("i");
                                    italic2.Attributes.Add("class", "fa ");
                                    anchor.Controls.Add(italic1);
                                    anchor.Controls.Add(span);
                                    anchor.Controls.Add(italic2);

                                    liSub.Controls.Add(anchor);
                                    liSub.Attributes.Add("class", dsTest.Tables[0].Rows[i]["Menu_CSS_Name"].ToString());
                                    ulchild.Attributes.Add("class", "collapse");
                                    //ulchild.Attributes.Add("height", "auto");
                                    ulchild.Attributes.Add("id", dsTest.Tables[0].Rows[i]["Menu_Name"].ToString());
                                }
                               }
                            }
                            else
                            {
                               
                                if (dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() != "17")
                                {
                                anchor = new HtmlGenericControl("a");
                                //anchor.InnerHtml = "<img src='../images/" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString().Replace(" ", "_") + ".png'>&nbsp;&nbsp;&nbsp;&nbsp;" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                                anchor.Attributes.Add("href", "javascript:void(0)");
                                anchor.Attributes.Add("onclick", "mainmenu();");
                                anchor.Attributes.Add("data-toggle", "collapse");
                                anchor.Attributes.Add("data-target", "#" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString());

                                italic1 = new HtmlGenericControl("i");
                                italic1.Attributes.Add("class", dsTest.Tables[0].Rows[i]["Menu_Icon_CSS_Name"].ToString());
                                span = new HtmlGenericControl("span");
                                span.InnerHtml = dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                                italic2 = new HtmlGenericControl("i");
                                italic2.Attributes.Add("class", "fa fa-caret-right submenu-indicator");
                                anchor.Controls.Add(italic1);
                                anchor.Controls.Add(span);
                                anchor.Controls.Add(italic2);

                                liSub.Controls.Add(anchor);
                                liSub.Attributes.Add("class", dsTest.Tables[0].Rows[i]["Menu_CSS_Name"].ToString());
                                ulchild.Attributes.Add("class", "collapse");
                                //ulchild.Attributes.Add("height", "auto");
                                ulchild.Attributes.Add("id", dsTest.Tables[0].Rows[i]["Menu_Name"].ToString());
                                //ulchild.InnerText = dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();

                                DataRow[] drRowArray = dsTest.Tables[1].Select(" Module_ID = '" + dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() + "' ");
                                foreach (DataRow drRow in drRowArray)
                                {
                                    li = new HtmlGenericControl("li");
                                   
                                    lbAnchor = new LinkButton();
                                    lbAnchor.Text = " " + drRow["Menu_Name"].ToString();
                                    lbAnchor.ID = "mnLink_" + drRow["Screen_ID"].ToString();
                                    lbAnchor.Attributes.Add("href", drRow["Url"].ToString() + "&Mode=N");
                                    if (drRow["Url"].ToString().Contains("?"))
                                        lbAnchor.Attributes.Add("href", drRow["Url"].ToString() + "&Mode=N");
                                    else
                                        lbAnchor.Attributes.Add("href", drRow["Url"].ToString() + "?Mode=N");
                                    lbAnchor.Visible = true;
                                    //lbAnchor.Attributes.Add("onclick", "submenu();");
                                    lbAnchor.Click += new EventHandler(lbMenu_Click);
                                    if (HttpContext.Current.Request.Path == Convert.ToString(drRow["Url"]))
                                    {
                                        ulchild.Style.Add("display", "block");
                                        lbAnchor.Style.Add("background-color", "#2f323a");
                                        lbAnchor.Style.Add("color", "white");
                                        lbAnchor.Style.Add("border-left", "4px solid #92cd18");
                                    }
                                    li.Controls.Add(lbAnchor);

                                    ulchild.Controls.Add(li);
                                }
                            }
                            }
                            liSub.Controls.Add(ulchild);
                            ul.Controls.Add(liSub);
                        }
                    }
                    pnlMenu.Controls.Add(ul);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objBasePageLTV != null) objBasePageLTV = null;
                if (dsTest != null) dsTest = null;
            }
        }

       
        protected void lnk_Users_Click(object sender, EventArgs e)
        {
            DataSet ldst = (DataSet)ViewState["modules"];
            Response.Redirect("~/Setup/UserMaster.aspx", false);
        }

        protected void lnk_roles_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Security/AccessRoles.aspx", false);
        }

        protected void lnkroles_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Security/AccessRoles.aspx", false);
        }

    }



}
