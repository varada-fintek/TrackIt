using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

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
    public partial class UserList : BasePage
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
                txtUserName.Focus();

                if (!IsPostBack)
                {
                    GetDropdownValues(ddlRoleName, DropdownFilter.Role_Name);
                    gvUser.PageSize = IntegerFunctions.ToInt16(PageSize);
                    GetUserDetails();
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
                GetUserDetails();
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
                Response.Redirect("~/Security/Users.aspx", false);
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
                txtStaffName.Text = string.Empty;
                ddlRoleName.SelectedIndex = 0;
                GetUserDetails();
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

        #region GetUserDetails
        /// <summary>
        /// Get User Details
        /// </summary>
        private void GetUserDetails()
        {
            UserBO objUser = new UserBO(this.ConnectionString);

            try
            {
                if (hdnSortExpression.Value.Trim().Length == 0)
                    hdnSortExpression.Value = "User_Name";
                if (string.IsNullOrEmpty(hdnAcsDesc.Value))
                    hdnAcsDesc.Value = "Asc";

                if (hdnIndex.Value.Trim().Length == 0 || hdnIndex.Value == "0")
                    hdnIndex.Value = "1";

                objUser.UserName = (!string.IsNullOrEmpty(txtUserName.Text)) ? txtUserName.Text.Trim().Replace("'", "''") : null;
                objUser.RoleID = (ddlRoleName.SelectedIndex > 0) ? new Guid(ddlRoleName.SelectedValue.ToString()) : (Guid?)null;
                objUser.StaffName = (!string.IsNullOrEmpty(txtStaffName.Text)) ? txtStaffName.Text.Trim().Replace("'", "''") : null;
                objUser.PageIndex = IntegerFunctions.ToInt16(hdnIndex.Value);
                objUser.PageSize = IntegerFunctions.ToInt16(gvUser.PageSize);
                objUser.SortBy = hdnSortExpression.Value;
                objUser.SortOrder = hdnAcsDesc.Value;

                objUser = UserBLL.GetUserDetails(objUser);

                gvUser.Visible = false;
                ucNoRecords.Visible = true;

                if (objUser.dsUser != null)
                {
                    if (objUser.dsUser.Tables.Count > 1)
                    {
                        if (objUser.dsUser.Tables[0].Rows.Count > 0)
                        {
                            gvUser.DataSource = objUser.dsUser.Tables[0];
                            gvUser.DataBind();
                            gvUser.Visible = true;
                            ucNoRecords.Visible = false;
                        }

                        if (IntegerFunctions.ToInt32(objUser.dsUser.Tables[1].Rows[0][0]) > gvUser.PageSize)
                            ucPaging.Visible = true;
                        else
                            ucPaging.Visible = false;

                        if (hdnIndex.Value.Trim().Length != 0)
                            intCurrentPageIndex = IntegerFunctions.ToInt32(hdnIndex.Value) - 1;
                        else
                            intCurrentPageIndex = 0;

                        ucPaging.SetPageCount(intCurrentPageIndex, IntegerFunctions.ToInt16(Math.Ceiling(Convert.ToDecimal(objUser.dsUser.Tables[1].Rows[0][0]) / Convert.ToDecimal(gvUser.PageSize))));
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
                if (objUser != null) objUser = null;
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
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("User", "lblListCaption");
                lblSearchHeader.Text = RollupText("User", "lblSearchHeader");
                lblRoleName.Text = RollupText("User", "lblRoleName");
                lblUserName.Text = RollupText("User", "lblUserName");
                lblStaffName.Text = RollupText("User", "lblStaffName");
                //btnAdd.Text = RollupText("Common", "btnAdd");
                //btnSearch.Text = RollupText("Common", "btnSearch");
                //btnClear.Text = RollupText("Common", "btnClear");

                sConfirmation = RollupText("Common", "DeleteRecord");

                gvUser.Columns[0].HeaderText = RollupText("User", "gridUserName");
                gvUser.Columns[1].HeaderText = RollupText("User", "gridStaffName");
                gvUser.Columns[2].HeaderText = RollupText("User", "gridPosition");
                gvUser.Columns[3].HeaderText = RollupText("Common", "gridEdit");
                gvUser.Columns[4].HeaderText = RollupText("Common", "gridDelete");

                gvUser.Columns[3].Visible = bitEdit;
                gvUser.Columns[4].Visible = bitDelete;

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

        #region gvUser_RowDataBound
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void gvUser_RowDataBound(Object src, GridViewRowEventArgs e)
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

        #region gvUser_RowDeleting
        /// <summary>
        /// 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="e"></param>
        protected void gvUser_RowDeleting(Object src, GridViewDeleteEventArgs e)
        {
            CommonBO objCommonBO = new CommonBO();
            try
            {
                Guid? guidUsersID = Conversion.ConvertStringToGuid(((Label)gvUser.Rows[e.RowIndex].FindControl("lblUsersID")).Text);
                string sRowVersion = ((Label)gvUser.Rows[e.RowIndex].FindControl("lblRowVersion")).Text;

                if (guidUsersID != null)
                {
                    objCommonBO.ModuleName = typeof(CtrlAModules.Security).Name;
                    objCommonBO.PageName = CtrlAModules.Security.USER;
                    objCommonBO.ID = guidUsersID;
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

                    if (gvUser.Rows.Count == 1 && IntegerFunctions.ToInt32(hdnPageCount.Value) > 1)
                        gvUser.PageIndex = IntegerFunctions.ToInt32(hdnIndex.Value) - 1;
                    else
                        gvUser.PageIndex = IntegerFunctions.ToInt32(hdnIndex.Value);

                    hdnIndex.Value = StringFunctions.ToString(gvUser.PageIndex);

                    GetUserDetails();
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

        #region gvUser_RowSorting
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUser_RowSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (hdnAcsDesc.Value.Equals("Asc"))
                    hdnAcsDesc.Value = "Desc";
                else if (hdnAcsDesc.Value.Equals("Desc"))
                    hdnAcsDesc.Value = "Asc";

                hdnSortExpression.Value = e.SortExpression;

                GetUserDetails();
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
                gvUser.PageIndex = iPageIndex;
                GetUserDetails();
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
                this.ucPaging.PageIndex = this.gvUser.PageIndex; ;
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
