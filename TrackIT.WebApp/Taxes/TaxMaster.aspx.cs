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
    public partial class TaxMaster: BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid iwdg_TaxMasterGrid;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {


            lblCreatetaxes.Text = RollupText("Taxes", "lblCreatetaxes");
            iwdg_TaxMasterGrid = new WebDataGrid();
            pnl_taxGrid.Controls.Add(iwdg_TaxMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxMasterGrid);

            iwdg_TaxMasterGrid.InitializeRow += iwdg_TaxMasterGrid_InitializeRow;
            DataSet lds_Result;
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select tax_tax_name,tax_tax_code from prj_taxes");
            if(lds_Result.Tables[0].Rows.Count > 0)
            {
                iwdg_TaxMasterGrid.DataSource = lds_Result.Tables[0];
                iwdg_TaxMasterGrid.DataBind();
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
    }
}