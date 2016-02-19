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
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select pc.client_name, pp.project_code,pp.project_name,pp.project_kickoff_date,pp.project_owner,pp.is_active from prj_projects pp inner join prj_clients pc on pc.client_key=pp.client_key");
            if (lds_Result.Tables[0].Rows.Count > 0)
            {
                iwdg_projectMasterGrid.DataSource = lds_Result.Tables[0];
                iwdg_projectMasterGrid.DataBind();
            }
            if(!IsPostBack)
            {
                DataSet lds_Client = ldbh_QueryExecutors.ExecuteDataSet("SELECT client_key AS [Value], client_name AS TextValue FROM prj_clients (NOLOCK) WHERE is_active = 1 ORDER BY client_name");
                if (lds_Client.Tables[0].Rows.Count > 0)
                {
                    //Unit Testing ID - UserMaster.aspx.cs_3
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_3 Roles dataset count" + lds_Client.Tables[0].Rows.Count);
                    ddlClients.Items.Clear();
                    System.Web.UI.WebControls.ListItem llstm_li = new System.Web.UI.WebControls.ListItem("Select", "");
                    ddlClients.DataSource = lds_Client;
                    ddlClients.DataTextField = "TextValue";
                    ddlClients.DataValueField = "Value";
                    ddlClients.DataBind();
                    ddlClients.Items.Insert(0, llstm_li);
                }

            }
        }

        private void iwdg_projectMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Row.Index == 0)
            {

                e.Row.Items.FindItemByKey("client_name").Column.Header.Text = RollupText("projects", "gridclientname");
                e.Row.Items.FindItemByKey("project_code").Column.Header.Text = RollupText("projects", "gridprojectcode");
                e.Row.Items.FindItemByKey("project_name").Column.Header.Text = RollupText("projects", "gridprojectname");
                e.Row.Items.FindItemByKey("project_kickoff_date").Column.Header.Text = RollupText("projects", "gridprojectkickoffdate");
                e.Row.Items.FindItemByKey("project_owner").Column.Header.Text = RollupText("projects", "gridprojectowner");
                e.Row.Items.FindItemByKey("is_active").Column.Header.Text = RollupText("projects", "gridisactive");
            }
        }
        private void ControlNames()
        {
            try
            {
                lblclientname.Text = RollupText("Projects", "lblclientname");
                reqvClient.ErrorMessage = RollupText("Projects", "reqvClient");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            mpe_projectPopup.Show();
        }
    }
}