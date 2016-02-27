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
        WebDataGrid iwdg_panelGrid;
        private static string istr_tablename = string.Empty;
        #endregion

        #region Events

        #region Pageload
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ControlNames();
              
                iwdg_projectMasterGrid = new WebDataGrid();
                iwdg_panelGrid = new WebDataGrid();

                pnl_projectGrid.Controls.Add(iwdg_projectMasterGrid);
              
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_projectMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_panelGrid);
                if (!IsPostBack)
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
                    DataSet lds_projowners = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='OWN' and cp.Active = 1 ORDER BY parameter_name");
                    if (lds_projowners.Tables[0].Rows.Count > 0)
                    {

                        ddlowner.Items.Clear();
                        System.Web.UI.WebControls.ListItem llstm_li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlowner.DataSource = lds_projowners;
                        ddlowner.DataTextField = "TextValue";
                        ddlowner.DataValueField = "Value";
                        ddlowner.DataBind();
                        ddlowner.Items.Insert(0, llstm_li);


                    }
                    clearcontrols();
                }

                GetProjectDetails();
                GetPanelDetails();
                if (!string.IsNullOrEmpty(hdnprjID.Value) && hdnpop.Value == "1")
                {

                    Int64? lint_projid = Convert.ToInt64(hdnprjID.Value.ToString());
                    btnSave.Visible = bitEdit;
                    EditProjectDetails(lint_projid);

                    mpe_projectPopup.Show();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region Project Master Grid initilizerow event
        private void iwdg_projectMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("project_key").Column.Hidden = true;
                e.Row.Items.FindItemByKey("client_name").Column.Header.Text = RollupText("projects", "gridclientname");
                e.Row.Items.FindItemByKey("project_code").Column.Header.Text = RollupText("projects", "gridprojectcode");
                e.Row.Items.FindItemByKey("project_name").Column.Header.Text = RollupText("projects", "gridprojectname");
                e.Row.Items.FindItemByKey("project_kickoff_date").Column.Header.Text = RollupText("projects", "gridprojectkickoffdate");
                e.Row.Items.FindItemByKey("project_owner").Column.Header.Text = RollupText("projects", "gridprojectowner");
                e.Row.Items.FindItemByKey("is_active").Column.Header.Text = RollupText("projects", "gridisactive");
            }
        }
        #endregion

        private void Iwdg_projectphases_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("Value").Column.Hidden = true;
                e.Row.Items.FindItemByKey("TextValue").Column.Header.Text = "Phases";
                e.Row.Items.FindItemByKey("Phases").Column.Header.Text = "Phaseowners";
                e.Row.Items.FindItemByKey("Phaseowners").Column.Header.Text = "Phaseresource";
            }
        }
        #region Button Clear click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearcontrols();
            GetProjectDetails();
            mpe_projectPopup.Hide();
        }
        #endregion

        #region button Save click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet lds_Result;
               
                if (string.IsNullOrEmpty(hdnprjID.Value))
                {
                    lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select project_code from prj_projects where is_active=1 and project_code='" + txtprojectcode.Text + "'");
                    if (lds_Result.Tables[0].Rows.Count > 0)
                    {
                        reqvprojectIdUNQ.ErrorMessage = RollupText("ProjectMaster", "reqvprojectIdUNQ");
                        ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + RollupText("ProjectMaster", "reqvprojectIdUNQ") + "')</script>", false);
                        mpe_projectPopup.Show();
                        reqvprojectIdUNQ.Enabled = true;
                        reqvprojectIdUNQ.Visible = true;
                     
                    }
                    else
                    {
                        InsertorUpdateProjectDetails(); 
                       
                    }
                }
                else
                {
                    InsertorUpdateProjectDetails(); 
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }

           
        }
        #endregion

        #endregion

        #region UserDefined Functions

        #region ControlNames
        private void ControlNames()
        {
            try
            {
                lblCreateProjects.Text = RollupText("Projects", "lblCreateProjects");
                lblactive.Text = RollupText("Projects", "lblactive");
                lblclientname.Text = RollupText("Projects", "lblclientname");
                lblprojectcode.Text = RollupText("Projects", "lblprojectcode");
                lblprojectname.Text = RollupText("Projects", "lblprojectname");
                lblprojectowner.Text = RollupText("Projects", "lblprojectowner");
                lblkickdate.Text = RollupText("Projects", "lblkickdate");

                reqvClient.ErrorMessage = RollupText("Projects", "reqvClient");
                reqvpname.ErrorMessage = RollupText("Projects", "reqvpname");
                reqvcode.ErrorMessage = RollupText("Projects", "reqvcode");
                reqvprojectIdUNQ.ErrorMessage = RollupText("Projects", "reqvprojectIdUNQ");
                reqvowner.ErrorMessage = RollupText("Projects", "reqvowner");
                reqvkickoffdate.ErrorMessage = RollupText("Projects", "reqvkickoffdate");
               
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region ClearControls
        private void clearcontrols()
        {
            try
            {
                txtprojectcode.Text = string.Empty;
                txtprojectname.Text = string.Empty;
                ddlClients.SelectedIndex = -1;
                ddlowner.SelectedIndex = -1;
                igwdp_kickoffdate.Value = string.Empty;
                chkinactive.Checked = true;
                chkinactive.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region Insert or Update Project Details
        protected void InsertorUpdateProjectDetails()
        {
            try
            {
                string astr_project_ID = Convert.ToString(Guid.NewGuid());
                istr_tablename = "prj_projects";
                Boolean lbool_type = true;
                string lstr_outMessage = string.Empty;
                if (string.IsNullOrEmpty(hdnprjID.Value))
                {
                   
                    string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                {
                    {"project_code",txtprojectcode.Text.Replace("'", "''") },
                    {"project_name",txtprojectname.Text.Replace("'", "''") },
                    {"client_key",ddlClients.SelectedValue },
                    { "project_kickoff_date",igwdp_kickoffdate.Value},
                    {"project_owner",ddlowner.SelectedValue },
                    { "is_active",(chkinactive.Checked ? 1 : 0).ToString()},
                    {"created_by",this.LoggedInUserId },
                    {"Created_Date", DateTime.Now},
                    {"last_modified_By", this.LoggedInUserId },
                    {"last_modified_date", DateTime.Now}
                }, lbool_type
                    );
                }
                else
                {

                   
                    lbool_type = false;

                    istr_tablename = "prj_projects";
                    string id = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                {
                    {"project_code",txtprojectcode.Text.Replace("'", "''") },
                    {"project_name",txtprojectname.Text.Replace("'", "''") },
                    {"client_key",ddlClients.SelectedValue },
                    { "project_kickoff_date",igwdp_kickoffdate.Value},
                    {"project_owner",ddlowner.SelectedValue },
                    { "is_active",(chkinactive.Checked ? 1 : 0).ToString()},
                    {"last_modified_By", this.LoggedInUserId },
                    {"last_modified_date", DateTime.Now}
                },
                new System.Collections.Generic.Dictionary<string, object>()
                     {
                         {"project_key", hdnprjID.Value},
                     },
                lbool_type
                   );

                    lstr_outMessage = "SUCCESS";
                }
                if (lstr_outMessage.Contains("SUCCESS"))
                {

                    string[] sBUID = lstr_outMessage.Split('^');
                    GetProjectDetails();
                    SaveMessage();
                   // ClearControls();
                    mpe_projectPopup.Hide();
                    return;
                }
                else
                {
                    Response.Redirect("~/project/ProjectMaster.aspx", false);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region GetProjectDetails
        private void GetProjectDetails()
        {
            try
            {
                iwdg_projectMasterGrid.InitializeRow += iwdg_projectMasterGrid_InitializeRow;
                iwdg_projectMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
                iwdg_projectMasterGrid.Columns.Add(td);

                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select pp.project_key, pc.client_name, pp.project_code,pp.project_name,pp.project_kickoff_date,pp.project_owner,pp.is_active from prj_projects pp inner join prj_clients pc on pc.client_key=pp.client_key");
                if (lds_Result.Tables[0].Rows.Count > 0)
                {
                    iwdg_projectMasterGrid.DataSource = lds_Result.Tables[0];
                    iwdg_projectMasterGrid.DataBind();
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion
        private void GetPanelDetails()
        {
            try
            {
                iwdg_projectphases.InitializeRow += Iwdg_projectphases_InitializeRow;
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
               
                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS[Value], cp.parameter_name AS TextValue,cp.parameter_key as Phases,cp.parameter_key as Phaseowners FROM com_parameters cp(NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code = cp.parameter_type WHERE cpt.parameter_type_code = 'PHA' and cp.Active = 1 ORDER BY parameter_name");
                             
                if (lds_Result.Tables[0].Rows.Count > 0)
                {
                    iwdg_projectphases.DataSource = lds_Result.Tables[0];
                    iwdg_projectphases.DataBind();
                }
                // Enable cell editing
                this.iwdg_projectphases.Behaviors.CreateBehavior<EditingCore>();
                this.iwdg_projectphases.Behaviors.EditingCore.Behaviors.CreateBehavior<CellEditing>();

                DataSet lds_taxtyperesult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='OWN' and cp.Active = 1 ORDER BY parameter_name");
                this.iwdg_projectphases.EditorProviders.Add(ddpPhaseowner);
                EditingColumnSetting phaseownerecolumn = new EditingColumnSetting();
                phaseownerecolumn.ColumnKey = "Phases";
                phaseownerecolumn.EditorID = ddpPhaseowner.ID;
                ddpPhaseowner.EditorControl.ValueField = "Value";
                ddpPhaseowner.EditorControl.TextField = "TextValue";
                ddpPhaseowner.EditorControl.DataSource = lds_taxtyperesult.Tables[0];
                this.iwdg_projectphases.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(phaseownerecolumn);

                DataSet lds_taxappliedonresult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='RES' and cp.Active = 1 ORDER BY parameter_name");
                this.iwdg_projectphases.EditorProviders.Add(ddpPhaseprovider);
                EditingColumnSetting phaseresourcecolumn = new EditingColumnSetting();
                phaseresourcecolumn.ColumnKey = "Phaseowners";
                phaseresourcecolumn.EditorID = ddpPhaseprovider.ID;
                ddpPhaseprovider.EditorControl.ValueField = "Value";
                ddpPhaseprovider.EditorControl.TextField = "TextValue";
                ddpPhaseprovider.EditorControl.DataSource = lds_taxappliedonresult.Tables[0];
                this.iwdg_projectphases.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(phaseresourcecolumn);
                iwdg_projectphases.DataBind();
                
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

       



        #region EditProjectDetails
        private void EditProjectDetails(Int64? aint_ProjectID)
        {
            try
            {
                Int64? lint_UserID = aint_ProjectID;
               
                //Fetch Single Record from table and assign to Edit
                DataSet lds_projectdetail = ldbh_QueryExecutors.ExecuteDataSet("select * from prj_projects pp where pp.project_key='" + aint_ProjectID + "'");

                if (lds_projectdetail.Tables[0].Rows[0]["client_key"] != null)
                {
                    if (ddlClients.Items.FindByValue((lds_projectdetail.Tables[0].Rows[0]["client_key"]).ToString().ToLower()) != null)
                    {
                        ddlClients.SelectedValue = lds_projectdetail.Tables[0].Rows[0]["client_key"].ToString();
                    }
                    else
                        ddlClients.SelectedIndex = 0;
                }
                else
                    ddlClients.SelectedIndex = 0;

                if (lds_projectdetail.Tables[0].Rows[0]["project_owner"] != null)
                {
                    if (ddlowner.Items.FindByValue((lds_projectdetail.Tables[0].Rows[0]["project_owner"]).ToString().ToLower()) != null)
                    {
                        ddlowner.SelectedValue = lds_projectdetail.Tables[0].Rows[0]["project_owner"].ToString();
                    }
                    else
                        ddlowner.SelectedIndex = 0;
                }
                else
                    ddlowner.SelectedIndex = 0;

                hdnprjID.Value = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_key"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_key"]).Trim() : string.Empty;
                 txtprojectcode.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_code"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_code"]).Trim() : string.Empty;
                txtprojectname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_name"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_name"]).Trim() : string.Empty;
                string lstr_kickoffdate= (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_kickoff_date"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_kickoff_date"]).Trim() : string.Empty;
                DateTime ldt = Convert.ToDateTime(lstr_kickoffdate);
                igwdp_kickoffdate.Value = ldt.ToString("yyyy-MM-dd");
               
                chkinactive.Checked = Convert.ToInt32(lds_projectdetail.Tables[0].Rows[0]["is_active"]) == 1 ? true : false;
                chkinactive.Enabled = true;

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion

        

        #endregion


        }
}