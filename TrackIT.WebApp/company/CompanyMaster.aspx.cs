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
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlNames();
            lwdg_companyMasterGrid = new WebDataGrid();
            pnl_companyGrid.Controls.Add(lwdg_companyMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_companyMasterGrid);
            lblCreateCompanies.Text = RollupText("Companies", "lblCreateCompanies");
            lwdg_companyMasterGrid.InitializeRow += Lwdg_companyMasterGrid_InitializeRow; ;
            DataSet lds_Result;
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select company_name,company_code,is_active from bil_companies");
            if (lds_Result.Tables[0].Rows.Count > 0)
            {
                lwdg_companyMasterGrid.DataSource = lds_Result.Tables[0];
                lwdg_companyMasterGrid.DataBind();

            }
            if (!IsPostBack)
            {
                DataSet lds_country = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='CON' and cp.Active = 1 ORDER BY parameter_name");
                if (lds_country.Tables[0].Rows.Count > 0)
                {
                    ddlcountry.Items.Clear();
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                    ddlcountry.DataSource = lds_country;
                    ddlcountry.DataTextField = "TextValue";
                    ddlcountry.DataValueField = "Value";
                    ddlcountry.DataBind();
                    ddlcountry.Items.Insert(0, li);


                    ddlbillcountry.DataSource = lds_country;
                    ddlbillcountry.DataTextField = "TextValue";
                    ddlbillcountry.DataValueField = "Value";
                    ddlbillcountry.DataBind();
                    ddlbillcountry.Items.Insert(0, li);
                }
            }
        }
        private void Lwdg_companyMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("company_name").Column.Header.Text = RollupText("Companies", "lblCreateCompanyName");
                e.Row.Items.FindItemByKey("company_code").Column.Header.Text = RollupText("Companies", "lblCreateCompanyCode");
                e.Row.Items.FindItemByKey("is_active").Column.Header.Text = RollupText("Companies", "lblCreateCompanyActive");

            }
        }
        #region ControlNames
        /// <summary>
        /// ControlNames Assign Values to label and Validators
        /// </summary>
        private void ControlNames()
        {
            try
            {

                //Unit Testing ID - UserMaster.aspx.cs_25
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_25 ControlNames");
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text =
                    RollupText("UserMaster", "lblListCaption");
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
                lblactive.Text = RollupText("Companies", "chkactive");
                lblsameinfo.Text = RollupText("Companies", "chksameinfo");

                sConfirmation = RollupText("Common", "DeleteRecord");
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

        protected void chksameinfo_CheckedChanged(object sender, EventArgs e)
        {
            if (chksameinfo.Checked == true)
            {
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
            mpe_UserPopup.Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertorUpdateUserDetails();

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }
        //  #region InsertorUpdateUserDetails
        protected void InsertorUpdateUserDetails()
        {
            try
            {
                string istr_tablename, lstr_outMessage=string.Empty;
                bool lbool_type = false;

                if (string.IsNullOrEmpty(hdnUserID.Value))
                {

                    istr_tablename = "bil_companies";
                    string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                    {
                      
                        {"company_name", txtcompanyname.Text.Replace("'", "''")},
                        {"company_code", txtcompanycode.Text.Replace("'", "''")},
                        {"is_active", (chkactive.Checked ? 1 : 0).ToString()},
                        {"company_address_1", txtaddressline1.Text.Replace("'", "''")},
                        {"company_address_2", txtaddressline2.Text.Replace("'", "''")},
                        {"company_city", ddlcountry.SelectedValue},
                        {"company_state", txtcity.Text.Replace("'", "''")},
                        {"company_zip", txtstate.Text.Replace("'", "''")},
                        {"company_country", txtzip.Text.Replace("'", "''")},
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
                    lstr_outMessage = "SUCCESS";

                }
                else
                {
                    lbool_type = false;
                    istr_tablename = "bil_companies";
                    string id = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                {
                    {"company_name", txtcompanyname.Text.Replace("'", "''")},
                        {"company_code", txtcompanycode.Text.Replace("'", "''")},
                        {"is_active", (chkactive.Checked ? 1 : 0).ToString()},
                        {"company_address_1", txtaddressline1.Text.Replace("'", "''")},
                        {"company_address_2", txtaddressline2.Text.Replace("'", "''")},
                        {"company_city", ddlcountry.SelectedValue},
                        {"company_state", txtcity.Text.Replace("'", "''")},
                        {"company_zip", txtstate.Text.Replace("'", "''")},
                        {"company_country", txtzip.Text.Replace("'", "''")},
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
                         {"company_key", hdnUserID.Value},
                     },
                         lbool_type);

                    lstr_outMessage = "SUCCESS";
                }

                //Sucess Message After Insert/Update
                if (lstr_outMessage.Contains("SUCCESS"))
                {

                    string[] sBUID = lstr_outMessage.Split('^');
                    //  GetUserDetails();
                    SaveMessage();
                    //  ClearControls();
                    mpe_UserPopup.Hide();
                    return;
                }
                else
                {
                    Response.Redirect("~/Setup/UserMaster.aspx", false);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

    }
}