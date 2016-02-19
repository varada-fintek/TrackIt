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
            try
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
                    //Unit Testing ID - UserMaster.aspx.cs_2
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_2 PageLoad IsPostBack  ");
                    //Assign all dropdown data Roles Drop Down
                    DataSet lds_cou = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='CON' and cp.Active = 1 ORDER BY parameter_name");
                    if (lds_cou.Tables[0].Rows.Count > 0)
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_3

                        ddlcountry.Items.Clear();
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlcountry.DataSource = lds_cou;
                        ddlcountry.DataTextField = "TextValue";
                        ddlcountry.DataValueField = "Value";
                        ddlcountry.DataBind();
                        ddlcountry.Items.Insert(0, li);
                    }
                    DataSet lds_bilcou = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='CON' and cp.Active = 1 ORDER BY parameter_name");
                    if (lds_bilcou.Tables[0].Rows.Count > 0)
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_3

                        ddlbillcountry.Items.Clear();
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlbillcountry.DataSource = lds_bilcou;
                        ddlbillcountry.DataTextField = "TextValue";
                        ddlbillcountry.DataValueField = "Value";
                        ddlbillcountry.DataBind();
                        ddlbillcountry.Items.Insert(0, li);
                    }
             }
            }
           catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
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
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("UserMaster", "lblListCaption");
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

    
    }


}
