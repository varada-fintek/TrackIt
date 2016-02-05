using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace TrackIT.WebApp
{
    public class BasePageUserControl : System.Web.UI.UserControl
    {
        #region "Constants"
        public const string EXISTS = "EXISTS";
        public const string CONCURRENCY = "CONCURRENCY";
        public const string SUCCESS = "SUCCESS";
        public const string WORKFLOWNOTMAPPED = "WORKFLOWNOTMAPPED";

        public const string Log_Only_Policy = "Log_Only_Policy";
        public const string Rethrow_Policy = "Rethrow_Policy";

        //MARKETING PLANNER EXPENSES
        public const string EXPENSE = "EXPENSE";
        public const string BUDGET = "BUDGET";
        #endregion

        private Guid? m_UCFIRM_ID;
        public Guid? UCFIRM_ID
        {
            get { return m_UCFIRM_ID; }
            set { m_UCFIRM_ID = value; }
        }

        private Guid? m_UCBU_ID;
        public Guid? UCBU_ID
        {
            get { return m_UCBU_ID; }
            set { m_UCBU_ID = value; }
        }

        private Guid? m_UCUSER_APP_ID;
        public Guid? UCUSER_APP_ID
        {
            get { return m_UCUSER_APP_ID; }
            set { m_UCUSER_APP_ID = value; }
        }

        private Guid? m_UCAPP_ID;
        public Guid? UCAPP_ID
        {
            get { return m_UCAPP_ID; }
            set { m_UCAPP_ID = value; }
        }

        private string m_UCREQUEST_TYPE;
        public string UCREQUEST_TYPE
        {
            get { return m_UCREQUEST_TYPE; }
            set { m_UCREQUEST_TYPE = value; }
        }

        private string m_UCMODULE_ID;
        public string UCMODULE_ID
        {
            get { return m_UCMODULE_ID; }
            set { m_UCMODULE_ID = value; }
        }

        private string m_UCVIEW_MODE;
        public string UCVIEW_MODE
        {
            get { return m_UCVIEW_MODE; }
            set { m_UCVIEW_MODE = value; }
        }

        private Guid? m_UCPROP_ID;
        public Guid? UCPROP_ID
        {
            get { return m_UCPROP_ID; }
            set { m_UCPROP_ID = value; }
        }

        private Guid? m_UCPROP_TRANS_ID;
        public Guid? UCPROP_TRANS_ID
        {
            get { return m_UCPROP_TRANS_ID; }
            set { m_UCPROP_TRANS_ID = value; }
        }

        private Guid? m_UCTRN_WORKFLOW_STEP_ID;
        public Guid? UCTRN_WORKFLOW_STEP_ID
        {
            get { return m_UCTRN_WORKFLOW_STEP_ID; }
            set { m_UCTRN_WORKFLOW_STEP_ID = value; }
        }

        private string m_UCSTEP_ACTION;
        public string UCSTEP_ACTION
        {
            get { return m_UCSTEP_ACTION; }
            set { m_UCSTEP_ACTION = value; }
        }

        private Guid? m_UCSTEP_ACTION_USERID;
        public Guid? UCSTEP_ACTION_USERID
        {
            get { return m_UCSTEP_ACTION_USERID; }
            set { m_UCSTEP_ACTION_USERID = value; }
        }

        #region "Constructor"

        public BasePageUserControl()
        {
        }

        #endregion

        #region "UDF - Grid Events"

        protected void RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.RowState == DataControlRowState.Alternate)
                        e.Row.Attributes.Add("onmouseout", "this.className='AlternatingRowStyle';");
                    else
                        e.Row.Attributes.Add("onmouseout", "this.className='gvRowStyle';");

                    e.Row.Style.Add("Cssclass", "label");
                    e.Row.Attributes.Add("onmouseover", "this.className='AlternatingOnMouseOver';");

                    if (e.Row.FindControl("imgEdit") != null)
                        ((ImageButton)e.Row.FindControl("imgEdit")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

                    if (e.Row.FindControl("imgDelete") != null)
                        ((ImageButton)e.Row.FindControl("imgDelete")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

                    if (e.Row.FindControl("imgBtnEdit") != null)
                        ((ImageButton)e.Row.FindControl("imgBtnEdit")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

                    if (e.Row.FindControl("imgBtnDelete") != null)
                        ((ImageButton)e.Row.FindControl("imgBtnDelete")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion
           
        protected static void HideClearButton(Button btnClear)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["StaffID"]))
                btnClear.Visible = false;
        }

        public static void ExtendableDropDownListWidth(DropDownList ddlDropdown, string sWidth, string sValidatorID)
        {
            try
            {
                ddlDropdown.Attributes.Add("onmousedown", "DropDownListDefaultWidth(this.id,'" + sWidth + "','" + sValidatorID + "');");
                ddlDropdown.Attributes.Add("onchange", "DropDownListDefaultWidthOnChange(this.id,'" + sWidth + "','" + sValidatorID + "');");
                ddlDropdown.Attributes.Add("onblur", "DropDownListDefaultWidthOnChange(this.id,'" + sWidth + "','" + sValidatorID + "');");
                ddlDropdown.Attributes.Add("onkeypress", "DropDownListDefaultWidthOnKeyPress(this.id,'" + sWidth + "','" + sValidatorID + "');");
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

    }
}
