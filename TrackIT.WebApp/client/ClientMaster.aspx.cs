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

namespace TrackIT.WebApp.client
{
    public partial class ClientMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid lwdg_clientMasterGrid;
        #endregion


        #region Page Load
        /// <summary>
        /// Page Load Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Unit Testing ID - ClientMaster.aspx.cs_1
            System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_1 PageLoad");

            ControlNames();
            lwdg_clientMasterGrid = new WebDataGrid();
            pnl_clientGrid.Controls.Add(lwdg_clientMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_clientMasterGrid);
          
            if (!IsPostBack)
            {
                //Unit Testing ID - ClientMaster.aspx.cs_2
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_2 PageLoad IsPostBack");

                //Assign all dropdown data Client Location Drop Down
                DataSet lds_ClientLocation = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='CON' and cp.Active = 1 ORDER BY parameter_name");
                if (lds_ClientLocation.Tables[0].Rows.Count > 0)
                {
                    //Unit Testing ID - ClientMaster.aspx.cs_3
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_3 Client Location dataset count" + lds_ClientLocation.Tables[0].Rows.Count);

                    ddlbillCountry.Items.Clear();
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                    ddlbillCountry.DataSource = lds_ClientLocation;
                    ddlbillCountry.DataTextField = "TextValue";
                    ddlbillCountry.DataValueField = "Value";
                    ddlbillCountry.DataBind();
                    ddlbillCountry.Items.Insert(0, li);
                    //Unit Testing ID - ClientMaster.aspx.cs_4
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_4 Client Location dataset count" + lds_ClientLocation.Tables[0].Rows.Count);

                    ddladdresscountry.Items.Clear();
                    ddladdresscountry.DataSource = lds_ClientLocation;
                    ddladdresscountry.DataTextField = "TextValue";
                    ddladdresscountry.DataValueField = "Value";
                    ddladdresscountry.DataBind();
                    ddladdresscountry.Items.Insert(0, li);
                }
             
                ClearControls();
            }
            GetClientDetails();
            if (!string.IsNullOrEmpty(hdnClientID.Value) && hdnpop.Value == "1")
            {
                //Edit client Details
                //Unit Testing ID - ClientMaster.aspx.cs_5
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_5 Edit Client Details popId and Unique ID" + hdnClientID.Value + hdnpop.Value);
                Int64? lint_Clientid = Convert.ToInt64(hdnClientID.Value.ToString());
                btnSave.Visible = bitEdit;
                EditClientDetails(lint_Clientid);
                hdnpop.Value = string.Empty;
                mpe_clientPopup.Show();
                
            }
        }
        #endregion

        #region Post Back Events
        
        #region Save Button Click
        /// <summary>
        /// Save Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected void btnSave_Click(object sender, EventArgs e)
        {    // Client Code Unique validations
            DataSet lds_Result;
            if (string.IsNullOrEmpty(hdnClientID.Value))
            {
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select client_code from prj_clients where  client_code ='" + txtclientCode.Text + "'");
                if (lds_Result.Tables[0].Rows.Count > 0)
                {
                    reqvclientcodeUNQ.ErrorMessage = RollupText("Client", "reqvUnqclientcode");
                    ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + RollupText("Client", "reqvUnqclientcode") + "')</script>", false);
                    mpe_clientPopup.Show();
                    reqvclientcodeUNQ.Enabled = true;
                    reqvclientcodeUNQ.Visible = true;
                    // Unit Testing ID - ClientMaster.aspx.cs_15
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_15 Client Code   Unique check");
                }
                else
                {
                    //Unit Testing ID - ClientMaster.aspx.cs_7
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_7 validate Page and Insert/Update Client" + Page.IsValid);
                    InsertorUpdateClientDetails();
                    //Unit Testing ID - ClientMaster.aspx.cs_8
                     System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_8 Pagevalidation Fails" + Page.IsValid);

                }
            }
            else
            {
                //Unit Testing ID - ClientMaster.aspx.cs_7
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_7 validate Page and Insert/Update User" + Page.IsValid);
                InsertorUpdateClientDetails();
                //Unit Testing ID - ClientMaster.aspx.cs_8
                //System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_8 Pagevalidation Fails" + Page.IsValid);

            }

            //Unit Testing ID - ClientMaster.aspx.cs_6
            System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_6 SaveButtonClick");

        }
        #endregion

        #region initializerow for user grid
        /// <summary>
        /// initialize Each row in the grid and Choose columns to display
        /// </summary>
        private void Lwdg_clientMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            //Unit Testing ID - ClientMaster.aspx.cs_18
            System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_18 intialize grid row " + e.Row.Index);
            if (e.Row.Index == 0)
            {
                //Unit Testing ID - ClientMaster.aspx.cs_19
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_19 row Index" + e.Row.Items.Count);
                e.Row.Items.FindItemByKey("client_key").Column.Hidden = true;
                e.Row.Items.FindItemByKey("client_code").Column.Header.Text = RollupText("client", "gridclientscode");
                e.Row.Items.FindItemByKey("client_name").Column.Header.Text = RollupText("client", "gridclientsname");
                e.Row.Items.FindItemByKey("is_active").Column.Header.Text = RollupText("client", "gridclientsactive");
            }
            if (!IsPostBack)
            {
                //Grid Postback to onRowSorting and Grid Filtering
                for (int lint_i = 0; lint_i < e.Row.Items.Count; lint_i++)
                {
                    if (e.Row.Items[lint_i].Column.Type.FullName.ToString().Equals("System.String") && !string.IsNullOrEmpty(e.Row.Items[lint_i].Column.Key))
                    {
                        //Unit Testing ID - ClientMaster.aspx.cs_20
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_20 " + e.Row.Items[lint_i].Column.Key);
                        ColumnFilter filter = new ColumnFilter();
                        filter.ColumnKey = e.Row.Items[lint_i].Column.Key;
                        filter.Condition = new RuleTextNode(TextFilterRules.Contains, "");
                        lwdg_clientMasterGrid.Behaviors.Filtering.ColumnFilters.Add(filter);
                    }
                }
            }
        }
        #endregion

        #region Clear Button Click
        /// <summary>
        /// Clear page,Controls and popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - ClientMaster.aspx.cs_9
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_9 bUtton Clear_Click");
                EnableDisableControls(true);
                ClearControls();
                mpe_clientPopup.Hide();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region Checkbox event
        protected void chkbillinfosame_CheckedChanged(object sender, EventArgs e)
        {
            mpe_clientPopup.Show();
            if (chkbillinfosame.Checked == true)
            {
                txtbillcity.Text = txtaddresscity.Text;
                txtbilladdress1.Text = txtaddressline1.Text;
                txtbilladdress2.Text = txtaddressline2.Text;
                txtbillstate.Text = txtaddressstate.Text;
                txtbillzip.Text = txtaddresszip.Text;
                ddlbillCountry.SelectedIndex = ddladdresscountry.SelectedIndex;
            }
            else if (chkbillinfosame.Checked == false)
            {
                txtbillcity.Text = "";
                txtbilladdress1.Text = "";
                txtbilladdress2.Text = "";
                txtbillstate.Text = "";
                txtbillzip.Text = "";
                ddlbillCountry.SelectedIndex = 0;
            }
            
        }

        #endregion
        #endregion

        #region user defined functions
       
        #region InsertorUpdateUserDetails
        protected void InsertorUpdateClientDetails()
        {
            try
            {
                //Unit Testing ID - ClientMaster.aspx.cs_10
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_10 InsertUpdateClient Details");
                string lstr_outMessage = string.Empty;
                 string istr_tablename = "prj_clients";
                 bool lbool_type = false;
                if (string.IsNullOrEmpty(hdnClientID.Value))
                {        //Unit Testing ID - ClientMaster.aspx.cs_11
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_11 Insert" + hdnClientID.Value);

                    string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                    {                    
                        {"client_name", txtclientName.Text.Replace("'", "''")},
                        {"client_code",txtclientCode.Text.Replace("'", "''")},
                        {"client_address_1",txtaddressline1.Text.Replace("'", "''")},
                        {"client_address_2",txtaddressline2.Text.Replace("'", "''")},
                        {"client_city",txtaddresscity.Text.Replace("'", "''")},
                        {"client_state",txtaddressstate.Text.Replace("'", "''")},
                        {"client_zip",txtaddresszip.Text.Replace("'", "''")},
                        {"client_country", ddladdresscountry.SelectedValue},                       
                        {"client_billinginfo_same", (chkbillinfosame.Checked ? 1 : 0).ToString()},
                        {"client_contact_name",txtclientcontactname.Text.Replace("'", "''")},
                        {"client_contact_designation",txtclientcontactdesignation.Text.Replace("'", "''")},
                        {"client_bill_address_1",txtbilladdress1.Text.Replace("'", "''")},
                        {"client_bill_address_2",txtbilladdress2.Text.Replace("'", "''")},
                        {"client_bill_city",txtbillcity.Text.Replace("'", "''")},
                        {"client_bill_state",txtbillstate.Text.Replace("'", "''")},
                        {"client_bill_zip",txtbillzip.Text.Replace("'", "''")},
                        {"client_bill_country",ddlbillCountry.SelectedValue},
                        {"is_active", (chkisactive.Checked ? 1 : 0).ToString()},
                        {"Created_By", this.LoggedInUserId },
                        {"Created_Date", DateTime.Now},
                        {"last_modified_By", this.LoggedInUserId },
                        {"last_modified_date", DateTime.Now}
                      }
                           , lbool_type);
                    lstr_outMessage = "SUCCESS";
                }
                else
                {
                    //Unit Testing ID - ClientMaster.aspx.cs_12
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_12 Update Client" + hdnClientID.Value);
                    lbool_type =false;
                    istr_tablename = "prj_clients";
                     string id=ldbh_QueryExecutors.SqlUpdate(istr_tablename,new System.Collections.Generic.Dictionary<string,object>()
                        {
                        {"client_name", txtclientName.Text.Replace("'", "''")},
                        {"client_code",txtclientCode.Text.Replace("'", "''")},
                        {"client_address_1",txtaddressline1.Text.Replace("'", "''")},
                        {"client_address_2",txtaddressline2.Text.Replace("'", "''")},
                        {"client_city",txtaddresscity.Text.Replace("'", "''")},
                        {"client_state",txtaddressstate.Text.Replace("'", "''")},
                        {"client_zip",txtaddresszip.Text.Replace("'", "''")},
                        {"client_country", ddladdresscountry.SelectedValue},                       
                        {"client_billinginfo_same", (chkbillinfosame.Checked ? 1 : 0).ToString()},
                        {"client_contact_name",txtclientcontactname.Text.Replace("'", "''")},
                        {"client_contact_designation",txtclientcontactdesignation.Text.Replace("'", "''")},
                        {"client_bill_address_1",txtbilladdress1.Text.Replace("'", "''")},
                        {"client_bill_address_2",txtbilladdress2.Text.Replace("'", "''")},
                        {"client_bill_city",txtbillcity.Text.Replace("'", "''")},
                        {"client_bill_state",txtbillstate.Text.Replace("'", "''")},
                        {"client_bill_zip",txtbillzip.Text.Replace("'", "''")},
                        {"client_bill_country",ddlbillCountry.SelectedValue},
                        {"is_active", (chkisactive.Checked ? 1 : 0).ToString()}, 
                        {"last_modified_By", this.LoggedInUserId },
                        {"last_modified_date", DateTime.Now}

                        },
                         new System.Collections.Generic.Dictionary<string, object>()
                     {
                         {"client_key", hdnClientID.Value},
                     },
                     lbool_type);
                     lstr_outMessage = "SUCCESS";
                }
                if (lstr_outMessage.Contains("SUCCESS"))
                {
                    //Unit Testing ID - ClientMaster.aspx.cs_13
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_13 success Measage" + lstr_outMessage);
                    string[] sBUID = lstr_outMessage.Split('^');
                    GetClientDetails();
                    SaveMessage();
                    ClearControls();
                    mpe_clientPopup.Hide();
                    return;
                }
                else
                {
                    //Unit Testing ID - ClientMaster.aspx.cs_14
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_14 ErrorMessage");
                    Response.Redirect("~/Setup/ClientMaster.aspx", false);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region GetClientDetails
        /// <summary>
        /// Get Client Details
        /// </summary>
        private void GetClientDetails()
        {
            try
            {
                //Unit Testing ID - ClientMaster.aspx.cs_16
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_16 GetClientDetails");
                lwdg_clientMasterGrid.InitializeRow += Lwdg_clientMasterGrid_InitializeRow;
                lwdg_clientMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Edit";
                td.Width = 30;
                lwdg_clientMasterGrid.Columns.Add(td);
                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select client_key,client_code,client_name,is_active from prj_clients");
                //Unit Testing ID - ClientMaster.aspx.cs_17
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_17 lds_Result set" + lds_Result.Tables.Count);
                if (lds_Result != null)
                {
                    if (lds_Result.Tables[0].Rows.Count > 0)
                    {
                        ViewState["export"] = (DataTable)lds_Result.Tables[0];
                        lwdg_clientMasterGrid.DataSource = lds_Result.Tables[0];
                        lwdg_clientMasterGrid.DataBind();
                        DataColumn[] keyColumns = new DataColumn[1];
                        DataTable ldt_dt = lds_Result.Tables[0];
                        lwdg_clientMasterGrid.DataKeyFields = "client_key";
                        keyColumns[0] = ldt_dt.Columns["client_key"];
                        ldt_dt.PrimaryKey = keyColumns;
                        lwdg_clientMasterGrid.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion
        
        #region EditClientDetails on Edit
        /// <summary>
        /// Get Client Details on Edit
        /// </summary>
        private void EditClientDetails(Int64? aint_ClientKey)
        {
            Int64? lint_ClientKey = aint_ClientKey;

            try
            {
                //Unit Testing ID - ClientMaster.aspx.cs_21
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_21 GetClient Details Edit" + aint_ClientKey);
                DataSet lds_clientdetail = ldbh_QueryExecutors.ExecuteDataSet("select * from prj_clients pc where pc.client_key='" + lint_ClientKey + "'");
                if (lds_clientdetail.Tables[0].Rows.Count > 0)
                {
                    if (lds_clientdetail.Tables[0].Rows[0]["client_country"] != null)
                    {
                        if (ddladdresscountry.Items.FindByValue((lds_clientdetail.Tables[0].Rows[0]["client_country"]).ToString().ToLower()) != null)
                        {
                            ddladdresscountry.SelectedValue = lds_clientdetail.Tables[0].Rows[0]["client_country"].ToString();
                        }
                        else
                        {
                            ddladdresscountry.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ddladdresscountry.SelectedIndex = 0;
                    }

                    if (lds_clientdetail.Tables[0].Rows[0]["client_bill_country"] != null)
                    {
                        if (ddlbillCountry.Items.FindByValue((lds_clientdetail.Tables[0].Rows[0]["client_bill_country"]).ToString().ToLower()) != null)
                        {
                            ddlbillCountry.SelectedValue = lds_clientdetail.Tables[0].Rows[0]["client_bill_country"].ToString();
                        }
                        else
                        {
                            ddlbillCountry.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        ddlbillCountry.SelectedIndex = 0;
                    }


                    hdnClientID.Value = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_key"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_key"]).Trim() : string.Empty;
                    txtclientName.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_name"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_name"]).Trim() : string.Empty;
                    txtclientCode.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_code"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_code"]).Trim() : string.Empty;
                    txtaddressline1.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_address_1"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_address_1"]).Trim() : string.Empty;
                    txtaddressline2.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_address_2"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_address_2"]).Trim() : string.Empty;
                    txtaddresscity.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_city"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_city"]).Trim() : string.Empty;
                    txtaddressstate.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_state"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_state"]).Trim() : string.Empty;
                    txtaddresszip.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_zip"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_zip"]).Trim() : string.Empty;
                    chkbillinfosame.Checked = Convert.ToInt32(lds_clientdetail.Tables[0].Rows[0]["client_billinginfo_same"]) == 1 ? true : false;

                    txtclientcontactname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_contact_name"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_contact_name"]).Trim() : string.Empty;
                    txtclientcontactdesignation.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_contact_designation"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_contact_designation"]).Trim() : string.Empty;

                    txtbilladdress1.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_address_1"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_address_1"]).Trim() : string.Empty;
                    txtbilladdress2.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_address_2"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_address_2"]).Trim() : string.Empty;
                    txtbillcity.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_city"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_city"]).Trim() : string.Empty;
                    txtbillstate.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_state"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_state"]).Trim() : string.Empty;
                    txtbillzip.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_zip"]))) ? Convert.ToString(lds_clientdetail.Tables[0].Rows[0]["client_bill_zip"]).Trim() : string.Empty;
                    chkisactive.Checked = Convert.ToInt32(lds_clientdetail.Tables[0].Rows[0]["is_active"]) == 1 ? true : false;
                    chkisactive.Enabled = true;

                }
                else
                {
                    ClearControls();
                }


            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion
        
        #region EnableDisableControls
        private void EnableDisableControls(bool bValue)
        {
            try
            {
                //Unit Testing ID - ClientMaster.aspx.cs_22
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_22 EnableDiasble Controls");


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
            //Unit Testing ID - ClientMaster.aspx.cs_23
            System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_23 ClearControls");
            txtaddresscity.Text = string.Empty;
            txtaddressline1.Text = string.Empty;
            txtaddressline2.Text = string.Empty;
            txtaddressstate.Text = string.Empty;
            txtaddresszip.Text = string.Empty;
            ddladdresscountry.SelectedIndex = -1;
            ddlbillCountry.SelectedIndex = -1;
            txtbilladdress1.Text = string.Empty;
            txtbilladdress2.Text = string.Empty; ;
            txtbillcity.Text = string.Empty;
            txtbillstate.Text = string.Empty;
            txtbillzip.Text = string.Empty;
            txtclientCode.Text = string.Empty;
            txtclientcontactdesignation.Text = string.Empty;
            txtclientcontactname.Text = string.Empty;
            txtclientName.Text = string.Empty;
            chkbillinfosame.Checked = false;
            chkisactive.Checked = true;
            chkisactive.Enabled = false;
            hdnClientID.Value = string.Empty;

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
                //Unit Testing ID - ClientMaster.aspx.cs_24
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_24 ControlNames");
                lblCreateClient.Text = RollupText("Client", "lblCreateClients");
                lblclientname.Text = RollupText("Client", "lblclientname");
                lblIsactive.Text = RollupText("Client", "lblIsactive");
                lblclientcode.Text = RollupText("Client", "lblclientcode");
                lblbilladdress1.Text = RollupText("Client", "lblbilladdress1");
                lblbilladdress2.Text = RollupText("Client", "lblbilladdress2");
                lblbillcity.Text = RollupText("Client", "lblbillcity");
                lblbillstate.Text = RollupText("Client", "lblbillstate");
                lblbillzip.Text = RollupText("Client", "lblbillzip");
                lblbillcountry.Text = RollupText("Client", "lblbillcountry");
                lblbillinfosame.Text = RollupText("Client", "lblbillinfosame");
                lblclientcontactname.Text = RollupText("Client", "lblclientcontactname");
                lblclientcontactdesignation.Text = RollupText("Client", "lblclientcontactdesignation");
                lbladdresscity.Text = RollupText("Client", "lbladdresscity");
                lbladdresscountry.Text = RollupText("Client", "lbladdresscountry");
                lbladdressline1.Text = RollupText("Client", "lbladdressline1");
                lbladdressline2.Text = RollupText("Client", "lbladdressline2");
                lbladdressstate.Text = RollupText("Client", "lbladdressstate");
                lbladdresszip.Text = RollupText("Client", "lbladdresszip");


                reqvclientcode.ErrorMessage = RollupText("Client", "reqvclientcode");
                reqvclientcodeUNQ.ErrorMessage = RollupText("Client", "reqvclientcodeUNQ");
                reqvtxtclientname.ErrorMessage = RollupText("Client", "reqvtxtclientname");
                reqvtxtclientcontactname.ErrorMessage = RollupText("Client", "reqvtxtclientcontactname");
                reqvtxtclientcontactdesignation.ErrorMessage = RollupText("Client", "reqvtxtclientcontactdesignation");
                reqvtxtbilladdressline1.ErrorMessage = RollupText("Client", "reqvtxtbilladdressline1");
                reqvtxtaddressLine1.ErrorMessage = RollupText("Client", "reqvtxtaddressLine1");
                reqvddlbillcountry.ErrorMessage = RollupText("Client", "reqvddlbillcountry");
                reqvcountry.ErrorMessage = RollupText("Client", "reqvcountry");
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #endregion

        #region  Export to Excel And PDF
        protected void btnExportExcel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Unit Testing ID - ClientMaster.aspx.cs_25
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_25 Export Excel");

                DataTable ldt_ExcelExp = (DataTable)ViewState["export"];
                lwdg_clientMasterGrid.DataSource = ldt_ExcelExp;
                lwdg_clientMasterGrid.DataBind();
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebExcelExporter.Export(lwdg_clientMasterGrid);
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebExcelExporter.Export(this.lwdg_clientMasterGrid);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                //Response.Redirect("~/Error.aspx", false);
            }
        }
        protected void btnExportPDF_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Unit Testing ID - ClientMaster.aspx.cs_26
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ClientMaster.aspx.cs_26 ExportPdf");
                DataTable ldt_PdfExp = (DataTable)ViewState["export"];
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_clientMasterGrid);
                lwdg_clientMasterGrid.DataSource = ldt_PdfExp;
                lwdg_clientMasterGrid.DataBind();
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebPDFExporter.Export(lwdg_clientMasterGrid);
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebPDFExporter.Export(this.lwdg_clientMasterGrid);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                //Response.Redirect("~/Error.aspx", false);
            }

        }
        #endregion

        #region Verify Control Rendereing
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        #endregion
    }
}
                 