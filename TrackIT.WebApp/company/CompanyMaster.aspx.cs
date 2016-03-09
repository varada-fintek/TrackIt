using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;
using Infragistics.Web.UI.GridControls;
using Infragistics.Web.UI;
using Infragistics.Documents.Excel;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text;
using TrackIT.WebApp.Common;
using TrackIT.WebApp.TrackITEnum;
using TrackIT.Common;
namespace TrackIT.WebApp.company
{
    public partial class CompanyMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid lwdg_companyMasterGrid;
        #endregion

        #region events
        #region Pageload
        /// <summary>
        /// Page Load Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Unit Testing ID - CompanyMaster.aspx.cs_1
            System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_1 PageLoad");
            ControlNames();
            lwdg_companyMasterGrid = new WebDataGrid();
            pnl_companyGrid.Controls.Add(lwdg_companyMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_companyMasterGrid);

           
            if (!IsPostBack)
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_2
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_2 PageLoad IsPostBack  ");
                //Assign all dropdown data Roles Drop Down
                DataSet lds_country = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='CON' and cp.Active = 1 ORDER BY parameter_name");
                if (lds_country.Tables[0].Rows.Count > 0)
                {
                    //Unit Testing ID - CompanyMaster.aspx.cs_3
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_3 Roles dataset count" + lds_country.Tables[0].Rows.Count);
                    ddlcountry.Items.Clear();
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                    ddlcountry.DataSource = lds_country;
                    ddlcountry.DataTextField = "TextValue";
                    ddlcountry.DataValueField = "Value";
                    ddlcountry.DataBind();
                    ddlcountry.Items.Insert(0, li);

                    //Unit Testing ID - CompanyMaster.aspx.cs_4
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_4 Roles dataset count" + lds_country.Tables[0].Rows.Count);
                    ddlbillcountry.DataSource = lds_country;
                    ddlbillcountry.DataTextField = "TextValue";
                    ddlbillcountry.DataValueField = "Value";
                    ddlbillcountry.DataBind();
                    ddlbillcountry.Items.Insert(0, li);
                }
                //GetUserDetails();
                ClearControls();
            }
            GetComapnyDetails();
            if (!string.IsNullOrEmpty(hdnCompID.Value) && hdnpop.Value == "1")
            {
                //Edit User Details
                //Unit Testing ID - CompanyMaster.aspx.cs_5
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_5 Edit User Details popId and Unique ID" + hdnCompID.Value + hdnpop.Value); 
                Int64? lint_Compid = Convert.ToInt64(hdnCompID.Value.ToString());
                btnSave.Visible = bitEdit;
                EditCompanyDetails(lint_Compid);
                hdnpop.Value = string.Empty;
                // System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                mpe_CompanyPopup.Show();
            }

        }
        #endregion

        #region initialize row event 
        /// <summary>
        /// initialize Each row in the grid and Choose columns to display
        /// </summary>
        private void Lwdg_companyMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Index == 0)
                //Unit Testing ID - CompanyMaster.aspx.cs_6
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_6 row Index" + e.Row.Items.Count);
            {
                e.Row.Items.FindItemByKey("company_key").Column.Hidden = false;
                //e.Row.Items.FindItemByKey("company_name").Column.Header.Text = RollupText("CompanyMaster", "gridcompanyname");
                //e.Row.Items.FindItemByKey("company_name").Column.CssClass = "Dataalign";
                e.Row.Items.FindItemByKey("company_name").Column.Header.Text = RollupText("Companies", "lblcompanyname");
                e.Row.Items.FindItemByKey("company_code").Column.Header.Text = RollupText("Companies", "lblcompanycode");
                e.Row.Items.FindItemByKey("is_active").Column.Header.Text = RollupText("Companies", "lblactive");

            }
        }
        #endregion

        #region btn save click
        /// <summary>
        /// Save Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertorUpdateUserDetails();
            }
        #endregion

        #region btn clear
        /// <summary>
        /// Clear page,Controls and popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
             //Unit Testing ID - CompanyMaster.aspx.cs_7
            System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_7 bUtton Clear_Click");
            //GetCompanyDetails();
            ClearControls();
            mpe_CompanyPopup.Hide();
        }
        #endregion

        #region Checked change event bill info same as address
        protected void chksameinfo_CheckedChanged(object sender, EventArgs e)
        {
            if (chksameinfo.Checked == true)
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_8
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_8 chksameinfo.Checked");
                txtbilladdressline1.Text = txtaddressline1.Text;
                txtbilladdressline2.Text = txtaddressline2.Text;
                txtbillcity.Text = txtcity.Text;
                txtbillstate.Text = txtstate.Text;
                txtbillzip.Text = txtzip.Text;
                ddlbillcountry.SelectedIndex = ddlcountry.SelectedIndex;
            }
            else
            {
                txtbilladdressline1.Text = string.Empty;
                txtbilladdressline2.Text = string.Empty;
                txtbillcity.Text = string.Empty;
                txtbillstate.Text = string.Empty;
                txtbillzip.Text = string.Empty;
                ddlbillcountry.SelectedIndex = -1;
            }
            mpe_CompanyPopup.Show();
        }
        #endregion

        #endregion

        #region User defined functions

        #region Insert or update comapny details
        protected void InsertorUpdateUserDetails()
        {
            try
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_9
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_9 InsertUpdateUser Details");
                string istr_tablename, lstr_outMessage = string.Empty;
                bool lbool_type = false;
                // hdnUserID to check weather Insert / Update.
                if (string.IsNullOrEmpty(hdnCompID.Value))
                {
                    //Unit Testing ID - CompanyMaster.aspx.cs_10
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_10 Insert" + hdnCompID.Value);
                    istr_tablename = "bil_companies";
                    lstr_outMessage = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                    {
                        // {"CompID", lstr_user_id},
                        {"company_name", txtcompanyname.Text.Replace("'", "''")},
                        {"company_code", txtcompanycode.Text.Replace("'", "''")},
                        {"is_active", (chkactive.Checked ? 1 : 0).ToString()},
                        {"company_address_1", txtaddressline1.Text.Replace("'", "''")},
                        {"company_address_2", txtaddressline2.Text.Replace("'", "''")},
                        {"company_city", txtcity.Text.Replace("'", "''")},
                        {"company_state",   txtstate.Text.Replace("'", "''") },
                        {"company_zip",   txtzip.Text.Replace("'", "''")},
                        {"company_country",  ddlcountry.SelectedValue},
                        {"company_billinginfo_same", (chksameinfo.Checked ? 1 : 0).ToString()},

                        {"company_contact_name", txtname.Text.Replace("'", "''")},
                        {"company_contact_designation", txtdesigination.Text.Replace("'", "''")},
                        {"company_bill_address_1", txtbilladdressline1.Text.Replace("'", "''")},
                        {"company_bill_address_2", txtbilladdressline2.Text.Replace("'", "''")},
                        {"company_bill_city", txtbillcity.Text.Replace("'", "''")},
                        {"company_bill_state", txtbillstate.Text.Replace("'", "''")},
                        {"company_bill_zip", txtbillzip.Text.Replace("'", "''")},
                        {"company_bill_country", ddlbillcountry.SelectedValue},
                          {"Created_By", this.LoggedInUserId },
                        {"Created_Date", DateTime.Now},
                        {"last_modified_By", this.LoggedInUserId },
                        {"last_modified_date", DateTime.Now}

                      }
                           , lbool_type);
                }
                // Update Company Details.
                else
                {
                    //Unit Testing ID - CompanyMaster.aspx.cs_11
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_11 Update User" + hdnCompID.Value);
                    lbool_type = false;
                    istr_tablename = "bil_companies";
                    lstr_outMessage = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                {
                        {"company_name", txtcompanyname.Text.Replace("'", "''")},
                        {"company_code", txtcompanycode.Text.Replace("'", "''")},
                        {"is_active", (chkactive.Checked ? 1 : 0).ToString()},
                        {"company_address_1", txtaddressline1.Text.Replace("'", "''")},
                        {"company_address_2", txtaddressline2.Text.Replace("'", "''")},
                        {"company_city",  txtcity.Text.Replace("'", "''")},
                        {"company_state", txtstate.Text.Replace("'", "''")},
                        {"company_zip", txtzip.Text.Replace("'", "''")  },
                        {"company_country", ddlcountry.SelectedValue},
                        {"company_billinginfo_same", (chksameinfo.Checked ? 1 : 0).ToString()},

                        {"company_contact_name", txtname.Text.Replace("'", "''")},
                        {"company_contact_designation", txtdesigination.Text.Replace("'", "''")},
                        {"company_bill_address_1", txtbilladdressline1.Text.Replace("'", "''")},
                        {"company_bill_address_2", txtbilladdressline2.Text.Replace("'", "''")},
                        {"company_bill_city", txtbillcity.Text.Replace("'", "''")},
                        {"company_bill_state", txtbillstate.Text.Replace("'", "''")},
                        {"company_bill_zip", txtbillzip.Text.Replace("'", "''")},
                        {"company_bill_country", ddlbillcountry.SelectedValue},
                        {"last_modified_By", this.LoggedInUserId },
                        {"last_modified_date", DateTime.Now}
                     },
                         new System.Collections.Generic.Dictionary<string, object>()
                     {
                         {"company_key", hdnCompID.Value},
                     },
                         lbool_type);

                }
                //Sucess Message After Insert/Update
                if (lstr_outMessage.Contains("SUCCESS"))
                {
                    //Unit Testing ID - CompanyMaster.aspx.cs_12
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_12 success Measage" + lstr_outMessage);
                    SaveMessage();
                    mpe_CompanyPopup.Hide();
                    GetComapnyDetails();
                    ClearControls();
                    return;
                }
                else
                {
                    //Unit Testing ID - CompanyMaster.aspx.cs_13
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_13 ErrorMessage");
                    Response.Redirect("~/Setup/UserMaster.aspx", false);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region Get Company details
        /// <summary>
        /// Get Company Details
        /// </summary>
        private void GetComapnyDetails()
        {
            try
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_15
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_14 GetCompanyDetails");
                //if (StringFunctions.IsNullOrEmpty(objUserBO)) objUserBO = new CompanyMasterBo();
                lwdg_companyMasterGrid.InitializeRow += Lwdg_companyMasterGrid_InitializeRow;
                lwdg_companyMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
                lwdg_companyMasterGrid.Columns.Add(td);
                //Query to Get Landing Page Grid Details
                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select company_key,company_name,company_code,is_active from bil_companies");
                lwdg_companyMasterGrid.Visible = false;
                if (lds_Result != null)
                {
                    if (lds_Result.Tables.Count > 0)
                    {
                        //Unit Testing ID - CompanyMaster.aspx.cs_16
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_15 lds_Result set" + lds_Result.Tables.Count);
                        if (lds_Result.Tables[0].Rows.Count > 0)
                        {

                            ViewState["export"] = (DataTable)lds_Result.Tables[0];
                            lwdg_companyMasterGrid.DataSource = lds_Result.Tables[0];
                            lwdg_companyMasterGrid.DataBind();
                            lwdg_companyMasterGrid.Visible = true;

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
                // if (objUserBO != null) objUserBO = null;
            }
        }
        #endregion

        #region EditCompanyDetails on Edit
        /// <summary>
        /// Get Company Details on Edit
        /// </summary>
        private void EditCompanyDetails(Int64? aint_CompID)
        {
            try
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_17
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_16 GetCompany Details Edit" + aint_CompID);
                // if (StringFunctions.IsNullOrEmpty(objUserBO)) objUserBO = new UserMasterBo();
                Int64? lint_UserID = aint_CompID;
               //Fetch Single Record from table and assign to Edit
                DataSet lds_companydetails = ldbh_QueryExecutors.ExecuteDataSet("select * from bil_companies bc where bc.company_key='" + aint_CompID + "'");
                if (lds_companydetails.Tables[0].Rows.Count > 0)
                {
                    if (lds_companydetails.Tables[0].Rows[0]["company_country"] != null)
                    {
                        if (ddlcountry.Items.FindByValue((lds_companydetails.Tables[0].Rows[0]["company_country"]).ToString().ToLower()) != null)
                        {
                            ddlcountry.SelectedValue = lds_companydetails.Tables[0].Rows[0]["company_country"].ToString();
                        }
                        else
                            ddlcountry.SelectedIndex = 0;
                    }
                    else
                        ddlcountry.SelectedIndex = 0;

                    if (lds_companydetails.Tables[0].Rows[0]["company_bill_country"] != null)
                    {
                        if (ddlbillcountry.Items.FindByValue((lds_companydetails.Tables[0].Rows[0]["company_bill_country"]).ToString().ToLower()) != null)
                        {
                            ddlbillcountry.SelectedValue = lds_companydetails.Tables[0].Rows[0]["company_bill_country"].ToString();
                        }
                        else
                            ddlbillcountry.SelectedIndex = 0;
                    }
                    else
                        ddlbillcountry.SelectedIndex = 0;

                    hdnCompID.Value = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_key"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_key"]).Trim() : string.Empty;
                    txtcompanycode.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_code"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_code"]).Trim() : string.Empty;
                    txtcompanyname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_name"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_name"]).Trim() : string.Empty;
                    txtname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_contact_name"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_contact_name"]).Trim() : string.Empty;
                    txtdesigination.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_contact_designation"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_contact_designation"]).Trim() : string.Empty;
                    txtaddressline1.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_address_1"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_address_1"]).Trim() : string.Empty;
                    txtaddressline2.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_address_2"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_address_2"]).Trim() : string.Empty;
                    txtcity.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_city"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_city"]).Trim() : string.Empty;
                    txtstate.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_state"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_state"]).Trim() : string.Empty;
                    txtzip.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_zip"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_zip"]).Trim() : string.Empty;
                    txtbilladdressline1.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_address_1"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_address_1"]).Trim() : string.Empty;
                    txtbilladdressline2.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_address_2"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_address_2"]).Trim() : string.Empty;
                    txtbillcity.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_city"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_city"]).Trim() : string.Empty;
                    txtbillstate.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_state"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_state"]).Trim() : string.Empty;
                    txtbillzip.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_zip"]))) ? Convert.ToString(lds_companydetails.Tables[0].Rows[0]["company_bill_zip"]).Trim() : string.Empty;
                    chksameinfo.Checked = Convert.ToInt32(lds_companydetails.Tables[0].Rows[0]["company_billinginfo_same"]) == 1 ? true : false;
                    chkactive.Checked = Convert.ToInt32(lds_companydetails.Tables[0].Rows[0]["is_active"]) == 1 ? true : false;
                    chkactive.Enabled = true;

                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                // if (objUserBO != null) objUserBO = null;
            }
        }
        #endregion


        #region ControlNames
        /// <summary>
        /// ControlNames Assign Values to label and Validators
        /// </summary>
        private void ControlNames()
        {
            try
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_18
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_17 ControlNames");
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("Companies", "lblListCaption");
                lblCreateCompanies.Text = RollupText("Companies", "lblCreateCompanies");
                lblcompanyname.Text = RollupText("Companies", "lblcompanyname");
                lblcompanycode.Text = RollupText("Companies", "lblcompanycode");
                lblactive.Text = RollupText("Companies", "lblactive");
                lbladdressline1.Text = RollupText("Companies", "lbladdressline1");
                lbladdressline2.Text = RollupText("Companies", "lbladdressline2");
                lblcity.Text = RollupText("Companies", "lblcity");
                lblstate.Text = RollupText("Companies", "lblstate");
                lblzip.Text = RollupText("Companies", "lblzip");
                lblcountry.Text = RollupText("Companies", "lblcountry");
                lblname.Text = RollupText("Companies", "lblname");
                lbldesigination.Text = RollupText("Companies", "lbldesigination");
                lblbilladdressline1.Text = RollupText("Companies", "lblbilladdressline1");
                lblbilladdressline2.Text = RollupText("Companies", "lblbilladdressline2");
                lblbillcity.Text = RollupText("Companies", "lblbillcity");
                lblbillstate.Text = RollupText("Companies", "lblbillstate");
                lblbillzip.Text = RollupText("Companies", "lblbillzip");
                lblbillcountry.Text = RollupText("Companies", "lblbillcountry");
                lblactive.Text = RollupText("Companies", "lblactive");
                lblsameinfo.Text = RollupText("Companies", "lblsameinfo");

                btnSave.Text = RollupText("Common", "btnSave");

                reqvtxtcompanyname.ErrorMessage = RollupText("Companies", "reqvtxtcompanyname");
                reqvcompanycode.ErrorMessage = RollupText("Companies", "reqvcompanycode");
                reqvtxtcompanyname.ErrorMessage = RollupText("Companies", "reqvtxtcompanyname");
                reqvtxtaddressline1.ErrorMessage = RollupText("Companies", "reqvtxtaddressline1");
                reqvtxtname.ErrorMessage = RollupText("Companies", "reqvtxtname");
                reqvtxtdesigination.ErrorMessage = RollupText("Companies", "reqvtxtdesigination");
                reqvtxtbilladdressline1.ErrorMessage = RollupText("Companies", "reqvtxtbilladdressline1");
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
        #region ClearControls
        /// <summary>
        /// Clear Each Control in the Screen on PageLoad
        /// </summary>
        private void ClearControls()
        {
            try
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_19
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_18 ClearControls");
                hdnpop.Value = string.Empty;
                txtcompanycode.Text = string.Empty;
                txtcompanyname.Text = string.Empty;
                txtname.Text = string.Empty;
                txtdesigination.Text = string.Empty;
                txtaddressline1.Text = string.Empty;
                txtaddressline2.Text = string.Empty;
                txtbilladdressline1.Text = string.Empty;
                txtbilladdressline1.Text = string.Empty;
                txtzip.Text = string.Empty;
                txtbillzip.Text = string.Empty;
                txtcity.Text = string.Empty;
                txtbillcity.Text = string.Empty;
                txtbillstate.Text = string.Empty;
                txtstate.Text = string.Empty;
                chksameinfo.Checked = false;
                
                chkactive.Checked = true;
                chkactive.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #endregion

        #region Export to Excel And PDF
        /// <summary>
        /// Button Export Excel click Event
        /// </summary>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_20
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_19 Export Excel");
                DataTable ldt_ExcelExp = (DataTable)ViewState["export"];
                lwdg_companyMasterGrid.DataSource = ldt_ExcelExp;
                lwdg_companyMasterGrid.DataBind();
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebExcelExporter.Export(lwdg_companyMasterGrid);
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebExcelExporter.Export(this.lwdg_companyMasterGrid);

            }
            catch (Exception ex)
            {
               // ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                //Response.Redirect("~/Error.aspx", false);
            }
        }

        /// <summary>
        /// Export to pdf button click Event
        /// </summary>
        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - CompanyMaster.aspx.cs_21
                System.Diagnostics.Debug.WriteLine("Unit testing ID - CompanyMaster.aspx.cs_20 ExportPdf");
                DataTable ldt_PdfExp = (DataTable)ViewState["export"];
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_companyMasterGrid);
                lwdg_companyMasterGrid.DataSource = ldt_PdfExp;
                lwdg_companyMasterGrid.DataBind();
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebPDFExporter.Export(lwdg_companyMasterGrid);
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebPDFExporter.Export(this.lwdg_companyMasterGrid);
            }
            catch (Exception ex)
            {
              // ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                // Response.Redirect("~/Error.aspx", false);
            }
        }


        #endregion
    }

}