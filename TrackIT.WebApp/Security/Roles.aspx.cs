using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

using ProjMngTrack.WebApp.Common;
using ProjMngTrack.WebApp.CtrlAEnum;
using ProjMngTrack.Common;
using ProjMngTrack.Security;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;


namespace ProjMngTrack.WebApp.Security
{
    public partial class Roles : BasePage
    {
        #region Declarations
        RoleBO objRole;
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
                    txtRoleName.Focus();

                    if ((bool)GetSessionValue(SessionItems.Super_User))
                        GetDropdownValues(ddlModuleName, DropdownFilter.Module_Name);
                    else
                        GetDropdownValues(ddlModuleName, DropdownFilter.Modules_By_Role, GetSessionValue(SessionItems.Role_ID).ToString());

                    if (!StringFunctions.IsNullOrEmpty(Request.QueryString["RoleID"]))
                    {
                        btnSave.Visible = bitEdit;
                        btnClear.Visible = false;
                        GetRoleDetails(Conversion.ConvertStringToGuid(Request.QueryString["RoleID"].ToString()));
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
                {
                    if (GetSessionValue(SessionItems.Role_ID) != null)
                        GetRoleAccessDetails(Conversion.ConvertStringToGuid(hdnRoleID.Value), Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.Role_ID).ToString()), ddlModuleName.SelectedValue);
                    else
                        GetRoleAccessDetails(Conversion.ConvertStringToGuid(hdnRoleID.Value), null, ddlModuleName.SelectedValue);
                }
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
                InsertOrUpdateRoles();
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
                txtRoleName.Text = string.Empty;
                ddlModuleName.SelectedIndex = 0;

                if (gvRoleAccess.HeaderRow != null)
                {
                    CheckBox htView = (CheckBox)gvRoleAccess.HeaderRow.FindControl("htView");
                    CheckBox htAdd = (CheckBox)gvRoleAccess.HeaderRow.FindControl("htAdd");
                    CheckBox htEdit = (CheckBox)gvRoleAccess.HeaderRow.FindControl("htEdit");
                    CheckBox htDelete = (CheckBox)gvRoleAccess.HeaderRow.FindControl("htDelete");

                    if (htView != null)
                        htView.Checked = false;
                    if (htAdd != null)
                        htAdd.Checked = false;
                    if (htEdit != null)
                        htEdit.Checked = false;
                    if (htDelete != null)
                        htDelete.Checked = false;
                }

