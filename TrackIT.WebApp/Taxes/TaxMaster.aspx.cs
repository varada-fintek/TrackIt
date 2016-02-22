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
            reqvtaxcode.ErrorMessage = RollupText("Taxes", "reqvtaxcode");
            reqvtxttaxname.ErrorMessage = RollupText("Taxes", "reqvtxttaxname");
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


                // Enable cell editing
                this.iwdg_TaxDetailsGrid.Behaviors.CreateBehavior<EditingCore>();
                this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CreateBehavior<CellEditing>();

                // Create an editor provider
                DatePickerProvider taxfromdate = new DatePickerProvider();
                taxfromdate.ID = "taxfromdateID";

                // Add to collection
                this.iwdg_TaxDetailsGrid.EditorProviders.Add(taxfromdate);

                // Create a column setting to use the editor provider
                EditingColumnSetting columnSetting = new EditingColumnSetting();
                columnSetting.ColumnKey = "tax_from";

                // Assign editor for column to use
                columnSetting.EditorID = taxfromdate.ID;

                // Add column setting
                this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSetting);
                
               

                // Create an editor provider
                DatePickerProvider taxtodate = new DatePickerProvider();
                taxtodate.ID = "taxtodateID";

                // Add to collection
                this.iwdg_TaxDetailsGrid.EditorProviders.Add(taxtodate);

                // Create a column setting to use the editor provider
                EditingColumnSetting columnSettingto = new EditingColumnSetting();
                columnSettingto.ColumnKey = "tax_to";

                // Assign editor for column to use
                columnSettingto.EditorID = taxtodate.ID;

                // Add column setting
                this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSettingto);

                DataSet lds_taxtype = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TYP' and cp.Active = 1 ORDER BY parameter_name");
                
                //Adding dropdown to DetailsGrid
                DropDownProvider ddl_taxtype = new DropDownProvider();
                ddl_taxtype.ID = "taxtypedropdown";
                this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxtype);
                EditingColumnSetting columnSettingtaxtype = new EditingColumnSetting();
                columnSettingtaxtype.ColumnKey = "tax_type";
                columnSettingtaxtype.EditorID = ddl_taxtype.ID;
                ddl_taxtype.EditorControl.ValueField = "Value";
                ddl_taxtype.EditorControl.TextField = "TextValue";
                ddl_taxtype.EditorControl.DataSource = lds_taxtype.Tables[0];

                this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxtype);
                
                this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSettingtaxtype);

                DataSet lds_taxappliedon = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TAO' and cp.Active = 1 ORDER BY parameter_name");

                DropDownProvider ddl_taxappliedon = new DropDownProvider();
                ddl_taxappliedon.ID = "taxappliedondropdown";
                this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxappliedon);
                EditingColumnSetting columnSettingtaxappliedon = new EditingColumnSetting();
                columnSettingtaxappliedon.ColumnKey = "tax_applied_on";
                columnSettingtaxappliedon.EditorID = ddl_taxappliedon.ID;
                ddl_taxappliedon.EditorControl.ValueField = "Value";
                ddl_taxappliedon.EditorControl.TextField = "TextValue";
                ddl_taxappliedon.EditorControl.DataSource = lds_taxappliedon.Tables[0];

                this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxappliedon);

                this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSettingtaxappliedon);
               
               
                
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