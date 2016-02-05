using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using ProjMngTrack.BusinessLogic;
using ProjMngTrack.BusinessObjects;
using ProjMngTrack.WebApp.Common;
using ProjMngTrack.WebApp.CtrlAEnum;
using ProjMngTrack.Common;
using ProjMngTrack.Security;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ProjMngTrack.WebApp.Security
{
    public partial class RoleList : BasePage
    {
        int intCurrentPageIndex;

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
                txtRoleName.Focus();
                if (!IsPostBack)
                {
                    gvRole.PageSize = IntegerFunctions.ToInt16(PageSize);
                    GetRoleDetails();
                }

                if (!string.IsNullOrEmpty(hdnPageCount.Value))
                {
                    this.ucPaging.PageCount = IntegerFunctions.ToInt32(hdnPageCount.Value);
                    this.ucPaging.PagingPageSize = IntegerFunctions.ToInt16(PageNavigationSize);
                    this.ucPaging.SetPageIndex += new UserControls.PagingControl.GridPagingEventHandler(SetPageIndex);
                    this.ucPaging.GetPageIndex += new UserControls.PagingControl.GridPagingEventHandler(GetPageIndex);
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion

        #region Search Button Click
        /// <summary>
        /// Search Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                hdnIndex.Value = "0";
                hdnAcsDesc.Value = "Asc";
                hdnSortExpression.Value = string.Empty;
                GetRoleDetails();

                this.ucPaging.SetPageCount(0, IntegerFunctions.ToInt32(hdnPageCount.Value));
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region Add Button Click
        /// <summary>
        /// Add Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Security/Roles.aspx", false);
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
                GetRoleDetails();
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

        #region GetRoleDetails
        /// <summary>
        /// Get Role Details
        /// </summary>
        private void GetRoleDetails()
        {
            RoleBO objRole = new RoleBO(this.ConnectionString);
            try
            {              
                if (hdnSortExpression.Value.Trim().Length == 0)
                    hdnSortExpression.Value = "Role_Name";
                if (hdnAcsDesc.Value.Trim().Length == 0)
                    hdnAcsDesc.Value = "Asc";

                if (hdnIndex.Value.Trim().Length == 0 || hdnIndex.Value == "0")
                    hdnIndex.Value = "1";

                objRole.RoleName = (!string.IsNullOrEmpty(txtRoleName.Text)) ? txtRoleName.Text.Trim().Replace("'", "''") : null ;

                objRole.PageIndex = IntegerFunctions.ToInt16(hdnIndex.Value);
                objRole.PageSize = IntegerFunctions.ToInt16(gvRole.PageSize);
                objRole.SortBy = hdnSortExpression.Value;
                objRole.SortOrder = hdnAcsDesc.Value;

                objRole = RoleBLL.GetRoleDetails(objRole);

                gvRole.Visible = false;
                ucNoRecords.Visible = true;

                if (objRole.dsRole != null )
                {
                    if (objRole.dsRole.Tables.Count > 1)
                    {
                        if (objRole.dsRole.Tables[0].Rows.Count > 0)
                        {
                            gvRole.DataSource = objRole.dsRole.Tables[0];
                            gvRole.DataBind();
                            gvRole.Visible = true;
                            ucNoRecords.Visible = false;                            
                        }                                        

                        if (IntegerFunctions.ToInt32(objRole.dsRole.Tables[1].Rows[0][0]) > gvRole.PageSize)
                            ucPaging.Visible = true;
                        else
                            ucPaging.Visible = false;

                        if (hdnIndex.Value.Trim().Length != 0)
                            intCurrentPageIndex = IntegerFunctions.ToInt32(hdnIndex.Value) - 1;
                        else
                            intCurrentPageIndex = 0;
                        ucPaging.SetPageCount(intCurrentPageIndex, IntegerFunctions.ToInt16(Math.Ceiling(Convert.ToDecimal(objRole.dsRole.Tables[1].Rows[0][0]) / Convert.ToDecimal(gvRole.PageSize))));
                        hdnPageCount.Value = ucPaging.PageCount.ToString();
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

        #region ControlNames
        /// <summary>
        /// ControlNames
        /// </summary>
        private void ControlNames()
        {
            try
            {
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("Role", "lblListCaption");
                lblSearchHeader.Text = RollupText("Role", "lblSearchHeader");
                lblRoleName.Text = RollupText("Role", "lblRoleName");

                //btnAdd.Text = RollupText("Common", "btnAdd");
                //btnSearch.Text = RollupText("Common", "btnSearch");
                //btnClear.Text = RollupText("Common", "btnClear");

                sConfirmation = RollupText("Common", "DeleteRecord");

                gvRole.Columns[0].HeaderText = RollupText("Role", "gridRoleName");
                gvRole.Columns[1].HeaderText = RollupText("Common", "gridAdmin");
                gvRole.Columns[2].HeaderText = RollupText("Common", "gridEdit");
                gvRole.Columns[3].HeaderText = RollupText("Common", "gridDelete");
                
                gvRole.Columns[2].Visible = bitEdit;
                gvRole.Columns[3].Visible = bitDelete;
                

                btnAdd.Visible = bitAdd;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion
        
        #endregion

        #region "Grid Events"

        #region gvRole_RowDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void gvRole_RowDataBound(Object src, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("imgDelete");
                    btnDelete.OnClientClick = "return confirm ('" + sConfirmation + "')";
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion        

        #region gvRole_RowDeleting
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void gvRole_RowDeleting(Object src, GridViewDeleteEventArgs e)
        {
            CommonBO objCommonBO = new CommonBO();
            try
            {
                Guid? guidRoleID = Conversion.ConvertStringToGuid(((Label)gvRole.Rows[e.RowIndex].FindControl("lblRoleID")).Text);
                string sRowVersion = ((Label)gvRole.Rows[e.RowIndex].FindControl("lblRowVersion")).Text;
                if (guidRoleID != null)
                {
                    objCommonBO.ModuleName = typeof(CtrlAModules.Security).Name;
                    objCommonBO.PageName = CtrlAModules.Security.ROLE;
                    objCommonBO.ID = guidRoleID;
                    objCommonBO.CreatedBy = this.LoggedInUserId;
                    objCommonBO.RowVersion = Conversion.StringToByte(sRowVersion);
                    objCommonBO.OutMessage = DeleteRecords(objCommonBO);
                    if (!string.IsNullOrEmpty(objCommonBO.OutMessage.Trim()))
                    {
                        if (objCommonBO.OutMessage.ToUpper().Equals(CONCURRENCY))
                            ConcurrencyMessage();
                        else if (objCommonBO.OutMessage.ToUpper().Equals(SUCCESS))
                            DeleteMessage();
                        else
                            ReferenceDeleteMessage(objCommonBO.OutMessage);                        
                    }
                    if (gvRole.Rows.Count == 1 && IntegerFunctions.ToInt32(hdnPageCount.Value) > 1)
                        gvRole.PageIndex = IntegerFunctions.ToInt32(hdnIndex.Value) - 1;
                    else
                        gvRole.PageIndex = IntegerFunctions.ToInt32(hdnIndex.Value);
                    hdnIndex.Value = StringFunctions.ToString(gvRole.PageIndex);
                    GetRoleDetails();
                }               
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
            finally
            {
                if (objCommonBO != null) objCommonBO = null;
            }
        }
        #endregion

        #region gvRole_RowSorting
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvRole_RowSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (hdnAcsDesc.Value.Equals("Asc"))
                    hdnAcsDesc.Value = "Desc";
                else if (hdnAcsDesc.Value.Equals("Desc"))
                    hdnAcsDesc.Value = "Asc";
                hdnSortExpression.Value = e.SortExpression;
                GetRoleDetails();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #endregion

        #region "Pagination Events"

        #region SetGridIndex
        /// <summary>
        /// 
        /// </summary>
        /// <param name="iPageIndex"></param>
        private void SetGridIndex(int iPageIndex)
        {
            try
            {
                gvRole.PageIndex = iPageIndex;
                GetRoleDetails();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region SetPageIndex
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetPageIndex(object sender, ProjMngTrack.WebApp.UserControls.PagingControl.GridPagingEventArgs e)
        {
            try
            {
                hdnIndex.Value = (e.PageIndex + 1).ToString();
                SetGridIndex(e.PageIndex);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region GetPageIndex
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetPageIndex(object sender, ProjMngTrack.WebApp.UserControls.PagingControl.GridPagingEventArgs e)
        {
            try
            {
                this.ucPaging.PageIndex = this.gvRole.PageIndex;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #endregion
    }
}
