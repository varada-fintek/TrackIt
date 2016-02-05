using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using System.Xml.Linq;

using ProjMngTrack.BusinessLogic;
using ProjMngTrack.BusinessObjects;
using ProjMngTrack.WebApp;
using ProjMngTrack.WebApp.Common;
using ProjMngTrack.WebApp.Factory;
using ProjMngTrack.WebApp.CtrlAEnum;
using ProjMngTrack.Common;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace ProjMngTrack.WebApp.UserControls
{
    public partial class ucApplicationRequestControls : BasePageUserControl //System.Web.UI.UserControl //BasePageUserControl
    {
        #region Declaraction
        //CreateRequestBO objCreateRequestBO;
        //ICreateRequest objCreateRequestBase;
        string strFileName = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                try
                {
                    BasePage objBasePagePL = Page as BasePage;
                    objBasePagePL.GetDropdownValues(ddlUCAppName, DropdownFilter.User_Applications, objBasePagePL.LoggedInStaffID.ToString());
                }
                catch (Exception ex)
                {
                    ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                    Response.Redirect("~/Error.aspx", false);
                }
            }
        }

        #region  Selected Index Changed Application Name
        /// <summary>
        /// Selected Index Changed Application Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlUCAppName_SelectedIndexChanged(object sender, EventArgs e)
        {
            getControls();
            SetApplicationName();
        }
        #endregion

        public void getControls()
        {
            this.EnsureChildControls();
            this.CreateControlCollection();
            this.RemoveControl();
            this.getDynamicControls();
            this.ChildControlsCreated = true;
            //CreateChildControls();
        }

        private void getDynamicControls()
        {
            try
            {
                if (StringFunctions.IsNullOrEmpty(objCreateRequestBO)) objCreateRequestBO = new CreateRequestBO();
                objCreateRequestBase = SetupFactory.CreateCreateRequestObject("");
                objCreateRequestBO.APP_ID = ddlUCAppName.SelectedIndex> 0? Conversion.ConvertStringToGuid(ddlUCAppName.SelectedValue.ToString()) : null; 
                objCreateRequestBO.REQUEST_TYPE = UCREQUEST_TYPE;
                objCreateRequestBO.USER_APP_ID = UCUSER_APP_ID;
                objCreateRequestBO = objCreateRequestBase.GetApplicationRequestDetails(objCreateRequestBO);

                if (objCreateRequestBO.dsResult != null)
                {
                    BasePage objBasePagePL = Page as BasePage;
                    if (objCreateRequestBO.dsResult.Tables.Count > 0)
                    {

                        foreach (DataRow dr in objCreateRequestBO.dsResult.Tables[1].Rows)
                        {
                            phControls.Controls.Add(new LiteralControl("<div class='form-group'>"));

                            Label lb = new Label();
                            lb.CssClass = "control-label col-md-2";
                            lb.ID = "lbl" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                            if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                lb.Text = dr["Data_Point_Caption"].ToString().Trim() + "*";
                            else
                                lb.Text = dr["Data_Point_Caption"].ToString().Trim();

                            phControls.Controls.Add(lb);

                            if (dr["Data_Type_ID"].ToString().Trim() == "A")
                                phControls.Controls.Add(new LiteralControl("<div class='col-md-5'>"));
                            else
                                phControls.Controls.Add(new LiteralControl("<div class='col-md-3'>"));

                            string sControlType = dr["Data_Type_ID"].ToString().Trim();
                            switch (sControlType)
                            {
                                case "T":
                                    TextBox tb = new TextBox();
                                    tb.ID = "txt" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tb.TextMode = TextBoxMode.SingleLine;
                                    tb.MaxLength = 100;
                                    tb.Text = dr["Text_Value"].ToString().Trim();
                                    tb.CssClass = "form-control";
                                    tb.EnableViewState = true;
                                    if (UCVIEW_MODE == "VIEW") { tb.Enabled = false; }
                                    phControls.Controls.Add(tb);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tb.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please enter " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.EnableViewState = true;
                                        req.InitialValue = "";
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }

                                    break;

                                case "REM":
                                    TextBox tbREM = new TextBox();
                                    tbREM.ID = "rem" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tbREM.TextMode = TextBoxMode.SingleLine;
                                    tbREM.MaxLength = 100;
                                    tbREM.Text = dr["Text_Value"].ToString().Trim();
                                    tbREM.CssClass = "form-control";
                                    tbREM.EnableViewState = true;
                                    if (UCVIEW_MODE == "VIEW") { tbREM.Enabled = false; }
                                    phControls.Controls.Add(tbREM);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tbREM.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please enter " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.EnableViewState = true;
                                        req.InitialValue = "";
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;


                                case "UID":
                                    TextBox tbUID = new TextBox();
                                    tbUID.ID = "uid" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tbUID.TextMode = TextBoxMode.SingleLine;
                                    tbUID.MaxLength = 100;
                                    tbUID.Text = dr["Text_Value"].ToString().Trim();
                                    tbUID.CssClass = "form-control";
                                    tbUID.EnableViewState = true;
                                    if (UCVIEW_MODE == "VIEW") { tbUID.Enabled = false; }
                                    phControls.Controls.Add(tbUID);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tbUID.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please enter " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.EnableViewState = true;
                                        req.InitialValue = "";
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;
                                
                                case "SDT":
                                    TextBox tbSDTDate = new TextBox();
                                    tbSDTDate.CssClass = "form-control";
                                    tbSDTDate.MaxLength = 10;
                                    tbSDTDate.ID = "sdt" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tbSDTDate.TextMode = TextBoxMode.SingleLine;
                                    tbSDTDate.EnableViewState = true;
                                    tbSDTDate.Attributes["type"] = "date";
                                    tbSDTDate.Text = "";
                                    if (!StringFunctions.IsNullOrEmpty(dr["Date_Value"]))
                                    {
                                        DateTime dDateValue = DateTime.Now;
                                        if (DateTime.TryParse(dr["Date_Value"].ToString(), out dDateValue))
                                        {
                                            tbSDTDate.Text = dDateValue.ToString("yyyy-MM-dd");
                                        }
                                    }
                                    if (UCVIEW_MODE == "VIEW") { tbSDTDate.Enabled = false; }
                                    phControls.Controls.Add(tbSDTDate);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tbSDTDate.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.EnableViewState = true;
                                        req.InitialValue = "";
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;

                                case "EDT":
                                    TextBox tbEDTDate = new TextBox();
                                    tbEDTDate.CssClass = "form-control";
                                    tbEDTDate.MaxLength = 10;
                                    tbEDTDate.ID = "edt" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tbEDTDate.TextMode = TextBoxMode.SingleLine;
                                    tbEDTDate.EnableViewState = true;
                                    tbEDTDate.Attributes["type"] = "date";
                                    tbEDTDate.Text = "";
                                    if (!StringFunctions.IsNullOrEmpty(dr["Date_Value"]))
                                    {
                                        DateTime dDateValue = DateTime.Now;
                                        if (DateTime.TryParse(dr["Date_Value"].ToString(), out dDateValue))
                                        {
                                            tbEDTDate.Text = dDateValue.ToString("yyyy-MM-dd");
                                        }
                                    }
                                    if (UCVIEW_MODE == "VIEW") { tbEDTDate.Enabled = false; }
                                    phControls.Controls.Add(tbEDTDate);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tbEDTDate.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.EnableViewState = true;
                                        req.InitialValue = "";
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;


                                case "D":
                                    TextBox tbDate = new TextBox();
                                    tbDate.CssClass = "form-control";
                                    tbDate.MaxLength = 10;
                                    tbDate.ID = "dat" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tbDate.TextMode = TextBoxMode.SingleLine;
                                    tbDate.EnableViewState = true;
                                    tbDate.Attributes["type"] = "date";
                                    tbDate.Text = "";
                                    if (!StringFunctions.IsNullOrEmpty(dr["Date_Value"]))
                                    {
                                        DateTime dDateValue = DateTime.Now;
                                        if (DateTime.TryParse(dr["Date_Value"].ToString(), out dDateValue))
                                        {
                                            tbDate.Text = dDateValue.ToString("yyyy-MM-dd");
                                        }
                                    }
                                    if (UCVIEW_MODE == "VIEW") { tbDate.Enabled = false; }
                                    phControls.Controls.Add(tbDate);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tbDate.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.EnableViewState = true;
                                        req.InitialValue = "";
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;

                                case "APP":

                                    DropDownList APPDT = new DropDownList();
                                    APPDT.CssClass = "form-control chzn-select";
                                    APPDT.ID = "app" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    objBasePagePL.GetDropdownValues(APPDT, DropdownFilter.User_Applications, objBasePagePL.LoggedInStaffID.ToString());
                                    if (APPDT.Items.FindByValue(ddlUCAppName.SelectedValue.ToString().ToLower()) != null)
                                        APPDT.SelectedValue = ddlUCAppName.SelectedValue.ToString().ToLower();

                                    if (UCVIEW_MODE == "VIEW") { APPDT.Enabled = false; }
                                    phControls.Controls.Add(APPDT);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = APPDT.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.InitialValue = "";
                                        req.EnableViewState = true;
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;

                                case "ROL":

                                    DropDownList ROLDT = new DropDownList();
                                    ROLDT.CssClass = "form-control chzn-select";
                                    ROLDT.ID = "rol" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    objBasePagePL.GetDropdownValues(ROLDT, DropdownFilter.Application_Application_Roles, ddlUCAppName.SelectedValue.ToString());
                                    if (UCVIEW_MODE == "VIEW") { ROLDT.Enabled = false; }
                                    phControls.Controls.Add(ROLDT);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = ROLDT.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.InitialValue = "";
                                        req.EnableViewState = true;
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;

                                case "EQU":

                                    DropDownList EQUDT = new DropDownList();
                                    EQUDT.CssClass = "form-control chzn-select";
                                    EQUDT.ID = "equ" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    objBasePagePL.GetDropdownValues(EQUDT, DropdownFilter.Application_Application_Users, ddlUCAppName.SelectedValue.ToString());
                                    if (UCVIEW_MODE == "VIEW") { EQUDT.Enabled = false; }
                                    phControls.Controls.Add(EQUDT);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = EQUDT.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.InitialValue = "";
                                        req.EnableViewState = true;
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;

                                case "L":

                                    string sListID = dr["LIST_ID"].ToString().Trim();
                                    DataTable dtList = GetListValues(Conversion.ConvertStringToGuid(sListID));
                                    ListItem liItem;
                                    DropDownList dt = new DropDownList();
                                    dt.CssClass = "form-control chzn-select";
                                    dt.ID = "cmb" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    dt.Items.Add("");
                                    if (dtList != null)
                                    {
                                        foreach (DataRow drList in dtList.Rows)
                                        {
                                            liItem = new ListItem();
                                            liItem.Value = drList["Value_ID"].ToString().Trim();
                                            liItem.Text = drList["Value_Name"].ToString().Trim();
                                            dt.Items.Add(liItem);
                                        }

                                        if (dr["List_Value_ID"].ToString().Trim() != string.Empty)
                                        {
                                            for (int iIndex = 0; iIndex < dt.Items.Count; iIndex++)
                                            {
                                                if (dt.Items[iIndex].Value.ToString() == dr["List_Value_ID"].ToString().Trim())
                                                {
                                                    dt.SelectedIndex = iIndex;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (UCVIEW_MODE == "VIEW") { dt.Enabled = false; }
                                    phControls.Controls.Add(dt);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = dt.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.InitialValue = "";
                                        req.EnableViewState = true;
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }

                                    break;

                                case "N":
                                    TextBox tbNumber = new TextBox();
                                    tbNumber.ID = "num" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tbNumber.TextMode = TextBoxMode.SingleLine;
                                    tbNumber.MaxLength = 20;
                                    tbNumber.Text = dr["Number_Value"].ToString().Trim();
                                    tbNumber.CssClass = "form-control";
                                    tbNumber.Attributes["onkeypress"] = "return isNumberKey(event)";
                                    tbNumber.EnableViewState = true;
                                    if (UCVIEW_MODE == "VIEW") { tbNumber.Enabled = false; }
                                    phControls.Controls.Add(tbNumber);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tbNumber.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please enter " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.InitialValue = "";
                                        req.EnableViewState = true;
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));
                                    }
                                    break;

                                case "C":

                                    phControls.Controls.Add(new LiteralControl("<span class='input-icon'>"));
                                    CheckBox chk = new CheckBox();
                                    chk.ID = "chk" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    chk.CssClass = "checkbox";
                                    chk.EnableViewState = true;
                                    chk.Checked = dr["Checkbox_Value"].ToString().Trim().ToUpper() == "TRUE" ? true : false;
                                    if (UCVIEW_MODE == "VIEW") { chk.Enabled = false; }
                                    phControls.Controls.Add(chk);
                                    phControls.Controls.Add(new LiteralControl("</span>"));
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    break;

                                case "A":
                                    //TextBox tbAttachment = new TextBox();
                                    //tbAttachment.ID = "att" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    //tbAttachment.TextMode = TextBoxMode.SingleLine;
                                    //tbAttachment.MaxLength = 20;
                                    //tbAttachment.CssClass = "form-control";
                                    //tbAttachment.EnableViewState = true;
                                    //if (UCVIEW_MODE == "VIEW") { tbAttachment.Enabled = false; }
                                    //phControls.Controls.Add(tbAttachment);
                                    //phControls.Controls.Add(new LiteralControl("</div>"));

                                    FileUpload tbAttachment = new FileUpload();
                                    tbAttachment.ID = "att" + dr["Workflow_DataPoint_ID"].ToString().Trim();
                                    tbAttachment.CssClass = "form-control";
                                    tbAttachment.EnableViewState = true;
                                    if (UCVIEW_MODE == "VIEW") { tbAttachment.Enabled = false; }
                                    phControls.Controls.Add(tbAttachment);
                                    phControls.Controls.Add(new LiteralControl("</div>"));

                                    if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                    {
                                        phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                        RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                        req.ID = "req" + dr["Workflow_DataPoint_ID"].ToString().Trim(); // + iIndex.ToString().Trim();
                                        req.CssClass = "validator_msg";
                                        req.ControlToValidate = tbAttachment.ID;
                                        req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                        req.SetFocusOnError = true;
                                        req.Text = "*";
                                        req.ErrorMessage = "Please select " + dr["Data_Point_Caption"].ToString().Trim();
                                        req.ValidationGroup = "vgrpSave";
                                        req.EnableViewState = true;
                                        req.InitialValue = "";
                                        phControls.Controls.Add(req);
                                        phControls.Controls.Add(new LiteralControl("</div>"));

                                    }

                                    break;
                            }
                            phControls.Controls.Add(new LiteralControl("</div>"));
                        }
                        //phControls.Controls.Add(new LiteralControl("</tr>"));
                        //phControls.Controls.Add(new LiteralControl("</table>"));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

        private void SetApplicationName()
        {
            foreach (Control ctrl in phControls.Controls)
            {
                if (ctrl is DropDownList)
                {
                    DropDownList dt = ctrl as DropDownList;
                    if (dt != null)
                    {
                        string sPropID = dt.ID.ToString();
                        string sPropDataType = sPropID.Substring(0, 3);
                        if (sPropDataType.ToUpper().Trim() == "APP")
                        {
                            if (dt.Items.FindByValue(ddlUCAppName.SelectedValue.ToString().ToLower()) != null)
                                dt.SelectedValue = ddlUCAppName.SelectedValue.ToString().ToLower();
                        }
                    }
                }
            }

        }
        
        private DataTable GetListValues(Guid? guidListID)
        {
            DataTable dt = null;
            ListValuesBO objListValuesBO = new ListValuesBO();
            IListValues objListBase;
            try
            {
                objListValuesBO.ListID = guidListID;
                objListBase = SetupFactory.CreateListValuesDetailsObject("");
                objListValuesBO = objListBase.GetListValuesDetails(objListValuesBO);
                if (!StringFunctions.IsNullOrEmpty(objListValuesBO))
                {
                    if (objListValuesBO.dsResult != null)
                    {
                        if (objListValuesBO.dsResult.Tables.Count > 0)
                        {
                            if (objListValuesBO.dsResult.Tables[1].Rows.Count > 0)
                            {
                                dt = objListValuesBO.dsResult.Tables[2];
                            }
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objListValuesBO != null) objListValuesBO = null;
            }
            return dt;
        }

        private void RemoveControl()
        {
            try
            {
                phControls.Controls.Clear();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

        #region InsertorUpdatePropertyTransValues
        public void InsertorUpdatePropertyTransValues(string chvMODULE_ID, Guid? guidMODULE_UNIQUE_ID)
        {
            //BasePage objBasePagePL = Page as BasePage;
            //try
            //{
            //    if (StringFunctions.IsNullOrEmpty(objCreateRequestBO)) objCreateRequestBO = new CreateRequestBO();
            //    objCreateRequestBO.MODULE_ID = chvMODULE_ID;
            //    objCreateRequestBO.MODULE_UNIQUE_ID = guidMODULE_UNIQUE_ID;
            //    objCreateRequestBO.CreatedBy = objBasePagePL.LoggedInUserId;
            //    objCreateRequestBO.xmlPropValueList = null;

            //    string sPropID = "";
            //    string sPropValue = "";
            //    string sPropDataType = "";

            //    int InsertCount = 0;
            //    XmlDocument docInsert = new XmlDocument();
            //    XmlNode rnInsert = XmlUtility.AddChildXmlNode(docInsert, null, "VALUEINSERT", "");

            //    foreach (Control ctrl in phControls.Controls)
            //    {
            //        sPropID = "";
            //        sPropValue = "";
            //        sPropDataType = "";
            //        if (ctrl is TextBox)
            //        {
            //            TextBox tb = ctrl as TextBox;
            //            if (tb != null)
            //            {
            //                sPropID = tb.ID.ToString();
            //                sPropDataType = sPropID.Substring(0, 3);
            //                sPropID = sPropID.Substring(3, (sPropID.Length - 3));
            //                sPropValue = tb.Text;
            //            }
            //        }
            //        else if (ctrl is DropDownList)
            //        {
            //            DropDownList dt = ctrl as DropDownList;
            //            if (dt != null)
            //            {
            //                if (dt.SelectedIndex > 0)
            //                {
            //                    sPropID = dt.ID.ToString();
            //                    sPropDataType = sPropID.Substring(0, 3);
            //                    sPropID = sPropID.Substring(3, (sPropID.Length - 3));
            //                    sPropValue = dt.SelectedValue.ToString();
            //                }
            //            }
            //        }
            //        else if (ctrl is CheckBox)
            //        {
            //            CheckBox chk = ctrl as CheckBox;
            //            if (chk != null)
            //            {
            //                sPropID = chk.ID.ToString();
            //                sPropDataType = sPropID.Substring(0, 3);
            //                sPropID = sPropID.Substring(3, (sPropID.Length - 3));
            //                sPropValue = chk.Checked == true ? "1" : "0";
            //            }
            //        }
            //        else if (ctrl is FileUpload)
            //        {
            //            FileUpload att = ctrl as FileUpload;
            //            if (att != null)
            //            {
            //                sPropID = att.ID.ToString();
            //                sPropDataType = sPropID.Substring(0, 3);
            //                sPropID = sPropID.Substring(3, (sPropID.Length - 3));
            //                if (att.HasFile)
            //                    sPropValue = UploadFile(att.PostedFile.FileName);
            //                else
            //                    sPropValue = "";
            //            }
            //        }

            //        if (sPropID.Trim() != string.Empty)
            //        {
            //            XmlNode fnInsert = XmlUtility.AddChildXmlNode(docInsert, rnInsert, "VALUEINSERTDATA", "");
            //            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "Prop_ID", sPropID);
            //            if (sPropDataType.ToUpper().Trim() == "TXT")
            //            {
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", sPropValue);
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "T");
            //            }
            //            else if (sPropDataType.ToUpper().Trim() == "DAT")
            //            {
            //                DateTime dt = DateTime.Now;
            //                if (!DateTime.TryParse(sPropValue, out dt))
            //                    dt = DateTime.Now;

            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", dt.ToString("yyyy-MM-dd"));
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "D");
            //            }
            //            else if (sPropDataType.ToUpper().Trim() == "NUM")
            //            {
            //                float fNum = 0;
            //                if (!float.TryParse(sPropValue, out fNum))
            //                    fNum = 0;

            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", fNum.ToString());
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "N");
            //            }
            //            else if (sPropDataType.ToUpper().Trim() == "ATT")
            //            {
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", sPropValue);
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "A");
            //            }
            //            else if (sPropDataType.ToUpper().Trim() == "CHK")
            //            {
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", sPropValue);
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "C");
            //            }
            //            else if (sPropDataType.ToUpper().Trim() == "CMB")
            //            {
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", sPropValue.Trim() == string.Empty ? "" : sPropValue);
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
            //                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "L");
            //            }
            //            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ACTIVE", "1");
            //            InsertCount++;
            //        }
            //    }

            //    if (InsertCount > 0)
            //    {
            //        docInsert.AppendChild(rnInsert);
            //        objCreateRequestBO.xmlPropValueList = docInsert.InnerXml;
            //    }
            //    else
            //        objCreateRequestBO.xmlPropValueList = null;

            //    objBase = CommonFactory.CreatePropertyTransValueObject(objBasePagePL.ObjectCreatorOption);
            //    objCreateRequestBO = objBase.InsertOrUpdatePropertyTransValue(objCreateRequestBO);
            //}
            //catch (Exception ex)
            //{
            //    if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
            //        throw;
            //}
            //finally
            //{
            //    if (objCreateRequestBO != null) objCreateRequestBO = null;
            //    if (objBase != null) objBase = null;
            //}
        }
        #endregion

        private string UploadFile(string filePath)
        {
            string Attachment_ID = "";
            return Attachment_ID;
        }

    }
}