                foreach (GridViewRow dr in gvRoleAccess.Rows)
                {
                    CheckBox chkAdd = (CheckBox)dr.Cells[0].FindControl("chkAdd");
                    CheckBox chkView = (CheckBox)dr.Cells[0].FindControl("chkView");
                    CheckBox chkEdit = (CheckBox)dr.Cells[0].FindControl("chkEdit");
                    CheckBox chkDelete = (CheckBox)dr.Cells[0].FindControl("chkDelete");

                    if (chkAdd != null)
                        chkAdd.Checked = false;
                    if (chkView != null)
                        chkView.Checked = false;
                    if (chkEdit != null)
                        chkEdit.Checked = false;
                    if (chkDelete != null)
                        chkDelete.Checked = false;
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
                Response.Redirect("~/Security/RoleList.aspx", false);
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

        #region gvRoleAccess_RowDataBound
        /// <summary>
        /// Each Item of the Grid is binded for ever Row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected void gvRoleAccess_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    CheckBox objchkViewSelectAll = (CheckBox)e.Row.FindControl("htView");
                    CheckBox objchkAddSelectAll = (CheckBox)e.Row.FindControl("htAdd");
                    CheckBox objchkEditSelectAll = (CheckBox)e.Row.FindControl("htEdit");
                    CheckBox objchkDeleteSelectAll = (CheckBox)e.Row.FindControl("htDelete");

                    objchkViewSelectAll.Text = RollupText("Role", "htView");
                    objchkAddSelectAll.Text = RollupText("Role", "htAdd");
                    objchkEditSelectAll.Text = RollupText("Role", "htEdit");
                    objchkDeleteSelectAll.Text = RollupText("Role", "htDelete");

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
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("Role", "lblCaption");
                lblRoleHeader.Text = RollupText("Role", "lblRoleHeader");
                lblRoleName.Text = RollupText("Role", "lblRoleName");
                lblModuleName.Text = RollupText("Role", "lblModuleName");
                chkAdmin.Text = RollupText("Role", "chkAdmin");
                rfvRoleName.ErrorMessage = RollupText("Role", "rfvRoleName");
                rfvModuleName.ErrorMessage = RollupText("Role", "rfvModuleName");

                //btnSave.Text = RollupText("Common", "btnSave");
                //btnClear.Text = RollupText("Common", "btnClear");
                //btnCancel.Text = RollupText("Common", "btnCancel");

                gvRoleAccess.Columns[0].HeaderText = RollupText("Role", "gridScreenName");
                gvRoleAccess.Columns[1].HeaderText = RollupText("Role", "gridCategoryName");
                //if (chkAdmin.Checked == true)
                //{
                //    gvRoleAccess.Rows[1].Visible = false;
                //    gvRoleAccess.Rows[3].Visible = false;
                //}
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;

            }
        }
        #endregion        

        #region InsertOrUpdateRoles
        /// <summary>
        /// 
        /// </summary>
        private void InsertOrUpdateRoles()
        {
            try
            {
                if (objRole == null) objRole = new RoleBO(this.ConnectionString);

                objRole.RoleID = Conversion.ConvertStringToGuid(hdnRoleID.Value);
                objRole.RoleName = (!string.IsNullOrEmpty(txtRoleName.Text)) ? txtRoleName.Text.Trim() : null;
                objRole.IsAdmin = chkAdmin.Checked;
                objRole.CreatedBy = this.LoggedInUserId;
                objRole.RowVersion = Conversion.StringToByte(hdnRowVersion.Value);
                objRole.ModuleID = Convert.ToInt32(ddlModuleName.SelectedValue.ToString());

                XmlDocument doc = new XmlDocument();
                XmlNode RootNode = XmlUtility.AddChildXmlNode(doc, null, "SCREENS", "");

                for (int i = 0; i <= gvRoleAccess.Rows.Count - 1; i++)
                {
                    CheckBox chkView = (CheckBox)gvRoleAccess.Rows[i].FindControl("chkView");
                    CheckBox chkAdd = (CheckBox)gvRoleAccess.Rows[i].FindControl("chkAdd");
                    CheckBox chkEdit = (CheckBox)gvRoleAccess.Rows[i].FindControl("chkEdit");
                    CheckBox chkDelete = (CheckBox)gvRoleAccess.Rows[i].FindControl("chkDelete");
                    Label lblScreenID = (Label)gvRoleAccess.Rows[i].FindControl("lblScreenID");

                    Label objlblView_Screen = (Label)gvRoleAccess.Rows[i].FindControl("lblView_Screen");
                    Label objlblAdd_Screen = (Label)gvRoleAccess.Rows[i].FindControl("lblAdd_Screen");
                    Label objlblEdit_Screen = (Label)gvRoleAccess.Rows[i].FindControl("lblEdit_Screen");
                    Label objlblDelete_Screen = (Label)gvRoleAccess.Rows[i].FindControl("lblDelete_Screen");

                    String sView = "0";
                    String sAdd = "0";
                    String sEdit = "0";
                    String sDelete = "0";

                    if( Convert.ToBoolean( objlblView_Screen.Text.Trim() ) )
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
                objRole.XMLData = doc.InnerXml;

                objRole = RoleBLL.InsertOrUpdateRoles(objRole);
                string sOutput = objRole.OutMessage;
                if (sOutput.Contains("SUCCESS^"))
                {
                    string[] sRoleID = sOutput.Split('^');
                    SaveMessage();
                    btnSave.Visible = bitEdit;
                    btnClear.Visible = false;
                    GetRoleDetails(Conversion.ConvertStringToGuid(sRoleID[1]));
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
                            Response.Redirect("~/Security/RoleList.aspx", false);
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
                if (objRole != null) objRole = null;
            }
        }
        #endregion

        #region GetRoleDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRoleID"></param>
        private void GetRoleDetails(Guid? guidRoleID)
        {
            try
            {
                if (objRole == null) objRole = new RoleBO(this.ConnectionString);

                objRole.RoleID = guidRoleID;
                objRole = RoleBLL.GetRoleDetails(objRole);

                if (!StringFunctions.IsNullOrEmpty(objRole))
                {
                    hdnRoleID.Value = (!string.IsNullOrEmpty(objRole.RoleID.ToString())) ? objRole.RoleID.ToString() : null;
                    txtRoleName.Text = (!string.IsNullOrEmpty(objRole.RoleName)) ? objRole.RoleName.Trim() : null;
                    chkAdmin.Checked = objRole.IsAdmin;

                    if (!StringFunctions.IsNullOrEmpty(objRole.ModuleID))
                    {
                        if (ddlModuleName.Items.FindByValue(objRole.ModuleID.ToString().Trim().ToLower()) != null)
                        {
                            ddlModuleName.SelectedValue = objRole.ModuleID.ToString().Trim().ToLower();
                            if (GetSessionValue(SessionItems.Role_ID) != null)
                                GetRoleAccessDetails(Conversion.ConvertStringToGuid(hdnRoleID.Value), Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.Role_ID).ToString()), ddlModuleName.SelectedValue);
                            else
                                GetRoleAccessDetails(Conversion.ConvertStringToGuid(hdnRoleID.Value), null, ddlModuleName.SelectedValue);
                            
                        }
                        else
                            ddlModuleName.SelectedIndex = 0;
                    }
                    else
                        ddlModuleName.SelectedIndex = 0;

                    hdnRowVersion.Value = Conversion.ByteToString(objRole.RowVersion);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objRole != null) objRole = null;
            }
        }
        #endregion

        #region GetRoleAccessDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRoleID"></param>
        /// <param name="sModuleID"></param>
        private void GetRoleAccessDetails(Guid? guidRoleID, Guid? guidParentRoleID, string sModuleID)
        {
            try
            {
                if (objRoleAccess == null) objRoleAccess = new RoleAccessBO(this.ConnectionString);

                objRoleAccess.RoleID = guidRoleID;
                objRoleAccess.ParentRoleID = guidParentRoleID;
                objRoleAccess.ModuleID = IntegerFunctions.ToInt32(sModuleID);
                objRoleAccess = RoleAccessBLL.GetRoleAccess(objRoleAccess);

                if (objRoleAccess.dsResult.Tables.Count > 0)
                {
                    gvRoleAccess.DataSource = objRoleAccess.dsResult;
                    gvRoleAccess.DataBind();
                    gvRoleAccess.Visible = true;
                    ucNoRecords.Visible = false;
                    trHeadingRow.Visible = true;
                    trBottomRow.Visible = true;
                }
                else
                {
                    gvRoleAccess.Visible = false;
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
                if (objRoleAccess != null) objRoleAccess = null;
            }
        }
        #endregion

        #endregion
    }
}
