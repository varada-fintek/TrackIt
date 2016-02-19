using System;
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

namespace TrackIT.WebApp.Taxes
{
    public partial class TaxMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid iwdg_TaxMasterGrid;
        WebDataGrid iwdg_TaxDetailsGrid;
        private static string istr_tablename = string.Empty;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {


            ControlNames();
            iwdg_TaxMasterGrid = new WebDataGrid();
            pnl_taxGrid.Controls.Add(iwdg_TaxMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxMasterGrid);

            iwdg_TaxDetailsGrid = new WebDataGrid();
            pnl_taxdetailsGrid.Controls.Add(iwdg_TaxDetailsGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxDetailsGrid);
            GetTaxDetails();



        }

        private void iwdg_TaxDetailsGrid_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("tax_from").Column.Header.Text = RollupText("Taxes", "detailsgridtaxfrom");
                e.Row.Items.FindItemByKey("tax_to").Column.Header.Text = RollupText("Taxes", "detailsgridtaxto");
                e.Row.Items.FindItemByKey("tax_percent").Column.Header.Text = RollupText("Taxes", "detailsgridtaxpercent");
                e.Row.Items.FindItemByKey("tax_type").Column.Header.Text = RollupText("Taxes", "detailsgridtaxtype");
                e.Row.Items.FindItemByKey("tax_applied_on").Column.Header.Text = RollupText("Taxes", "detailsgridtaxappliedon");
            }
        }

        private void iwdg_TaxMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Row.Index == 0)
            {

                e.Row.Items.FindItemByKey("tax_tax_name").Column.Header.Text = RollupText("Taxes", "gridtaxname");
                e.Row.Items.FindItemByKey("tax_tax_code").Column.Header.Text = RollupText("Taxes", "gridtaxcode");
            }
        }

        private void ControlNames()
        {
            lblCreatetaxes.Text = RollupText("Taxes", "lblCreatetaxes");
            lbltaxname.Text = RollupText("Taxes", "lbltaxname");
            lbltaxcode.Text = RollupText("Taxes", "lbltaxcode");
        }

        private void GetTaxDetails()
        {
            try
            {
                iwdg_TaxMasterGrid.InitializeRow += iwdg_TaxMasterGrid_InitializeRow;
                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select tax_tax_name,tax_tax_code from prj_taxes");
                if (lds_Result.Tables[0].Rows.Count > 0)
                {
                    iwdg_TaxMasterGrid.DataSource = lds_Result.Tables[0];
                    iwdg_TaxMasterGrid.DataBind();
                }
                iwdg_TaxDetailsGrid.InitializeRow += iwdg_TaxDetailsGrid_InitializeRow;
                DataSet lds_taxResult;
                lds_taxResult = ldbh_QueryExecutors.ExecuteDataSet("select tax_from,tax_to,tax_percent,tax_type,tax_applied_on from prj_taxes_details");
                if (lds_taxResult.Tables[0].Rows.Count > 0)
                {
                    iwdg_TaxDetailsGrid.DataSource = lds_taxResult.Tables[0];
                    iwdg_TaxDetailsGrid.DataBind();
                }

            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        protected void InsertorUpdateTaxDetails()
        {
            try
            {
                istr_tablename = "prj_taxes";
                Boolean lbool_type = true;
                string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object> 
                {
                    {"tax_tax_name",txttaxname.Text.Replace("'","''")},
                    {"tax_tax_code",txttaxcode.Text.Replace("'","''")},
                }, lbool_type
                );
            }
            catch (Exception ex)
            {

                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertorUpdateTaxDetails();
            mpe_taxPopup.Show();
        }
    }
}