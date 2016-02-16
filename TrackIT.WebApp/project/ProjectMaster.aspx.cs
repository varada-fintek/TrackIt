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
        WebDataGrid iwdg_projectMasterGrid;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlNames();
            lblCreateProjects.Text = RollupText("Projects", "lblCreateProjects");
            
            iwdg_projectMasterGrid = new WebDataGrid();
            pnl_projectGrid.Controls.Add(iwdg_projectMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_projectMasterGrid);

            iwdg_projectMasterGrid.InitializeRow += iwdg_projectMasterGrid_InitializeRow;
            DataSet lds_Result;
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select project_code,project_name,project_kickoff_date,project_owner,is_active from prj_projects");
            if (lds_Result.Tables[0].Rows.Count > 0)
            {
                iwdg_projectMasterGrid.DataSource = lds_Result.Tables[0];
                iwdg_projectMasterGrid.DataBind();
            }
        }
        private void iwdg_projectMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Row.Index == 0)
            {

                e.Row.Items.FindItemByKey("project_code").Column.Header.Text = RollupText("projects", "gridprojectcode");
                e.Row.Items.FindItemByKey("project_name").Column.Header.Text = RollupText("projects", "gridprojectname");
                e.Row.Items.FindItemByKey("project_kickoff_date").Column.Header.Text = RollupText("projects", "gridprojectkickoffdate");
                e.Row.Items.FindItemByKey("project_owner").Column.Header.Text = RollupText("projects", "gridprojectowner");
                e.Row.Items.FindItemByKey("is_active").Column.Header.Text = RollupText("projects", "gridisactive");
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

                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_25 ControlNames");
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("Projects", "lblListCaption");
                lblclientname.Text = RollupText("Projects", "lblclientname");
                lblprojectcode.Text = RollupText("Projects", "lblprojectcode");
                lblprojectname.Text = RollupText("Projects", "lblprojectname");
                reqvprojectcode.ErrorMessage = RollupText("Projects", "reqvprojectcode");
                reqvprojectname.ErrorMessage = RollupText("Projects", "reqvprojectname");

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
        #region Verify Control Rendereing
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        #endregion
    }
}