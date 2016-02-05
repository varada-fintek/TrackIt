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
using System.Xml.Linq;
using System.Xml;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;


//using TrackIT.BusinessObjects;
using TrackIT.WebApp.Common;

using TrackIT.WebApp.TrackITEnum;
using TrackIT.Common;
using TrackIT.Security;

using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI;
using Infragistics.Documents.Excel;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace TrackIT.WebApp.Security
{
    public partial class AccessRoles : BasePage
    {
        #region Declarations
        //RoleBO objRoleBO;
        //RoleAccessBO objRoleAccess;
        public string XMLData { get; set; }
        int intCurrentPageIndex;
        WebDataGrid lwdg_RoleMasterGrid;
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
         
        private string istr_getroles = "Security_GetRoleDetails";
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
                PropertyValueControls();
                lwdg_RoleMasterGrid = new WebDataGrid();
                //lwsm_ScriptManager = new WebScriptManager();
                pnl_RoleGrid.Controls.Add(lwdg_RoleMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_RoleMasterGrid);

                if (!IsPostBack)
                {
                    //FS_ID 4.9.1.1
                    //Unit Testing ID - AccessRoles.aspx.cs_1
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - AccessRoles.aspx.cs_1 Role Type");
                    if (Request.QueryString["Mode"] != null)
                    {
                        if (!string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()) && Request.QueryString["Mode"].ToString().Equals("N"))
                        {
                           
                            hdnIndex.Value = "1";
                        }
                    }
                    else if (!string.IsNullOrEmpty(CurrentPageIndex.ToString().Trim()))
                    {
                        hdnIndex.Value = this.CurrentPageIndex.ToString();
                    }
                    GetRoleDetails();
                    ClearControls();
                }
               // GetRoleDetails();
                if (!string.IsNullOrEmpty(hdnRoleID.Value) && hdnpop.Value == "1")
                {
                    Guid? RoleId = Conversion.ConvertStringToGuid(hdnRoleID.Value.ToString());
                    //Guid userid = Conversion.ConvertStringToGuid(hdnBUID.Value);
                    EnableDiableControls(true);
                    btnSave.Visible = bitEdit;
                    GetRoleDetails(RoleId);
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                    ModalPopupExtender1.Show();
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

                Page.Validate("vgrpSave");
                if (Page.IsValid)
                    InsertorUpdateRoles();
                else
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
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
                EnableDiableControls(true);
                ClearControls();
                PropertyValueControls();
                GetRoleDetails();
                ModalPopupExtender1.Hide();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

      

        

    

        #region ddlBusinessUnit_SelectedIndexChanged
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBusinessUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            //    if (ddlBusinessUnit.Items.Count > 0)
                { 
            //        if (ddlBusinessUnit.Items[0].Text.ToString() == "All Business Units")
                    {
             //           reqvddlBusinessUnit.Visible = false;
                    }                
               }
            //    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
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
        #region InsertorUpdateRoles
        protected void InsertorUpdateRoles()
        {
            try
            {
               // if (objRoleBO == null) objRoleBO = new RoleBO(this.ConnectionString);
                // SqlConnection Conn = new SqlConnection(objRoleBO.ConnectionString);
                //objRoleBO.RoleID = Conversion.ConvertStringToGuid(hdnRoleID.Value);
                //objRoleBO.RoleCode = (!string.IsNullOrEmpty(txtRoleCode.Text)) ? txtRoleCode.Text.Trim() : null;
                //objRoleBO.RoleName = (!string.IsNullOrEmpty(txtRoleName.Text)) ? txtRoleName.Text.Trim() : null;
                //objRoleBO.RoleType = "CUS";
                //objRoleBO.Active = chkinactive.Checked;
                //objRoleBO.CreatedBy = this.LoggedInUserId;
                //objRoleBO.RowVersion = Conversion.StringToByte(hdnRowVersion.Value);

                XmlDocument doc = new XmlDocument();
                XmlNode RootNode = XmlUtility.AddChildXmlNode(doc, null, "SCREENS", "");

                for (int i = 0; i <= gvRoleAccess.Rows.Count - 1; i++)
                {
                    
                    Label objlblScreenID = (Label)gvRoleAccess.Rows[i].FindControl("lblScreenID");
                    if (objlblScreenID.Text.Trim() != "0")
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
                }
                doc.AppendChild(RootNode);
                 XMLData = doc.InnerXml;
            
                SqlParameter[] objParams = {
                        new SqlParameter ("@RoleID",  Conversion.ConvertStringToGuid(hdnRoleID.Value.Replace("'", "''"))),
                        new SqlParameter ("@RoleCode", txtRoleCode.Text.Replace("'", "''")),
                        new SqlParameter ("@RoleName", txtRoleName.Text.Replace("'", "''")),
                        new SqlParameter ("@RoleType","CUS"),
                        new SqlParameter ("@Active", chkinactive.Checked ? 1 : 0),
                        new SqlParameter ("@XMLData", XMLData),
                        new SqlParameter ("@CreatedBy", this.LoggedInUserId),
                        new SqlParameter ("@RowVersion", Conversion.StringToByte(hdnRowVersion.Value))
                    };
                string Usp_INSERT_OR_UPDATE_ROLES = "Security_InsertOrUpdateRoles";
               DataSet lds_dsResult = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, Usp_INSERT_OR_UPDATE_ROLES, objParams);
               // objRoleBO.dsResult = ldbh_QueryExecutors.ExecuteDataSetSP("[USP_GET_USER_FUND_DETAILS]");
               string sOutput = lds_dsResult.Tables[0].Rows[0][0].ToString();
                if (sOutput.Contains("SUCCESS^"))
                {
                    string[] sRoleID = sOutput.Split('^');
                   // ucPropControls.InsertorUpdatePropertyTransValues("ROLES", (!string.IsNullOrEmpty(sRoleID[1])) ? Conversion.ConvertStringToGuid(sRoleID[1]) : null);
                    SaveMessage();
                    GetRoleDetails(); 
                    ClearControls();
                    return;
                }
                else
                {
                    switch (sOutput.ToUpper())
                    {
                        case EXISTS:
                            AlreadyExistMessage();
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                            return;
                        case CONCURRENCY:
                            ConcurrencyMessage();
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                            return;
                        default:
                            Response.Redirect("~/Security/AccessRoles.aspx", false);
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
    //            if (objRoleBO != null) objRoleBO = null;
            }
        }
        #endregion

        #region GetRoleDetails
        /// <summary>
        /// Get Role Details
        /// </summary>
        private void GetRoleDetails()
        {
            //RoleBO objRole = new RoleBO(this.ConnectionString);
            try
            {
               // btnClear_Click(null, null);

                if (hdnSortExpression.Value.Trim().Length == 0)
                    hdnSortExpression.Value = "Role_Name";
                if (hdnAcsDesc.Value.Trim().Length == 0)
                    hdnAcsDesc.Value = "Asc";

                if (hdnIndex.Value.Trim().Length == 0 || hdnIndex.Value == "0")
                    hdnIndex.Value = "1";

                //objRole.LoginUserID = this.LoggedInStaffID;
                //objRole.IsAdminRole = this.IsAdminRole;
                //objRole.SortBy = hdnSortExpression.Value;
                //objRole.SortOrder = hdnAcsDesc.Value;

                //objRole = RoleBLL.GetRoleDetails(objRole);

                //SqlParameter[] objParams = { 
                //       // new SqlParameter ("@guidLoginUserID", this.LoggedInUserId),
                //        //new SqlParameter ("@bitIsAdminRole", this.IsAdminRole),
                //        new SqlParameter ("@RoleID", Conversion.ConvertStringToGuid(hdnRoleID.Value)),
                //        //new SqlParameter ("@BUID", objRole.BUID),
                //        new SqlParameter ("@RoleName", objRole.RoleName),
                //        new SqlParameter ("@BUName", objRole.BUName),
                //        new SqlParameter ("@Order", objRole.SortOrder),
                //        new SqlParameter ("@Pageindex", objRole.PageIndex),
                //        new SqlParameter ("@Pagesize",  objRole.PageSize),
                //        new SqlParameter ("@SortBy", objRole.SortBy)
                //    };

               DataSet lds_dsResult = ldbh_QueryExecutors.ExecuteDataSet("SELECT SR.Role_ID, SR.Role_Code, SR.Role_Name, SR.Is_Predefined, SR.Role_Type, SR.Active FROM Security_Roles SR");
                lwdg_RoleMasterGrid.InitializeRow += lwdg_RoleMasterGrid_InitializeRow;
                lwdg_RoleMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
                lwdg_RoleMasterGrid.Columns.Add(td);

                lwdg_RoleMasterGrid.Visible = false;
                if (lds_dsResult != null)
                {
                    if (lds_dsResult.Tables.Count > 0)
                    {
                        if (lds_dsResult.Tables[0].Rows.Count > 0)
                        {
                           
                            ViewState["export"] = (DataTable)lds_dsResult.Tables[0];
                            lwdg_RoleMasterGrid.DataSource = lds_dsResult.Tables[0];
                            lwdg_RoleMasterGrid.DataBind();
                            lwdg_RoleMasterGrid.Visible = true;
                        }

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
                //if (objRole != null) objRole = null;
            }
        }
        #endregion


        #region initializerow for user grid
        protected void lwdg_RoleMasterGrid_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
        {

            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("Role_ID").Column.Hidden = true;
                e.Row.Items.FindItemByKey("Is_Predefined").Column.Hidden = true;
                e.Row.Items.FindItemByKey("Role_Type").Column.Hidden = true;
              
                e.Row.Items.FindItemByKey("Role_Code").Column.Header.Text = RollupText("Role", "gridRoleCode");
                e.Row.Items.FindItemByKey("Role_Code").Column.CssClass = "Dataalign";
                e.Row.Items.FindItemByKey("Role_Name").Column.Header.Text = RollupText("Role", "gridRoleName");
                e.Row.Items.FindItemByKey("Role_Name").Column.CssClass = "Dataalign";
                e.Row.Items.FindItemByKey("Active").Column.Header.Text = RollupText("Common", "gvActive");
                if (!IsPostBack)
                {
                    for (int i = 0; i < e.Row.Items.Count; i++)
                    {
                        if (e.Row.Items[i].Column.Type.FullName.ToString().Equals("System.String") && !string.IsNullOrEmpty(e.Row.Items[i].Column.Key))
                        {
                            ColumnFilter filter = new ColumnFilter();
                            filter.ColumnKey = e.Row.Items[i].Column.Key;
                            filter.Condition = new RuleTextNode(TextFilterRules.Contains, "");
                            lwdg_RoleMasterGrid.Behaviors.Filtering.ColumnFilters.Add(filter);
                        }
                    }
                }

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
                //if (objRoleBO == null) objRoleBO = new RoleBO(this.ConnectionString);

//                objRoleBO.RoleID = guidRoleID;
                //objRoleBO = RoleBLL.GetRoleDetails(objRoleBO);

                DataSet lds_dsresult = ldbh_QueryExecutors.ExecuteDataSet("SELECT SR.Role_ID, SR.Role_Code, SR.Role_Name, SR.Is_Predefined, SR.Role_Type, SR.Active FROM Security_Roles SR where SR.Role_ID='"+guidRoleID+"'");
                if (lds_dsresult.Tables[0].Rows.Count>0)
                {
                    hdnRoleID.Value = (!string.IsNullOrEmpty(Convert.ToString(lds_dsresult.Tables[0].Rows[0]["Role_ID"]))) ? Convert.ToString(lds_dsresult.Tables[0].Rows[0]["Role_ID"]).Trim() : string.Empty;
                    txtRoleCode.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_dsresult.Tables[0].Rows[0]["Role_Code"]))) ? Convert.ToString(lds_dsresult.Tables[0].Rows[0]["Role_Code"]).Trim() : string.Empty;
                    txtRoleName.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_dsresult.Tables[0].Rows[0]["Role_Name"]))) ? Convert.ToString(lds_dsresult.Tables[0].Rows[0]["Role_Name"]).Trim() : string.Empty;
                    chkinactive.Checked = Convert.ToInt32(lds_dsresult.Tables[0].Rows[0]["Active"]) == 1 ? true : false;
                    GetRoleAccessDetails(Conversion.ConvertStringToGuid(hdnRoleID.Value));
                    PropertyValueControls();
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
  //              if (objRoleBO != null) objRoleBO = null;
            }
        }
        #endregion

        #region GetRoleAccessDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sRoleID"></param>
        /// <param name="sModuleID"></param>
        private void GetRoleAccessDetails(Guid? guidRoleID)
        {
            try
            {
                //if (objRoleAccess == null) objRoleAccess = new RoleAccessBO(this.ConnectionString);

 //               objRoleAccess.RoleID = guidRoleID;
               // objRoleAccess = RoleAccessBLL.GetRoleAccess(objRoleAccess);
                SqlParameter[] objParams = { 
                        new SqlParameter ("@RoleID", guidRoleID)
                    };
                string Usp_INSERT_OR_UPDATE_ROLES = "Security_GetRoleAccess";
                DataSet lds_dsResult = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, Usp_INSERT_OR_UPDATE_ROLES, objParams);
                   // objRoleAccess.dsResult = SqlHelper.ExecuteDataset(Conn, GET_ROLE_ACCESS, objParams)

                if (lds_dsResult.Tables.Count > 0)
                {
                    //FS_ID 4.9.1.4
                    //Unit Testing ID - AccessRoles.aspx.cs_4
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - AccessRoles.aspx.cs_4 " + guidRoleID.ToString());

                    //FS_ID 4.9.1.7
                    //Unit Testing ID - AccessRoles.aspx.cs_7
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - AccessRoles.aspx.cs_7 " + guidRoleID.ToString());

                    //FS_ID 4.9.1.9
                    //Unit Testing ID - AccessRoles.aspx.cs_8
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - AccessRoles.aspx.cs_8 " + guidRoleID.ToString());

                    gvRoleAccess.DataSource = lds_dsResult;
                    gvRoleAccess.DataBind();
                    gvRoleAccess.Visible = true;
                   // NoRecordsAccess.Visible = false;
                }
                else
                {
                    gvRoleAccess.Visible = false;
                   // NoRecordsAccess.Visible = true;
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                //if (objRoleAccess != null) objRoleAccess = null;
            }
        }
        #endregion

        #region EnableDiableControls
        private void EnableDiableControls(bool bValue)
        {
            try
            {
               // ddlBusinessUnit.Enabled = bValue;
               
                txtRoleName.Enabled = bValue;
                chkinactive.Enabled = bValue;
                gvRoleAccess.Enabled = bValue;
                txtRoleCode.Enabled = bValue;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region ClearControls
        private void ClearControls()
        {
            try
            {
                hdnRoleID.Value = string.Empty;
                hdnRowVersion.Value = string.Empty;
                //ddlBusinessUnit.SelectedIndex = -1;
                txtRoleCode.Text = "";
                txtRoleName.Text = string.Empty;
                chkinactive.Checked = true;
                chkinactive.Enabled = false;


                gvRoleAccess.Visible = false;
               // NoRecordsAccess.Visible = true;


               
                
                GetRoleAccessDetails(Conversion.ConvertStringToGuid(hdnRoleID.Value));

                //ddlBusinessUnit.Enabled = true;
                btnSave.Visible = bitAdd;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region ControlNames
        /// <summary>
        /// ControlNames
        /// </summary>
        private void ControlNames()
        {
            try
            {
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("Role", "lblCaption");
                lblCreateRoles.Text = RollupText("Role", "lblRoleHeader");
                lblRoleCode.Text = RollupText("Role", "lblRoleCode");
                lblRoleName.Text = RollupText("Role", "lblRoleName");
                lblInactive.Text = RollupText("Common", "lblActive");


                sConfirmation = RollupText("Common", "DeleteRecord");
                btnSave.Text = RollupText("Common", "btnSave");
                btnClear.Text = RollupText("Common", "btnCancel");
                reqvtxtRoleName.ErrorMessage = RollupText("Role", "reqvtxtRoleName_ErrorMsg");
                reqvtxtRoleCode.ErrorMessage = RollupText("Role", "reqvtxtRoleCode_ErrorMsg");

                gvRoleAccess.Columns[0].HeaderText = RollupText("Role", "gridScreenName");
                gvRoleAccess.Columns[1].HeaderText = RollupText("Role", "gridCategoryName");
                gvRoleAccess.Columns[1].Visible = false;

                if (!bitAdd)
                    createnew.Style.Add("display", "none");

            
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region PropertyValueControls
        protected void PropertyValueControls()
        {
           // ucPropControls.UCFIRM_ID = this.LoggedInFirmId;
           //// ucPropControls.UCBU_ID = (!string.IsNullOrEmpty(ddlBusinessUnit.SelectedValue)) ? Conversion.ConvertStringToGuid(ddlBusinessUnit.SelectedValue) : null; ;
           // ucPropControls.UCMODULE_ID = "ROLES";
           // ucPropControls.UCPROP_ID = this.LoggedInFirmId;
           // ucPropControls.UCPROP_TRANS_ID = (!string.IsNullOrEmpty(hdnRoleID.Value)) ? Conversion.ConvertStringToGuid(hdnRoleID.Value) : null;

           // if (btnSave.Visible == false)
           //     ucPropControls.UCVIEW_MODE = "VIEW";
           // else
           //     ucPropControls.UCVIEW_MODE = "";

           // ucPropControls.getControls();
        }
        #endregion
        #endregion

       

      

        #region "Access Grid Events"
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

                    Label lblScreenID = (Label)e.Row.FindControl("lblScreenID");
                    if (lblScreenID.Text.Trim() == "0")
                    {
                        e.Row.Cells[0].ColumnSpan = e.Row.Cells.Count; 
                        e.Row.CssClass = "success";
                        for (int i = 1; i < e.Row.Cells.Count; i++)
                        {
                            e.Row.Cells[i].Visible = false;
                        }
                    }
                    else
                    {

                        CheckBox objchkView = (CheckBox)e.Row.FindControl("chkView");
                        CheckBox objchkAdd = (CheckBox)e.Row.FindControl("chkAdd");
                        CheckBox objchkEdit = (CheckBox)e.Row.FindControl("chkEdit");
                        CheckBox objchkDelete = (CheckBox)e.Row.FindControl("chkDelete");

                        objchkView.Attributes.Add("onclick", "return UnSelectCheckboxes('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");
                        objchkAdd.Attributes.Add("onclick", "return MinimumOneCheckboxSelected('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");
                        objchkEdit.Attributes.Add("onclick", "return MinimumOneCheckboxSelected('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");
                        objchkDelete.Attributes.Add("onclick", "return MinimumOneCheckboxSelected('" + objchkView.ClientID + "','" + objchkAdd.ClientID + "','" + objchkEdit.ClientID + "','" + objchkDelete.ClientID + "')");

                        //FS_ID 4.9.1.10
                        //Unit Testing ID - AccessRoles.aspx.cs_9
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - AccessRoles.aspx.cs_9 Hide Firm Menu");


                    }
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

        #region Export
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["export"];
            lwdg_RoleMasterGrid.DataSource = dt;
            lwdg_RoleMasterGrid.DataBind();
            WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
            WebExcelExporter.Export(lwdg_RoleMasterGrid);
            WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;

            this.WebExcelExporter.Export(this.lwdg_RoleMasterGrid);
        }

        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["export"];
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_RoleMasterGrid);
            lwdg_RoleMasterGrid.DataSource = dt;
            lwdg_RoleMasterGrid.DataBind();
            WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
            WebPDFExporter.Export(lwdg_RoleMasterGrid);
            WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;

            this.WebPDFExporter.Export(this.lwdg_RoleMasterGrid);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

     

        #endregion

    }
}
