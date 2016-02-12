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


namespace TrackIT.WebApp.project
{
    public partial class ProjectMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid lwdg_projectsMasterGrid;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            lwdg_projectsMasterGrid = new WebDataGrid();
            pnl_projectsGrid.Controls.Add(lwdg_projectsMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_projectsMasterGrid);
            lblCreateProjects.Text = RollupText("Projects", "lblCreateProjects");

            lwdg_projectsMasterGrid.InitializeRow += Lwdg_projectsMasterGrid_InitializeRow; ;
            
            //Query to Get Landing Page Grid Details
            DataSet lds_Result;
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select project_code,project_name from prj_projects");
            if(lds_Result.Tables[0].Rows.Count>0)
            {
                lwdg_projectsMasterGrid.DataSource = lds_Result.Tables[0];
                lwdg_projectsMasterGrid.DataBind();

            }
        }

        private void Lwdg_projectsMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("project_code").Column.Header.Text = RollupText("Projects", "gridProjectcode");
                e.Row.Items.FindItemByKey("project_name").Column.Header.Text = RollupText("Projects", "gridProjectName");
            }
            //throw new NotImplementedException();
        }
    }
}