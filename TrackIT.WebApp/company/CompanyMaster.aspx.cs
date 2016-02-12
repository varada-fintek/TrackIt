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
    }
}
