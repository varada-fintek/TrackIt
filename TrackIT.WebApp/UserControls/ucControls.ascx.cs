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
    public partial class ucControls : BasePageUserControl //System.Web.UI.UserControl //BasePageUserControl
    {

        #region Declaraction

        PropertyTransValueBO objPropertyTransValueBO;
        IPropertyTransValue objBase;
        string strFileName = string.Empty;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                try
                {
                    getControls();
                }
                catch (Exception ex)
                {
                    ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                    Response.Redirect("~/Error.aspx", false);
                }
            }
        }

        //protected override void CreateChildControls()
        //{
        //    this.EnsureChildControls();
        //    this.CreateControlCollection();
        //    this.RemoveControl();
        //    this.getDynamicControls();
        //    this.ChildControlsCreated = true;
        //}

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

                if (StringFunctions.IsNullOrEmpty(objPropertyTransValueBO)) objPropertyTransValueBO = new PropertyTransValueBO();
                objBase = CommonFactory.CreatePropertyTransValueObject("");

                objPropertyTransValueBO.FIRM_ID = UCFIRM_ID;
                objPropertyTransValueBO.BU_ID = UCBU_ID;
                objPropertyTransValueBO.MODULE_ID = UCMODULE_ID;
                objPropertyTransValueBO.PROP_TRANS_ID = UCPROP_TRANS_ID;
                objPropertyTransValueBO.PROP_ID = UCPROP_ID;
                objPropertyTransValueBO.dtPropList = objBase.GetPropertyTransValue(objPropertyTransValueBO);

                if (objPropertyTransValueBO.dtPropList != null)
                {
                    foreach (DataRow dr in objPropertyTransValueBO.dtPropList.Rows)
                    {
                        phControls.Controls.Add(new LiteralControl("<div class='form-group'>"));

                        Label lb = new Label();
                        lb.CssClass = "control-label col-md-2";
                        lb.ID = "lbl" + dr["PROP_ID"].ToString().Trim();
                        if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                            lb.Text = dr["PROP_CAPTION"].ToString().Trim() + "*";
                        else
                            lb.Text = dr["PROP_CAPTION"].ToString().Trim();

                        phControls.Controls.Add(lb);

                        if (dr["PROP_DATA_TYPE_ID"].ToString().Trim() =="A")
                            phControls.Controls.Add(new LiteralControl("<div class='col-md-5'>"));
                        else
                            phControls.Controls.Add(new LiteralControl("<div class='col-md-3'>"));

                        string sControlType = dr["PROP_DATA_TYPE_ID"].ToString().Trim();
                        switch (sControlType)
                        {
                            case "T":
                                TextBox tb = new TextBox();
                                tb.ID = "txt" + dr["PROP_ID"].ToString().Trim();
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
                                    req.ID = "req" + dr["PROP_ID"].ToString().Trim();
                                    req.CssClass = "validator_msg";
                                    req.ControlToValidate = tb.ID;
                                    req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                    req.SetFocusOnError = true;
                                    req.Text = "*";
                                    req.ErrorMessage = "Please enter " + dr["Prop_Caption"].ToString().Trim();
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
                                tbDate.ID = "dat" + dr["PROP_ID"].ToString().Trim();
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
                                    req.ID = "req" + dr["PROP_ID"].ToString().Trim();
                                    req.CssClass = "validator_msg";
                                    req.ControlToValidate = tbDate.ID;
                                    req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                    req.SetFocusOnError = true;
                                    req.Text = "*";
                                    req.ErrorMessage = "Please select " + dr["Prop_Caption"].ToString().Trim();
                                    req.ValidationGroup = "vgrpSave";
                                    req.EnableViewState = true;
                                    req.InitialValue = "";
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
                                dt.ID = "cmb" + dr["PROP_ID"].ToString().Trim();
                                ListItem li = new ListItem("Select", "");
                                dt.Items.Insert(0, li);

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
                                    req.ID = "req" + dr["PROP_ID"].ToString().Trim();
                                    req.CssClass = "validator_msg";
                                    req.ControlToValidate = dt.ID;
                                    req.Display = System.Web.UI.WebControls.ValidatorDisplay.Dynamic;
                                    req.SetFocusOnError = true;
                                    req.Text = "*";
                                    req.ErrorMessage = "Please select " + dr["Prop_Caption"].ToString().Trim();
                                    req.ValidationGroup = "vgrpSave";
                                    req.InitialValue = "";
                                    req.EnableViewState = true;
                                    phControls.Controls.Add(req);
                                    phControls.Controls.Add(new LiteralControl("</div>"));
                                }

                                break;

                            case "N":
                                TextBox tbNumber = new TextBox();
                                tbNumber.ID = "num" + dr["PROP_ID"].ToString().Trim();
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
                                    req.ID = "req" + dr["PROP_ID"].ToString().Trim();
                                    req.CssClass = "validator_msg";
                                    req.ControlToValidate = tbNumber.ID;
                                    req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                    req.SetFocusOnError = true;
                                    req.Text = "*";
                                    req.ErrorMessage = "Please enter " + dr["Prop_Caption"].ToString().Trim();
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
                                chk.ID = "chk" + dr["PROP_ID"].ToString().Trim();
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
                                //tbAttachment.ID = "att" + dr["PROP_ID"].ToString().Trim();
                                //tbAttachment.TextMode = TextBoxMode.SingleLine;
                                //tbAttachment.MaxLength = 20;
                                //tbAttachment.CssClass = "form-control";
                                //tbAttachment.EnableViewState = true;
                                //if (UCVIEW_MODE == "VIEW") { tbAttachment.Enabled = false; }
                                //phControls.Controls.Add(tbAttachment);
                                //phControls.Controls.Add(new LiteralControl("</div>"));

                                FileUpload tbAttachment = new FileUpload();
                                tbAttachment.ID = "att" + dr["PROP_ID"].ToString().Trim();
                                tbAttachment.CssClass = "form-control";
                                tbAttachment.EnableViewState = true;
                                if (UCVIEW_MODE == "VIEW") { tbAttachment.Enabled = false; }
                                phControls.Controls.Add(tbAttachment);
                                phControls.Controls.Add(new LiteralControl("</div>"));

                                if (dr["Is_Mandatory"].ToString().ToUpper().Trim() == "TRUE")
                                {
                                    phControls.Controls.Add(new LiteralControl("<div class='col-md-1'>"));
                                    RequiredFieldValidator req = new System.Web.UI.WebControls.RequiredFieldValidator();
                                    req.ID = "req" + dr["PROP_ID"].ToString().Trim(); // + iIndex.ToString().Trim();
                                    req.CssClass = "validator_msg";
                                    req.ControlToValidate = tbAttachment.ID;
                                    req.Display = System.Web.UI.WebControls.ValidatorDisplay.Static;
                                    req.SetFocusOnError = true;
                                    req.Text = "*";
                                    req.ErrorMessage = "Please select " + dr["Prop_Caption"].ToString().Trim();
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
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

        private DataTable GetListValues(Guid? guidListID)
        {
            DataTable dt = null;
         //   ListValuesBO objListValuesBO = new ListValuesBO();
           // IListValues objListBase;
            try
            {
              //  objListValuesBO.ListID = guidListID;
              //  objListBase = SetupFactory.CreateListValuesDetailsObject("");
              //  objListValuesBO = objListBase.GetListValuesDetails(objListValuesBO);
                //if (!StringFunctions.IsNullOrEmpty(objListValuesBO))
                //{
                //    if (objListValuesBO.dsResult != null)
                //    {
                //        if (objListValuesBO.dsResult.Tables.Count > 0)
                //        {
                //            if (objListValuesBO.dsResult.Tables[1].Rows.Count > 0)
                //            {
                //                dt = objListValuesBO.dsResult.Tables[2];
                //            }
                //        }
                //    }
                //}
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
            BasePage objBasePagePL = Page as BasePage;
            try
            {
                if (StringFunctions.IsNullOrEmpty(objPropertyTransValueBO)) objPropertyTransValueBO = new PropertyTransValueBO();
                objPropertyTransValueBO.MODULE_ID = chvMODULE_ID;
                objPropertyTransValueBO.MODULE_UNIQUE_ID = guidMODULE_UNIQUE_ID;
                objPropertyTransValueBO.CreatedBy = objBasePagePL.LoggedInUserId;
                objPropertyTransValueBO.xmlPropValueList = null;

                string sPropID = "";
                string sPropValue = "";
                string sPropDataType = "";

                int InsertCount = 0;
                XmlDocument docInsert = new XmlDocument();
                XmlNode rnInsert = XmlUtility.AddChildXmlNode(docInsert, null, "VALUEINSERT", "");

                foreach (Control ctrl in phControls.Controls)
                {
                    sPropID = "";
                    sPropValue = "";
                    sPropDataType = "";
                    if (ctrl is TextBox)
                    {
                        TextBox tb = ctrl as TextBox;
                        if (tb != null)
                        {
                            sPropID = tb.ID.ToString();
                            sPropDataType = sPropID.Substring(0, 3);
                            sPropID = sPropID.Substring(3, (sPropID.Length - 3));
                            sPropValue = tb.Text;
                        }
                    }
                    else if (ctrl is DropDownList)
                    {
                        DropDownList dt = ctrl as DropDownList;
                        if (dt != null)
                        {
                            if (dt.SelectedIndex > 0)
                            {
                                sPropID = dt.ID.ToString();
                                sPropDataType = sPropID.Substring(0, 3);
                                sPropID = sPropID.Substring(3, (sPropID.Length - 3));
                                sPropValue = dt.SelectedValue.ToString();
                            }
                        }
                    }
                    else if (ctrl is CheckBox)
                    {
                        CheckBox chk = ctrl as CheckBox;
                        if (chk != null)
                        {
                            sPropID = chk.ID.ToString();
                            sPropDataType = sPropID.Substring(0, 3);
                            sPropID = sPropID.Substring(3, (sPropID.Length - 3));
                            sPropValue = chk.Checked == true ? "1" : "0";
                        }
                    }
                    else if (ctrl is FileUpload)
                    {
                        FileUpload att = ctrl as FileUpload;
                        if (att != null)
                        {
                            sPropID = att.ID.ToString();
                            sPropDataType = sPropID.Substring(0, 3);
                            sPropID = sPropID.Substring(3, (sPropID.Length - 3));
                            if (att.HasFile)
                                sPropValue = UploadFile(att.PostedFile.FileName);
                            else
                                sPropValue = "";
                        }
                    }

                    if (sPropID.Trim() != string.Empty)
                    {
                        XmlNode fnInsert = XmlUtility.AddChildXmlNode(docInsert, rnInsert, "VALUEINSERTDATA", "");
                        XmlUtility.AddXmlAttribute(docInsert, fnInsert, "Prop_ID", sPropID);
                        if (sPropDataType.ToUpper().Trim() == "TXT")
                        {
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", sPropValue);
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "T");
                        }
                        else if (sPropDataType.ToUpper().Trim() == "DAT")
                        {
                            //DateTime dt = DateTime.Now;
                            //if (!DateTime.TryParse(sPropValue, out dt))
                            //    dt = DateTime.Now;

                            DateTime dt = DateTime.Now;
                            if (DateTime.TryParse(sPropValue, out dt))
                            { 
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", dt.ToString("yyyy-MM-dd"));
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "D");
                            }
                            else
                            {
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "D");
                            }
                        }
                        else if (sPropDataType.ToUpper().Trim() == "NUM")
                        {
                            //float fNum = 0;
                            //if (!float.TryParse(sPropValue, out fNum))
                            //    fNum = 0;

                            float fNum = 0;
                            if (float.TryParse(sPropValue, out fNum))
                            { 
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", fNum.ToString());
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "N");
                            }
                            else 
                            {
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
                                XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "N");
                            }
                        }
                        else if (sPropDataType.ToUpper().Trim() == "ATT")
                        {
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", sPropValue);
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "A");
                        }
                        else if (sPropDataType.ToUpper().Trim() == "CHK")
                        {
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", sPropValue);
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "C");
                        }
                        else if (sPropDataType.ToUpper().Trim() == "CMB")
                        {
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "LIST_VALUE_ID", sPropValue.Trim() == string.Empty ? "" : sPropValue);
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "TEXT_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATE_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "NUMBER_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ATTACHMENT_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "CHECKBOX_VALUE", "");
                            XmlUtility.AddXmlAttribute(docInsert, fnInsert, "DATA_TYPE", "L");
                        }
                        XmlUtility.AddXmlAttribute(docInsert, fnInsert, "ACTIVE", "1");
                        InsertCount++;
                    }
                }

                if (InsertCount > 0)
                {
                    docInsert.AppendChild(rnInsert);
                    objPropertyTransValueBO.xmlPropValueList = docInsert.InnerXml;
                }
                else
                    objPropertyTransValueBO.xmlPropValueList = null;

                objBase = CommonFactory.CreatePropertyTransValueObject(objBasePagePL.ObjectCreatorOption);
                objPropertyTransValueBO = objBase.InsertOrUpdatePropertyTransValue(objPropertyTransValueBO);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                if (objPropertyTransValueBO != null) objPropertyTransValueBO = null;
                if (objBase != null) objBase = null;
            }
        }
        #endregion

        private string UploadFile(string filePath)
        {
            BasePage objBasePagePL = Page as BasePage;
            string Attachment_ID = "";
            try
            {
                string filename = System.IO.Path.GetFileName(filePath);
                string ext = System.IO.Path.GetExtension(filename);
                string contenttype = String.Empty;
                switch (ext)
                {
                    case ".doc":
                        contenttype = "application/vnd.ms-word";
                        break;
                    case ".docx":
                        contenttype = "application/vnd.ms-word";
                        break;
                    case ".xls":
                        contenttype = "application/vnd.ms-excel";
                        break;
                    case ".xlsx":
                        contenttype = "application/vnd.ms-excel";
                        break;
                    case ".jpg":
                        contenttype = "image/jpg";
                        break;
                    case ".png":
                        contenttype = "image/png";
                        break;
                    case ".gif":
                        contenttype = "image/gif";
                        break;
                    case ".pdf":
                        contenttype = "application/pdf";
                        break;
                }
                if (contenttype != String.Empty)
                {
                    System.IO.FileStream fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    System.IO.BinaryReader br = new System.IO.BinaryReader(fs);
                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    PropertyTransValueBO objPropertyTransAttachmentValueBO = new PropertyTransValueBO();
                    objPropertyTransAttachmentValueBO.AttachmentType = contenttype;
                    objPropertyTransAttachmentValueBO.AttachmentName = filename;
                    objPropertyTransAttachmentValueBO.AttachmentValue = bytes;
                    objBase = CommonFactory.CreatePropertyTransValueObject(objBasePagePL.ObjectCreatorOption);
                    objPropertyTransAttachmentValueBO = objBase.InsertOrUpdatePropertyTransValue(objPropertyTransAttachmentValueBO);
                    string sOutput = objPropertyTransAttachmentValueBO.OutMessage;
                    if (sOutput.Contains("SUCCESS^"))
                    {
                        string[] sAttachmentID = sOutput.Split('^');
                        Attachment_ID = (!string.IsNullOrEmpty(sAttachmentID[1])) ? sAttachmentID[1].ToString() : string.Empty;
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
                if (objPropertyTransValueBO != null) objPropertyTransValueBO = null;
                if (objBase != null) objBase = null;
            }
            return Attachment_ID;
        }

    }
}