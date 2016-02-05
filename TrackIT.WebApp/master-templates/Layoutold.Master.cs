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

using ProjMngTrack.BusinessLogic;
using ProjMngTrack.WebApp.CtrlAEnum;

using ProjMngTrack.Common;
using ProjMngTrack.Security;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ProjMngTrack.WebApp.master_templates
{
    public partial class Layoutold : System.Web.UI.MasterPage
    {
        
        #region "Declaration"

        protected DataTable dtTopModules;
        protected DataTable dtLeftModules;
        public string sTitleCaption = Utilities.GetKey("ApplicationTitle");
        public Int32 iShowingTopMenuCount = Convert.ToInt32(Utilities.GetKey("ShowingTopMenuCount"));

        #endregion

        #region "Events"

        protected void Page_Load(object sender, EventArgs e)
        {
            BasePage objBasePagePL = Page as BasePage;
            try
            {
                //Checking If Session is Expired or Not
                //if ((StringFunctions.IsNullOrEmpty(objBasePagePL.GetSessionValue(SessionItems.User_ID))) || (objBasePagePL.GetSessionValue(SessionItems.User_ID) == null))
                //    Response.Redirect("~/Login.aspx", false);

                if (!IsPostBack)
                {
                    
                    //LoadTopMenuItems();
                   

                    SelectTopMenu();

                    iTitle.Text = (objBasePagePL.GetSessionValue(SessionItems.Module_Name) != null) ? sTitleCaption + " :: " + objBasePagePL.GetSessionValue(SessionItems.Module_Name).ToString() : sTitleCaption;
                    
                    if (!StringFunctions.IsNullOrEmpty(objBasePagePL.ModuleID))
                    {
                        if (objBasePagePL.ModuleID == CtrlAModules.CtrlA_MODULES.SETUP || objBasePagePL.ModuleID == CtrlAModules.CtrlA_MODULES.REPORTS || objBasePagePL.ModuleID == CtrlAModules.CtrlA_MODULES.SETUP || objBasePagePL.ModuleID == CtrlAModules.CtrlA_MODULES.SECURITY)
                        {

                            LoadTreeViewMenuItems(objBasePagePL.LoggedInUserId, objBasePagePL.ModuleID.ToString());
                            SelectTreeViewMenu();
                            tvLeftMenu.Visible = true;
                            mnLeftMenu.Visible = false;
                        }
                        else
                        {
                            LoadLeftMenuItems(objBasePagePL.ModuleID.ToString());                        
                            SelectLeftMenu();
                            tvLeftMenu.Visible = false;
                            mnLeftMenu.Visible = true;
                        }
                    }
                    objBasePagePL.SetSessionValue(SessionItems.Top_Menu, null);
                    objBasePagePL.SetSessionValue(SessionItems.Left_Menu, null);
                    objBasePagePL.GetTopandLeftMenuItems();
                     LoadTreeViewMenuItems(objBasePagePL.LoggedInUserId, "0");
                    LoadMenuItems(objBasePagePL.LoggedInUserId, "0");
                    LoadMenuWithUL(objBasePagePL.LoggedInUserId, "0");
                    //SelectTreeViewMenu();
                    tvLeftMenu.Visible = false;
                    mnLeftMenu.Visible = false;
                    mnNewLeftMenu.Visible = false;

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


        protected void tvLeftMenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            BasePage objBasePageTSN = Page as BasePage;
            try
            {
                tvLeftMenu.CollapseAll();
                tvLeftMenu.SelectedNode.Expand();
                objBasePageTSN.SetSessionValue(SessionItems.Left_Node, tvLeftMenu.SelectedValue.Trim());                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Log_Only_Policy");
                //Response.Redirect("~/Error.aspx", false);
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                if (objBasePageTSN != null) objBasePageTSN = null;
            }
        }

        protected void tvLeftMenu_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            BasePage objBasePageTSN = Page as BasePage;
            try
            {
                //string value = e.Node.Value;
                //if (e.Node.Expanded == true)
                //{
                //    for (int i = 0; i < tvLeftMenu.Nodes.Count; i++)
                //    {
                //        if (tvLeftMenu.Nodes[i].Value.Trim() == e.Node.Value.Trim())
                //        {
                //            tvLeftMenu.Nodes[i].Expand();
                //            objBasePageTSN.SetSessionValue(SessionItems.Left_Node, tvLeftMenu.Nodes[i].Value.Trim());
                //        }
                //        else
                //            tvLeftMenu.Nodes[i].Collapse();
                //    }
                //}
                //else
                //{
                //    if (tvLeftMenu.Nodes.Count >= 0)
                //        tvLeftMenu.Nodes[0].Expand();
                //    objBasePageTSN.SetSessionValue(SessionItems.Left_Node, tvLeftMenu.Nodes[0].Value.Trim());
                //}
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Log_Only_Policy");
                //Response.Redirect("~/Error.aspx", false);
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                if (objBasePageTSN != null) objBasePageTSN = null;
            }
        }

        protected void mnTopMenu_MenuItemClick(object sender, MenuEventArgs e)
        {
            BasePage objBasePageMTM = Page as BasePage;
            try
            {
                if (!StringFunctions.IsNullOrEmpty(e.Item.Value))
                {                    
                    if(!StringFunctions.IsNullOrEmpty(objBasePageMTM.GetSessionValue(SessionItems.Previous_Next)))
                    {
                        ReLoadTopMenuList(objBasePageMTM.GetSessionValue(SessionItems.Previous_Next).ToString().Trim().ToLower());
                        if (!StringFunctions.IsNullOrEmpty(e.Item.Value.ToString().Trim()))
                            SelectTopMenu();
                    }
                    objBasePageMTM.SetSessionValue(SessionItems.Module_ID, e.Item.Value.ToString().Trim());
                    objBasePageMTM.SetSessionValue(SessionItems.Module_Name, e.Item.Text.ToString().Trim());

                    if (e.Item.Value.ToString() == CtrlAModules.CtrlA_MODULES.SETUP || e.Item.Value.ToString() == CtrlAModules.CtrlA_MODULES.REPORTS)
                    {
                        tvLeftMenu.Visible = true;
                        mnLeftMenu.Visible = false;

                        LoadTreeViewMenuItems(objBasePageMTM.LoggedInUserId, e.Item.Value.ToString());
                        Response.Redirect("~/Home.aspx", false);
                    }
                    else
                    {
                        tvLeftMenu.Visible = false;
                        mnLeftMenu.Visible = true;

                        LoadLeftMenuItems(e.Item.Value.ToString());
                        e.Item.Selected = true;
                        Response.Redirect("~/Home.aspx", false);
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
                if (objBasePageMTM != null) objBasePageMTM = null;
            }
        }

        protected void ibPrevious_Click(object sender, EventArgs e)
        {
            BasePage objBasePageIBP = Page as BasePage;
            try
            {
                ReLoadTopMenuList(BasePage.PREVIOUS);
                objBasePageIBP.SetSessionValue(SessionItems.Previous_Next, BasePage.PREVIOUS);                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Log_Only_Policy");
                //Response.Redirect("~/Error.aspx", false);
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                if (objBasePageIBP != null) objBasePageIBP = null;
            }
        }

        protected void ibNext_Click(object sender, EventArgs e)
        {
            BasePage objBasePageIBN = Page as BasePage;
            try
            {
                ReLoadTopMenuList(BasePage.NEXT);
                objBasePageIBN.SetSessionValue(SessionItems.Previous_Next, BasePage.NEXT);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "Log_Only_Policy");
                //Response.Redirect("~/Error.aspx", false);
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                if (objBasePageIBN != null) objBasePageIBN = null;
            }
        }

        #endregion

        #region "User Defined Methods"

        protected void LoadTopMenuItems()
        {
            BasePage objBasePageLTM = Page as BasePage;
            int iTotalRecordsCount = 0;
            int iTotalLoopCount = 0;
            
            try
            {
                if (objBasePageLTM.GetSessionValue(SessionItems.Top_Menu) == null)
                {
                    objBasePageLTM.GetTopandLeftMenuItems();
                    dtTopModules = (DataTable)objBasePageLTM.GetSessionValue(SessionItems.Top_Menu);
                    iTotalRecordsCount = dtTopModules.Rows.Count;
                    if (iShowingTopMenuCount < iTotalRecordsCount)
                    {
                        //tdLeftArrow.Visible = true;
                        //tdRightArrow.Visible = true;
                    }
                    else
                    {
                        //tdLeftArrow.Visible = false;
                        //tdRightArrow.Visible = false;
                    }
                    if (iShowingTopMenuCount < iTotalRecordsCount)
                        iTotalLoopCount = iShowingTopMenuCount;
                    else
                        iTotalLoopCount = iTotalRecordsCount;

                    if (dtTopModules.Rows.Count > 0)
                    {
                        objBasePageLTM.SetSessionValue(SessionItems.Top_Menu, dtTopModules);
                        for (int i = 0; i < iTotalLoopCount; i++)
                        {
                            if (i < iTotalLoopCount)
                            {
                                MenuItem objTopMenuItem1 = new MenuItem();
                                objTopMenuItem1.Text = dtTopModules.Rows[i]["Module_Name"].ToString();
                                objTopMenuItem1.Value = dtTopModules.Rows[i]["Module_ID"].ToString();
                                //mnTopMenu.Items.Add(objTopMenuItem1);
                                // TO LOADING THE SUBMENU FOR FIRST MODULES
                                if (i == 0)
                                {
                                    objTopMenuItem1.Selected = true;
                                    LoadLeftMenuItems(dtTopModules.Rows[i]["Module_ID"].ToString());
                                    objBasePageLTM.SetSessionValue(SessionItems.Module_ID, dtTopModules.Rows[i]["Module_ID"].ToString());
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (objBasePageLTM.GetSessionValue(SessionItems.Previous_Next) != null)
                        ReLoadTopMenuList(objBasePageLTM.GetSessionValue(SessionItems.Previous_Next).ToString());
                    else
                        ReLoadTopMenuList(string.Empty);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
            finally
            {
               
            }
        }

        protected void LoadLeftMenuItems(string sModuleID)
        {
            BasePage objBasePageLLM = Page as BasePage;
            try
            {
                DataRow[] moduleItems;
                if (objBasePageLLM.GetSessionValue(SessionItems.Left_Menu) != null)
                {
                    dtLeftModules = (DataTable)objBasePageLLM.GetSessionValue(SessionItems.Left_Menu);
                    moduleItems = dtLeftModules.Select("Module_ID = " + sModuleID);
                    if (dtLeftModules != null)
                    {
                        mnLeftMenu.Items.Clear();
                        MenuItem objLeftMenuItem;
                        foreach (DataRow drRow in moduleItems)
                        {
                            objLeftMenuItem = new MenuItem();
                            objLeftMenuItem.Text = " " + drRow["Menu_Name"].ToString();
                            objLeftMenuItem.Value = " " + drRow["Screen_ID"].ToString().Trim();
                            if(drRow["Url"].ToString().Contains("?"))
                                objLeftMenuItem.NavigateUrl = drRow["Url"].ToString() + "&Mode=N";
                            else
                                objLeftMenuItem.NavigateUrl = drRow["Url"].ToString() + "?Mode=N";
                            switch (sModuleID)
                            {
                                case "1":
                                    mnLeftMenu.CssClass = "LeftMenuSecNavMiddle";                                    
                                    break;
                                case "3":
                                    mnLeftMenu.CssClass = "LeftMenuMarNavMiddle";                                    
                                    break;
                                case "4":
                                    mnLeftMenu.CssClass = "LeftMenuRegNavMiddle";                                    
                                    break;
                                case "5":
                                    mnLeftMenu.CssClass = "LeftMenuHosNavMiddle";                                    
                                    break;
                                case "6":
                                    mnLeftMenu.CssClass = "LeftMenuHCMNavMiddle";                                    
                                    break;
                                case "7":
                                    mnLeftMenu.CssClass = "LeftMenuFinNavMiddle";                                    
                                    break;
                                case "8":
                                    mnLeftMenu.CssClass = "LeftMenuRegNavMiddle";                                    
                                    break;
                                case "9":
                                    mnLeftMenu.CssClass = "LeftMenuLibNavMiddle";                                    
                                    break;
                                case "11":
                                    mnLeftMenu.CssClass = "LeftMenuAcaNavMiddle";                                    
                                    break;
                                case "14":
                                    mnLeftMenu.CssClass = "LeftMenuTransNavMiddle";                                    
                                    break;
                                case "15":
                                    mnLeftMenu.CssClass = "LeftMenuHCMNavMiddle";                                    
                                    break;
                                case "default":
                                    mnLeftMenu.CssClass = "LeftMenuNavMiddle";                                    
                                    break;
                            }                            
                            mnLeftMenu.Items.Add(objLeftMenuItem);
                        }
                    }
                }
                else
                {
                    objBasePageLLM.GetTopandLeftMenuItems();
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objBasePageLLM != null) objBasePageLLM = null;
            }
        }

        protected void LoadTreeViewMenuItems(Guid? guidUserID, string sModuleID)
        {
            BasePage objBasePageLTV = Page as BasePage;
            DataSet dsTest = null;
            try
            {
                UserAccessBO objUserAccessBO = new UserAccessBO(objBasePageLTV.ConnectionString);
                objUserAccessBO.UsersID = guidUserID;
                objUserAccessBO.ModuleID = Convert.ToInt32(sModuleID);
                dsTest = ProjMngTrack.Security.UserAccessBLL.GetTreeViewMenuDetails(objUserAccessBO);
                
                if (dsTest != null)
                {
                    tvLeftMenu.Nodes.Clear();

                    if (dsTest.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsTest.Tables[0].Rows.Count; i++)
                        {
                            TreeNode objTreeNode = new TreeNode();

                            objTreeNode.Text = " " + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                            objTreeNode.Value = " " + dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim();
                            DataRow[] drRowArray = dsTest.Tables[1].Select(" Module_ID = '" + dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() + "' ");
                            foreach (DataRow drRow in drRowArray)
                            {
                                TreeNode objChildNode = new TreeNode();
                                objChildNode.Text = " " + drRow["Menu_Name"].ToString();
                                objChildNode.Value = " " + drRow["Screen_ID"].ToString().Trim();                                
                                if (drRow["Url"].ToString().Contains("?"))
                                    objChildNode.NavigateUrl = drRow["Url"].ToString() + "&Mode=N";
                                else
                                    objChildNode.NavigateUrl = drRow["Url"].ToString() + "?Mode=N";
                                //objChildNode.ImageUrl = "~/images/bullet.png";
                                //objChildNode.ImageUrl = "~/images/ball_glass_greenS.gif";
                                objTreeNode.ChildNodes.Add(objChildNode);
                            }

                            tvLeftMenu.Nodes.Add(objTreeNode);

                            if (i == 0 && string.IsNullOrEmpty(objBasePageLTV.LeftNode))
                                objTreeNode.Expand();
                            else if (objBasePageLTV.LeftNode.Trim() == dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim())
                            {
                                objTreeNode.Expand();
                            }
                            else
                                objTreeNode.Collapse();
                        }
                    }
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

        protected void LoadMenuItems(Guid? guidUserID, string sModuleID)
        {
            BasePage objBasePageLTV = Page as BasePage;
            DataSet dsTest = null;
            try
            {
                UserAccessBO objUserAccessBO = new UserAccessBO(objBasePageLTV.ConnectionString);
                objUserAccessBO.UsersID = guidUserID;
                objUserAccessBO.ModuleID = Convert.ToInt32(sModuleID);
                dsTest = ProjMngTrack.Security.UserAccessBLL.GetTreeViewMenuDetails(objUserAccessBO);

                if (dsTest != null)
                {
                    mnNewLeftMenu.Items.Clear();
                    

                    if (dsTest.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsTest.Tables[0].Rows.Count; i++)
                        {
                            MenuItem objMenuItem = new MenuItem();

                            objMenuItem.Text = " " + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                            objMenuItem.Value = " " + dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim();
                            DataRow[] drRowArray = dsTest.Tables[1].Select(" Module_ID = '" + dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() + "' ");
                            foreach (DataRow drRow in drRowArray)
                            {
                                MenuItem objChildMenuItem = new MenuItem();
                                objChildMenuItem.Text = " " + drRow["Menu_Name"].ToString();
                                objChildMenuItem.Value = " " + drRow["Screen_ID"].ToString().Trim();
                                if (drRow["Url"].ToString().Contains("?"))
                                    objChildMenuItem.NavigateUrl = drRow["Url"].ToString() + "&Mode=N";
                                else
                                    objChildMenuItem.NavigateUrl = drRow["Url"].ToString() + "?Mode=N";
                                //objChildNode.ImageUrl = "~/images/bullet.png";
                                //objChildNode.ImageUrl = "~/images/ball_glass_greenS.gif";
                                objMenuItem.ChildItems.Add(objChildMenuItem);
                            }

                            mnNewLeftMenu.Items.Add(objMenuItem);
                            objMenuItem.ChildItems[0].Selected = true;
                            

                            //if (i == 0 && string.IsNullOrEmpty(objBasePageLTV.LeftNode))
                            //    objMenuItem.Expand();

                            //else if (objBasePageLTV.LeftNode.Trim() == dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim())
                            //{
                            //    objMenuItem.Expand();
                            //}
                            //else
                            //    objMenuItem.Collapse();
                        }
                    }
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
        protected void ReLoadTopMenuList(string sSelectedAction)
        {
            BasePage objBasePageRL = Page as BasePage;
            try
            {
                int iTotalRecordsCount = 0;
                int iTotalLoopCount = 0;
                int iStartLoop = 0;
                
                //mnTopMenu.Items.Clear();

                if (objBasePageRL.GetSessionValue(SessionItems.Top_Menu) != null)
                    dtTopModules = (DataTable)objBasePageRL.GetSessionValue(SessionItems.Top_Menu);
                else
                {
                    objBasePageRL.GetTopandLeftMenuItems();
                    dtTopModules = (DataTable)objBasePageRL.GetSessionValue(SessionItems.Top_Menu);
                }

                //MenuItem objTopReMenuItem = new MenuItem();
                if (dtTopModules != null)
                {
                    iTotalRecordsCount = dtTopModules.Rows.Count;
                    if (sSelectedAction.Equals(BasePage.PREVIOUS))
                    {
                        if (iShowingTopMenuCount < iTotalRecordsCount)
                            iTotalLoopCount = iShowingTopMenuCount;
                        else
                            iTotalLoopCount = dtTopModules.Rows.Count;
                    }
                    else if (sSelectedAction.Equals(BasePage.NEXT))
                    {
                        if (iShowingTopMenuCount < iTotalRecordsCount)
                            iStartLoop = dtTopModules.Rows.Count - iShowingTopMenuCount;
                        else
                            iStartLoop = 0;
                        iTotalLoopCount = dtTopModules.Rows.Count;
                    }
                    else
                    {
                        if (iShowingTopMenuCount < iTotalRecordsCount)
                            iTotalLoopCount = iShowingTopMenuCount;
                        else
                            iTotalLoopCount = dtTopModules.Rows.Count;
                    }
                    if (iShowingTopMenuCount < iTotalRecordsCount)
                    {
                        //tdLeftArrow.Visible = true;
                        //tdRightArrow.Visible = true;
                    }
                    else
                    {
                        //tdLeftArrow.Visible = false;
                        //tdRightArrow.Visible = false;
                    }
                    if (dtTopModules.Rows.Count > 0)
                    {
                        for (int i = iStartLoop; i < iTotalLoopCount; i++)
                        {   
                            if (i < iTotalLoopCount)
                            {
                                MenuItem objTopReMenuItem1 = new MenuItem();
                                objTopReMenuItem1.Text = dtTopModules.Rows[i]["Module_Name"].ToString();
                                objTopReMenuItem1.Value = dtTopModules.Rows[i]["Module_ID"].ToString();
                                //mnTopMenu.Items.Add(objTopReMenuItem1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, "Rethrow_Policy"))
                    throw;
            }
            finally
            {
                if (objBasePageRL != null) objBasePageRL = null;
            }
        }

        public void SelectLeftMenu()
        {
            BasePage objBasePageSLM = Page as BasePage;
            try
            {
                // Get the Parent Screen Url because we set the Add , Edit , Delete Permission for the Parent Screen Url only.
                UserAccessBO objUserAccessBO = new UserAccessBO(objBasePageSLM.ConnectionString);
                objUserAccessBO.FilePath = this.Request.FilePath.ToString().Trim();
                string sParent_Screen_Url = UserAccessBLL.GetParentScreenUrl(objUserAccessBO);

                foreach (MenuItem item in mnLeftMenu.Items)
                {
                    if (item.NavigateUrl.Substring(0, item.NavigateUrl.IndexOf('?')) == sParent_Screen_Url.Trim())
                    {
                        item.Selected = true;                        
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objBasePageSLM != null) objBasePageSLM = null;
            }
        }

        public void SelectTreeViewMenu()
        {
            BasePage objBasePageTVM = Page as BasePage;
            try
            {
                if (!this.Request.FilePath.Contains("Home.aspx"))
                {
                    //tvLeftMenu.CollapseAll();

                    UserAccessBO objUserAccessBO = new UserAccessBO(objBasePageTVM.ConnectionString);
                    // Get the Parent Screen Url because we set the Add , Edit , Delete Permission for the Parent Screen Url only.
                    objUserAccessBO.FilePath = this.Request.FilePath.ToString().Trim();
                    string sParent_Screen_Url = UserAccessBLL.GetParentScreenUrl(objUserAccessBO);

                    foreach (TreeNode objTreeNode in tvLeftMenu.Nodes)
                    {
                        bool blnCollapse = false;                        

                        foreach (TreeNode objChildNode in objTreeNode.ChildNodes)
                        {
                            if (objChildNode.NavigateUrl.Contains("/HCM/StaffReports.aspx"))
                            {
                                if (objChildNode.NavigateUrl.ToString().Trim() == sParent_Screen_Url.Trim() + "?" + this.Request.QueryString.ToString())
                                {
                                    objChildNode.Selected = true;
                                    blnCollapse = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (objChildNode.NavigateUrl.Substring(0, objChildNode.NavigateUrl.IndexOf('?')) == sParent_Screen_Url.Trim())
                                {
                                    objChildNode.Selected = true;
                                    blnCollapse = true;
                                    break;
                                }
                            }
                        }

                        if (blnCollapse)
                            objTreeNode.Expand();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objBasePageTVM != null) objBasePageTVM = null;
            }
        }
        
        public void SelectTopMenu()
        {
            BasePage objBasePageTM = Page as BasePage;
            try
            {
                if (objBasePageTM.GetSessionValue(SessionItems.Module_ID) != null)
                {
                    if (!string.IsNullOrEmpty(objBasePageTM.GetSessionValue(SessionItems.Module_ID).ToString()))
                    {
                        //foreach (MenuItem item in mnTopMenu.Items)
                        //{
                        //    if (System.IO.Path.GetFileName(item.Value) == objBasePageTM.GetSessionValue(SessionItems.Module_ID).ToString().Trim())
                        //    {
                        //        item.Selected = true;
                        //        break;
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, BasePage.Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objBasePageTM != null) objBasePageTM = null;
            }
        }

        #endregion
        protected void lbMenu_Click(object sender, EventArgs e)
        {
            BasePage objBasePageMTM = Page as BasePage;
            try
            {
                //if (!StringFunctions.IsNullOrEmpty(e.Item.Value))
                //{
                //    if (!StringFunctions.IsNullOrEmpty(objBasePageMTM.GetSessionValue(SessionItems.Previous_Next)))
                //    {
                //        ReLoadTopMenuList(objBasePageMTM.GetSessionValue(SessionItems.Previous_Next).ToString().Trim().ToLower());
                //        if (!StringFunctions.IsNullOrEmpty(e.Item.Value.ToString().Trim()))
                //            SelectTopMenu();
                //    }
                //    objBasePageMTM.SetSessionValue(SessionItems.Module_ID, e.Item.Value.ToString().Trim());
                //    objBasePageMTM.SetSessionValue(SessionItems.Module_Name, e.Item.Text.ToString().Trim());

                //    if (e.Item.Value.ToString() == CtrlAModules.CtrlA_MODULES.SETUP || e.Item.Value.ToString() == CtrlAModules.CtrlA_MODULES.REPORTS)
                //    {
                //        tvLeftMenu.Visible = true;
                //        mnLeftMenu.Visible = false;

                //        LoadTreeViewMenuItems(objBasePageMTM.LoggedInUserId, e.Item.Value.ToString());
                //        Response.Redirect("~/Home.aspx", false);
                //    }
                //    else
                //    {
                //        tvLeftMenu.Visible = false;
                //        mnLeftMenu.Visible = true;

                //        LoadLeftMenuItems(e.Item.Value.ToString());
                //        e.Item.Selected = true;
                //        Response.Redirect("~/Home.aspx", false);
                //    }
                //}
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


               protected void LoadMenuWithUL(Guid? guidUserID, string sModuleID)
        {
            BasePage objBasePageLTV = Page as BasePage;
            DataSet dsTest = null;
            try
            {
                UserAccessBO objUserAccessBO = new UserAccessBO(objBasePageLTV.ConnectionString);
                objUserAccessBO.UsersID = guidUserID;
                objUserAccessBO.ModuleID = Convert.ToInt32(sModuleID);
                dsTest = ProjMngTrack.Security.UserAccessBLL.GetTreeViewMenuDetails(objUserAccessBO);

                if (dsTest != null)
                {
                    HtmlGenericControl ul, ulchild, liSub, li, anchor;
                    Image img=null;
                    ul = new HtmlGenericControl("ul");
                    LinkButton lbAnchor;
                    if (dsTest.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsTest.Tables[0].Rows.Count; i++)
                        {
                            liSub = new HtmlGenericControl("li");
                            //liSub.InnerText = dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                            //img = new Image();
                            //img.ImageUrl = "../images/" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString().Replace(" ", "_") + ".png";
                            //img.ID = "img_Icon_" + dsTest.Tables[0].Rows[i]["Module_ID"].ToString();
                            ////liSub.Controls.Add(img);
                            anchor = new HtmlGenericControl("a");
                            anchor.InnerHtml = "<img src='../images/" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString().Replace(" ", "_") + ".png'>&nbsp;&nbsp;&nbsp;&nbsp;" + dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                            anchor.Attributes.Add("href", "#");
                            liSub.Controls.Add(anchor);
                            liSub.Attributes.Add("class", "active has-sub");
                            ulchild = new HtmlGenericControl("ul");
                            //ulchild.InnerText = dsTest.Tables[0].Rows[i]["Menu_Name"].ToString();
                            DataRow[] drRowArray = dsTest.Tables[1].Select(" Module_ID = '" + dsTest.Tables[0].Rows[i]["Module_ID"].ToString().Trim() + "' ");
                            foreach (DataRow drRow in drRowArray)
                            {
                                li = new HtmlGenericControl("li");
                                li.Attributes.Add("class", "has-sub");
                                //img = new Image();
                                //img.ImageUrl = "#";
                                //img.ID = "img_Icon_" + drRow["Screen_ID"].ToString();
                                //li.Controls.Add(img);
                                lbAnchor = new LinkButton();
                                lbAnchor.Text = " " + drRow["Menu_Name"].ToString();
                                lbAnchor.ID = "mnLink_" + drRow["Screen_ID"].ToString();
                                if (drRow["Url"].ToString().Contains("?"))
                                    lbAnchor.Attributes.Add("href", drRow["Url"].ToString() + "&Mode=N");
                                else
                                    lbAnchor.Attributes.Add("href", drRow["Url"].ToString() + "?Mode=N");
                                lbAnchor.Visible = true;
                                lbAnchor.Click += new EventHandler(lbMenu_Click);
                                if (HttpContext.Current.Request.Path == Convert.ToString(drRow["Url"]))
                                {
                                    ulchild.Style.Add("display", "block");
                                    lbAnchor.Style.Add("background-color", "#293541");
                                    lbAnchor.Style.Add("color", "white");
                                    lbAnchor.Style.Add("border-top", "1px solid #000000");
                                }
                                li.Controls.Add(lbAnchor);                                
                                
                                ulchild.Controls.Add(li);
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





       

    }
}
