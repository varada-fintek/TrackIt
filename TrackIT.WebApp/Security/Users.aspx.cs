using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;

using ProjMngTrack.WebApp.Common;
using ProjMngTrack.WebApp.CtrlAEnum;
using ProjMngTrack.Common;
using ProjMngTrack.Security;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ProjMngTrack.WebApp.Security
{
    public partial class Users : BasePage
    {
        #region Declarations

        UserBO objUser;
        UserAccessBO objUserAccess;
        RoleAccessBO objRoleAccess;

        #endregion

        #region "Events"

        #region Page Load Events
        /// <summary>
        /// Page Load Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ControlNames();
                Page.Validate();

                if (!IsPostBack)
                {
                    txtUserName.Focus();
                    GetDropdownValues(ddlRoleName, DropdownFilter.Role_Name);
                    GetModulesByRoleID(null);

                    if (!StringFunctions.IsNullOrEmpty(Request.QueryString["UsersID"]))
                    {   
                        btnSave.Visible = bitEdit;
                        btnClear.Visible = false;
                        GetUserDetails(Conversion.ConvertStringToGuid(Request.QueryString["UsersID"].ToString()));
                    }
                    else
                        btnSave.Visible = bitAdd;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region ddlRoleName_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlRoleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {       
                if (!StringFunctions.IsNullOrEmpty(ddlRoleName.SelectedValue))
                    GetModulesByRoleID(Conversion.ConvertStringToGuid(ddlRoleName.SelectedValue));
                txtPassword.Attributes.Add("value", txtPassword.Text);
                Page.Validate();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region ddlModuleName_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlModuleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlModuleName.SelectedIndex > -1)
                    GetUserAccess(Conversion.ConvertStringToGuid(hdnUserID.Value), Conversion.ConvertStringToGuid(ddlRoleName.SelectedValue), ddlModuleName.SelectedValue.ToString());

                txtPassword.Attributes.Add("value", txtPassword.Text);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region Save Button Click
        /// <summary>
        /// Save Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                InsertOrUpdateUsers();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region Clear Button Click
        /// <summary>
        /// Clear Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtPassword.Attributes.Add("value", string.Empty);
                ddlRoleName.SelectedIndex = 0;
                ddlModuleName.SelectedIndex = 0;

                if (gvUserAccess.HeaderRow != null)
                {
                    if (((CheckBox)gvUserAccess.HeaderRow.FindControl("htView")) != null)
                        ((CheckBox)gvUserAccess.HeaderRow.FindControl("htView")).Checked = false;

                    if (((CheckBox)gvUserAccess.HeaderRow.FindControl("htAdd")) != null)
                        ((CheckBox)gvUserAccess.HeaderRow.FindControl("htAdd")).Checked = false;

                    if (((CheckBox)gvUserAccess.HeaderRow.FindControl("htEdit")) != null)
                        ((CheckBox)gvUserAccess.HeaderRow.FindControl("htEdit")).Checked = false;

                    if (((CheckBox)gvUserAccess.HeaderRow.FindControl("htDelete")) != null)
                        ((CheckBox)gvUserAccess.HeaderRow.FindControl("htDelete")).Checked = false;
                }

                foreach (GridViewRow dr in gvUserAccess.Rows)
                {
                    if (((CheckBox)dr.Cells[0].FindControl("chkAdd")) != null)
                        ((CheckBox)dr.Cells[0].FindControl("chkAdd")).Checked = false;

                    if (((CheckBox)dr.Cells[0].FindControl("chkView")) != null)
                        ((CheckBox)dr.Cells[0].FindControl("chkView")).Checked = false;

                    if (((CheckBox)dr.Cells[0].FindControl("chkEdit")) != null)
                        ((CheckBox)dr.Cells[0].FindControl("chkEdit")).Checked = false;

                    if (((CheckBox)dr.Cells[0].FindControl("chkDelete")) != null)
                        ((CheckBox)dr.Cells[0].FindControl("chkDelete")).Checked = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region Cancel Button Click
        /// <summary>
        /// Cancel Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Security/UserList.aspx", false);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #endregion

        #region "Grid Events"

        #region gvUserAccess_RowDataBound
        /// <summary>
        /// Each Item of the Grid is binded for ever Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void gvUserAccess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    CheckBox objchkViewSelectAll = (CheckBox)e.Row.FindControl("htView");
                    CheckBox objchkAddSelectAll = (CheckBox)e.Row.FindControl("htAdd");
                    CheckBox objchkEditSelectAll = (CheckBox)e.Row.FindControl("htEdit");
                    CheckBox objchkDeleteSelectAll = (CheckBox)e.Row.FindControl("htDelete");

                    objchkViewSelectAll.Text = RollupText("User", "htView");
                    objchkAddSelectAll.Text = RollupText("User", "htAdd");
                    objchkEditSelectAll.Text = RollupText("User", "htEdit");
                    objchkDeleteSelectAll.Text = RollupText("User", "htDelete");

                    objchkViewSelectAll.Attributes.Add("onclick", "return fnViewSelectAll('" + objchkViewSelectAll.ClientID + "','" + objchkAddSelectAll.ClientID + "' ,'" + objchkEditSelectAll.ClientID + "' , '" + objchkDeleteSelectAll.ClientID + "')");
                    objchkAddSelectAll.Attributes.Add("onclick", "return fnAddSelectAll('" + objchkAddSelectAll.ClientID + "','" + objchkViewSelectAll.ClientID + "')");
                    objchkEditSelectAll.Attributes.Add("onclick", "return fnEditSelectAll('" + objchkEditSelectAll.ClientID + "','" + objchkViewSelectAll.ClientID + "')");
                    objchkDeleteSelectAll.Attributes.Add("onclick", "return fnDeleteSelectAll('" + objchkDeleteSelectAll.ClientID + "','" + objchkViewSelectAll.ClientID + "')");
                }

                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox objchkView = (CheckBox)e.Row.FindControl("chkView");
                    CheckBox objchkAdd = (CheckBox)e.Row.FindControl("chkAdd");
                    CheckBox objchkEdit = (CheckBox)e.Row.FindControl("chkEdit");
                    CheckBox objchkDelete = (CheckBox)e.Row.FindControl("chkDelete");

                    objchkView.Attributes.Add("onclick", "return UnSelectCheckboxes('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");
                    objchkAdd.Attributes.Add("onclick", "return MinimumOneCheckboxSelected('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");
                    objchkEdit.Attributes.Add("onclick", "return MinimumOneCheckboxSelected('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");
                    objchkDelete.Attributes.Add("onclick", "return MinimumOneCheckboxSelected('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #endregion

        #region "User Defined Functions"

        #region ControlNames
        /// <summary>
        /// 
        /// </summary>
        private void ControlNames()
        {
            try
            {
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("User", "lblCaption");
                lblUserHeader.Text = RollupText("User", "lblUserHeader");
                lblUserName.Text = RollupText("User", "lblUserName");
                lblRoleName.Text = RollupText("User", "lblRoleName");
                lblModuleName.Text = RollupText("User", "lblModuleName");
                lblPassword.Text = RollupText("User", "lblPassword");

                rfvUserName.ErrorMessage = RollupText("User", "rfvUserName");
                rfvPassword.ErrorMessage = RollupText("User", "rfvPassword");
                rfvRoleName.ErrorMessage = RollupText("User", "rfvRoleName");
                rfvModuleName.ErrorMessage = RollupText("User", "rfvModuleName");
                
                //btnSave.Text = RollupText("Common", "btnSave");
                //btnClear.Text = RollupText("Common", "btnClear");
                //btnCancel.Text = RollupText("Common", "btnCancel");

                gvUserAccess.Columns[0].HeaderText = RollupText("User", "gridScreenName");
                gvUserAccess.Columns[1].HeaderText = RollupText("User", "gridCategoryName");
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region InsertOrUpdateUsers
        /// <summary>
        /// 
        /// </summary>
        private void InsertOrUpdateUsers()
        {
            try
            {
                if (objUser == null) objUser = new UserBO(this.ConnectionString);

                objUser.UsersID = Conversion.ConvertStringToGuid(hdnUserID.Value);
                objUser.UserName = (!string.IsNullOrEmpty(txtUserName.Text)) ? txtUserName.Text.Trim() : null;

                if (!string.IsNullOrEmpty(txtPassword.Text))
                    objUser.Password = Crypto.CreateHash(txtPassword.Text);

                if (ddlRoleName.SelectedIndex > 0)
                    objUser.RoleID = Conversion.ConvertStringToGuid(ddlRoleName.SelectedValue.ToString());

                objUser.UserType = "OTH";
                objUser.CreatedBy = this.LoggedInUserId;
                objUser.RowVersion = Conversion.StringToByte(hdnRowVersion.Value);

                //GETTING THE USER ACCESS RIGHTS
                XmlDocument doc = new XmlDocument();
                XmlNode RootNode = XmlUtility.AddChildXmlNode(doc, null, "SCREENS", "");

                for (int i = 0; i <= gvUserAccess.Rows.Count - 1; i++)
                {
                    CheckBox chkView = (CheckBox)gvUserAccess.Rows[i].FindControl("chkView");
                    CheckBox chkAdd = (CheckBox)gvUserAccess.Rows[i].FindControl("chkAdd");
                    CheckBox chkEdit = (CheckBox)gvUserAccess.Rows[i].FindControl("chkEdit");
                    CheckBox chkDelete = (CheckBox)gvUserAccess.Rows[i].FindControl("chkDelete");
                    Label lblScreenID = (Label)gvUserAccess.Rows[i].FindControl("lblScreenID");

                    Label objlblView_Screen = (Label)gvUserAccess.Rows[i].FindControl("lblView_Screen");
                    Label objlblAdd_Screen = (Label)gvUserAccess.Rows[i].FindControl("lblAdd_Screen");
                    Label objlblEdit_Screen = (Label)gvUserAccess.Rows[i].FindControl("lblEdit_Screen");
                    Label objlblDelete_Screen = (Label)gvUserAccess.Rows[i].FindControl("lblDelete_Screen");

                    string sView = "0";
                    string sAdd = "0";
                    string sEdit = "0";
                    string sDelete = "0";

                    if (Convert.ToBoolean(objlblView_Screen.Text.Trim()))
                    {
                        if (chkView.Checked)
                            sView = "1";
                    }

                    if (Convert.ToBoolean(objlblAdd_Screen.Text.Trim()))
                    {
                        if (chkAdd.Checked)
                            sAdd = "1";
                    }

                    if (Convert.ToBoolean(objlblEdit_Screen.Text.Trim()))
                    {
                        if (chkEdit.Checked)
                            sEdit = "1";
                    }

                    if (Convert.ToBoolean(objlblDelete_Screen.Text.Trim()))
                    {
                        if (chkDelete.Checked)
                            sDelete = "1";
                    }

                    XmlNode fieldname = XmlUtility.AddChildXmlNode(doc, RootNode, "SCREEN", "");
                    XmlUtility.AddXmlAttribute(doc, fieldname, "SCREEN_ID", lblScreenID.Text);
                    XmlUtility.AddXmlAttribute(doc, fieldname, "VIEW", sView);
                    XmlUtility.AddXmlAttribute(doc, fieldname, "ADD", sAdd);
                    XmlUtility.AddXmlAttribute(doc, fieldname, "EDIT", sEdit);
                    XmlUtility.AddXmlAttribute(doc, fieldname, "DELETE", sDelete);
                }
                doc.AppendChild(RootNode);

                objUser.XMLData = doc.InnerXml;
                objUser.ModuleID = Convert.ToInt32(ddlModuleName.SelectedValue.ToString());

                objUser = UserBLL.InsertOrUpdateUsers(objUser);
                string sOutput = objUser.OutMessage;
                if (sOutput.Contains("SUCCESS^"))
                {
                    string[] sUserID = sOutput.Split('^');
                    SaveMessage();
                    btnSave.Visible = bitEdit;
                    btnClear.Visible = false;
                    GetUserDetails(Conversion.ConvertStringToGuid(sUserID[1]));
                    return;
                }
                else
                {
                    switch (sOutput.ToUpper())
                    {
                        case EXISTS:
                            AlreadyExistMessage();
                            return;
                        case CONCURRENCY:
                            ConcurrencyMessage();
                            return;
                        default:
                            Response.Redirect("~/Security/UserList.aspx", false);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objUser != null) objUser = null;
            }
        }
        #endregion

        #region GetUserDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserID"></param>
        private void GetUserDetails(Guid? guidUserID)
        {
            try
            {
                if (objUser == null) objUser = new UserBO(this.ConnectionString);

                objUser.UsersID = guidUserID;
                objUser = UserBLL.GetUserDetails(objUser);

                if (!StringFunctions.IsNullOrEmpty(objUser))
                {
                    hdnUserID.Value = (!string.IsNullOrEmpty(objUser.UsersID.ToString())) ? objUser.UsersID.ToString() : null;
                    txtUserName.Text = (!string.IsNullOrEmpty(objUser.UserName)) ? objUser.UserName.Trim() : null;
                    if (objUser.UserType == "OTH")
                        txtUserName.Enabled = true;
                    else
                        txtUserName.Enabled = false;

                    if (objUser.Password == null)
                        tdPassword.Visible = true;
                    else
                        tdPassword.Visible = false;

                    if (!StringFunctions.IsNullOrEmpty(objUser.RoleID))
                    {
                        if (ddlRoleName.Items.FindByValue(objUser.RoleID.ToString()) != null)
                        {
                            ddlRoleName.SelectedValue = objUser.RoleID.ToString();
                            GetModulesByRoleID(Conversion.ConvertStringToGuid(ddlRoleName.SelectedValue));
                        }
                        else
                            ddlRoleName.SelectedIndex = 0;
                    }
                    else
                        ddlRoleName.SelectedIndex = 0;

                    hdnRowVersion.Value = Conversion.ByteToString(objUser.RowVersion);

                    if (!StringFunctions.IsNullOrEmpty(objUser.ModuleID))
                    {
                        if (ddlModuleName.Items.FindByValue(objUser.ModuleID.ToString().Trim()) != null)
                        {
                            ddlModuleName.SelectedValue = objUser.ModuleID.ToString().Trim();
                            GetUserAccess(Conversion.ConvertStringToGuid(hdnUserID.Value), Conversion.ConvertStringToGuid(ddlRoleName.SelectedValue), ddlModuleName.SelectedValue.ToString());
                        }
                        else
                            ddlModuleName.SelectedIndex = 0;
                    }
                    else
                        ddlModuleName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objUser != null) objUser = null;
            }
        }
        #endregion

        #region GetModulesByRoleID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserID"></param>
        /// <param name="sModuleID"></param>
        private void GetModulesByRoleID(Guid? guidRoleID)
        {
            try
            {
                if (objRoleAccess == null) objRoleAccess = new RoleAccessBO(this.ConnectionString);

                objRoleAccess.RoleID = guidRoleID;
                if ((bool)GetSessionValue(SessionItems.Super_User))
                    objRoleAccess.ParentRoleID = null;
                else
                    objRoleAccess.ParentRoleID = Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.Role_ID).ToString());
                objRoleAccess = RoleAccessBLL.GetModulesByRoleID(objRoleAccess);
                ddlModuleName.DataValueField = "Module_ID";
                ddlModuleName.DataTextField = "Module_Name";
                ddlModuleName.DataSource = objRoleAccess.dsResult;
                ddlModuleName.DataBind();

                ListItem li = new ListItem("Select", "");
                ddlModuleName.Items.Insert(0, li);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objRoleAccess != null) objRoleAccess = null;
            }
        }
        #endregion

        #region GetUserAccess
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserID"></param>
        /// <param name="sModuleID"></param>
        private void GetUserAccess(Guid? guidUserID, Guid? guidRoleID, string sModuleID)
        {
            try
            {
                if (objUserAccess == null) objUserAccess = new UserAccessBO(this.ConnectionString);

                objUserAccess.UsersID = guidUserID;
                objUserAccess.RoleID = guidRoleID;
                if (!string.IsNullOrEmpty(sModuleID))
                    objUserAccess.ModuleID = IntegerFunctions.ToInt32(sModuleID);
                else
                    objUserAccess.ModuleID = 0;
                objUserAccess = UserAccessBLL.GetUserAccess(objUserAccess);

                if (objUserAccess.dsUserAccess.Tables.Count > 0)
                {
                    gvUserAccess.DataSource = objUserAccess.dsUserAccess;
                    gvUserAccess.DataBind();
                    gvUserAccess.Visible = true;
                    ucNoRecords.Visible = false;
                    trHeadingRow.Visible = true;
                    trBottomRow.Visible = true;
                }
                else
                {
                    gvUserAccess.Visible = false;
                    ucNoRecords.Visible = true;
                    trHeadingRow.Visible = false;
                    trBottomRow.Visible = false;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objUserAccess != null) objUserAccess = null;
            }
        }
        #endregion

        #endregion
    }
}
