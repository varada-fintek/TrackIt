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
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_project_phases);
                GetProjectDetails();
              //  iwdg_project_phases.DataSource = GetDataSource();
                if (!IsPostBack)
                {
                    iwdg_project_phases.DataSource = GetDataSource();
                    iwdg_project_phases.DataBind();
                    DataSet lds_Client = ldbh_QueryExecutors.ExecuteDataSet("SELECT client_key AS [Value], client_name AS TextValue FROM prj_clients  WHERE is_active = 1 ORDER BY client_name");
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
                    DataSet lds_projowners = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp  inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='OWN' and cp.Active = 1 ORDER BY parameter_name");
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
                   // clearcontrols();
                }
                DataSet lds_phaseowner = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='OWN' and cp.Active = 1 ORDER BY parameter_name");
                ddpphaseowners.EditorControl.DataSource = lds_phaseowner;
                ddpphaseowners.EditorControl.ValueField = "Value";
                ddpphaseowners.EditorControl.TextField = "TextValue";
                ddpphaseowners.EditorControl.DataKeyFields = "Value";

                DataSet lds_phaseresource = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='RES' and cp.Active = 1 ORDER BY parameter_name");
                ddpphaseresource.EditorControl.DataSource = lds_phaseresource;
                ddpphaseresource.EditorControl.ValueField = "Value";
                ddpphaseresource.EditorControl.TextField = "TextValue";
                ddpphaseresource.EditorControl.DataKeyFields = "Value";
                if (!string.IsNullOrEmpty(hdnprjID.Value) && hdnpop.Value == "1")
                {
                    Int64? lint_projid = Convert.ToInt64(hdnprjID.Value.ToString());
                    btnSave.Visible = bitEdit;
                    EditProjectDetails(lint_projid);
                    iwdg_project_phases.DataSource = GetDataSource();
                    hdnpop.Value = string.Empty;
                    mpe_projectPopup.Show();
                }
                else
                {
                    iwdg_project_phases.DataSource = GetDataSource();
                }
               
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region Get project phasesGrid datasource
        private DataTable GetDataSource()
        {
            DataTable dataSource = null;

            if (Session["PrjPhasesTable"] == null)
            {
                //DataSet lds_set = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS[Value], cp.parameter_name AS TextValue,'' as Phases,'' as PhaseOwners FROM com_parameters cp inner join com_parameter_type cpt on cpt.parameter_type_code = cp.parameter_type WHERE cpt.parameter_type_code = 'PHA' and cp.Active = 1 ORDER BY parameter_name");
                DataSet lds_set = ldbh_QueryExecutors.ExecuteDataSet("select cast(iif(po.owner_key is not null or prs.resource_key is not null, 1, 0) as bit) CheckStatus,cp.parameter_key as Parameter_Key,cp.parameter_name Phase, po.owner_key Owner_key,pp.phase_key, prs.resource_key Resource_key from com_parameters cp left join prj_project_phases pp on cp.parameter_key=pp.project_phase_key and  pp.project_key=0 left join prj_project_phase_owners po on po.phase_key=pp.phase_key left join prj_project_phase_resources prs on prs.phase_key=pp.phase_key left join com_parameters co on co.parameter_key=po.owner_key left join com_parameters crs on crs.parameter_key=prs.resource_key where cp.parameter_type='PHA'  ");
                //dataSource = lds_set.Tables[0];
                dataSource = new DataTable(lds_set.Tables[0].TableName);
                DataColumn ldc_projectPK = new DataColumn("prj_prjKEY");
                ldc_projectPK.AutoIncrement = true;
                ldc_projectPK.AutoIncrement = true;
                ldc_projectPK.AutoIncrementStep = 1;
                ldc_projectPK.AutoIncrementSeed = 1;
                dataSource.Columns.Add(ldc_projectPK);
                dataSource.BeginLoadData();
                DataTableReader dtReader = new DataTableReader(lds_set.Tables[0]);
                dataSource.Load(dtReader);
                dataSource.EndLoadData();

                DataColumn[] PK = new DataColumn[1];
                PK[0] = dataSource.Columns["prj_prjKEY"];
                dataSource.PrimaryKey = PK;
               // this.Session["PrjPhasesTable"] = ldt_Prjdetails;
                this.Session.Add("PrjPhasesTable", dataSource);
            }
            else
            {
                dataSource = (DataTable)this.Session["PrjPhasesTable"];
               // dataSource.AcceptChanges();
            }
            return dataSource;
        }
        #endregion

        #region Project Master Grid initilizerow event
        private void iwdg_projectMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }

        }
        #endregion

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
                this.Session["PrjPhasesTable"] = null;
               // iwdg_project_phases.DataSource = null;
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
                    DataTable ldt_projectphase_info = (DataTable)this.Session["PrjPhasesTable"];
                    ldt_projectphase_info.AcceptChanges();
                     istr_tablename = "prj_project_phases";
                    foreach (DataRow lrow in ldt_projectphase_info.Rows)
                    {
                        if(Convert.ToBoolean(lrow["CheckStatus"])==true)
                        {
                            
                            istr_tablename = "prj_project_phases";
                             string lstr_phases_tableID = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                            {
                               {"project_key",Convert.ToInt64(lstr_id)},
                               {"project_phase_key",Convert.ToInt64(lrow["Parameter_Key"])},
                               {"is_active",(chkinactive.Checked? 1:0).ToString()},
                               {"created_by",this.LoggedInUserId },
                               {"Created_Date", DateTime.Now},
                               {"last_modified_By", this.LoggedInUserId },
                               {"last_modified_date", DateTime.Now}
                            }, lbool_type);

                             istr_tablename = "prj_project_phase_owners";
                             if (!string.IsNullOrEmpty(lrow["Owner_key"].ToString()))
                             {
                                 string lstr_phaseownerID = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                                        {
                                            {"owner_key",Convert.ToInt64(lrow["Owner_key"])},
                                            {"phase_key",Convert.ToInt64(lstr_phases_tableID)},
                                            {"is_active",(chkinactive.Checked? 1:0).ToString()},
                                            {"created_by",this.LoggedInUserId },
                                            {"Created_Date", DateTime.Now},
                                            {"last_modified_By", this.LoggedInUserId },
                                            {"last_modified_date", DateTime.Now}
                                        }, lbool_type);
                             }
                             if (!string.IsNullOrEmpty(lrow["Resource_key"].ToString()))
                             {
                                 istr_tablename = "prj_project_phase_resources";
                                 string lstr_phaseresourceID = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                                        {
                                            {"resource_key",Convert.ToInt64(lrow["Resource_key"])},
                                            {"phase_key",Convert.ToInt64(lstr_phases_tableID)},
                                            {"is_active",(chkinactive.Checked? 1:0).ToString()},
                                            {"created_by",this.LoggedInUserId },
                                            {"Created_Date", DateTime.Now},
                                            {"last_modified_By", this.LoggedInUserId },
                                            {"last_modified_date", DateTime.Now}
                                        }, lbool_type);
                             }
                        }
                    }
                    lstr_outMessage = "SUCCESS";
                }
                else
                {
                    lbool_type = false;
                    istr_tablename = "prj_projects";
                   lstr_outMessage = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
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
                    DataSet lds_del_phases_resource_owneres= ldbh_QueryExecutors.ExecuteDataSet("select pp.phase_key from prj_project_phases pp inner join prj_project_phase_owners po on po.phase_key=pp.phase_key inner join prj_project_phase_resources pr on pr.phase_key=pp.phase_key where pp.project_key=" + Convert.ToInt64(hdnprjID.Value));
                    foreach(DataRow ldrow_delete in lds_del_phases_resource_owneres.Tables[0].Rows)
                    {
                        ldbh_QueryExecutors.ExecuteNonQuery("Delete from prj_project_phase_owners where phase_key=" + ldrow_delete["phase_key"]);
                        ldbh_QueryExecutors.ExecuteNonQuery("Delete from prj_project_phase_resources where phase_key=" + ldrow_delete["phase_key"]);
                        ldbh_QueryExecutors.ExecuteNonQuery("Delete from prj_project_phases where phase_key=" + ldrow_delete["phase_key"]);
                    }
                    ldbh_QueryExecutors.ExecuteNonQuery("Delete from prj_project_phases where project_key=" + hdnprjID.Value);
                    DataTable ldt_projectphaseinfo = (DataTable)this.Session["PrjPhasesTable"];
                    ldt_projectphaseinfo.AcceptChanges();
                    istr_tablename = "prj_project_phases";
                    foreach (DataRow lrow in ldt_projectphaseinfo.Rows)
                    {
                        if (Convert.ToBoolean(lrow["CheckStatus"]) == true)
                        {
                           // if(string.IsNullOrEmpty(lrow["phase_key"].ToString()))
                            lbool_type = true;
                                istr_tablename = "prj_project_phases";
                                string lstr_phasesID = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                            {
                               {"project_key",Convert.ToInt64(hdnprjID.Value)},
                               {"project_phase_key",Convert.ToInt64(lrow["Parameter_Key"])},
                               {"is_active",(chkinactive.Checked? 1:0).ToString()},
                               {"created_by",this.LoggedInUserId },
                               {"Created_Date", DateTime.Now},
                               {"last_modified_By", this.LoggedInUserId },
                               {"last_modified_date", DateTime.Now}
                            }, lbool_type);
                                lstr_outMessage = "SUCCESS";
                                lbool_type = false;
                                if (!string.IsNullOrEmpty(lrow["Owner_key"].ToString()))
                                {
                                    istr_tablename = "prj_project_phase_owners";
                                    lstr_outMessage = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                                        {
                                            {"owner_key",Convert.ToInt64(lrow["Owner_key"])},
                                            {"phase_key",Convert.ToInt64(lstr_phasesID)},
                                            {"is_active",(chkinactive.Checked? 1:0).ToString()},
                                            {"created_by",this.LoggedInUserId },
                                            {"Created_Date", DateTime.Now},
                                            {"last_modified_By", this.LoggedInUserId },
                                            {"last_modified_date", DateTime.Now}
                                        }, lbool_type);
                                }
                                lbool_type = false;
                                if (!string.IsNullOrEmpty(lrow["Resource_key"].ToString()))
                                {
                                    istr_tablename = "prj_project_phase_resources";
                                     lstr_outMessage = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                                        {
                                            {"resource_key",Convert.ToInt64(lrow["Resource_key"])},
                                            {"phase_key",Convert.ToInt64(lstr_phasesID)},
                                            {"is_active",(chkinactive.Checked? 1:0).ToString()},
                                            {"created_by",this.LoggedInUserId },
                                            {"Created_Date", DateTime.Now},
                                            {"last_modified_By", this.LoggedInUserId },
                                            {"last_modified_date", DateTime.Now}
                                        }, lbool_type);
                                }
                        }
                    }
                   
                   
                }
                if (lstr_outMessage.Contains("SUCCESS"))
                {

                    GetProjectDetails();
                    SaveMessage();
                    clearcontrols();
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
                td.Width = 20;
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

        #region EditProjectDetails
        private void EditProjectDetails(Int64? aint_ProjectID)
        {
            try
            {
                Int64? lint_UserID = aint_ProjectID;

                //Fetch Single Record from table and assign to Edit
                DataSet lds_projectdetail = ldbh_QueryExecutors.ExecuteDataSet("select * from prj_projects pp where pp.project_key='" + aint_ProjectID + "'");
                DataSet lds_projectphases = ldbh_QueryExecutors.ExecuteDataSet("select cast(iif(po.owner_key is not null or prs.resource_key is not null, 1, 0)as bit )CheckStatus,cp.parameter_key as Parameter_Key,cp.parameter_name Phase, po.owner_key Owner_key,pp.phase_key,   prs.resource_key Resource_key from com_parameters cp left join prj_project_phases pp on cp.parameter_key=pp.project_phase_key and  pp.project_key='" + aint_ProjectID + "' left join prj_project_phase_owners po on po.phase_key=pp.phase_key left join prj_project_phase_resources prs on prs.phase_key=pp.phase_key left join com_parameters co on co.parameter_key=po.owner_key left join com_parameters crs on crs.parameter_key=prs.resource_key where cp.parameter_type='PHA'  ");
                
                DataTable ldt_prjdetails = new DataTable(lds_projectphases.Tables[0].TableName);

                DataColumn ldc_prjoectdetailskey = new DataColumn("prj_prjKEY");
                ldc_prjoectdetailskey.AutoIncrement = true;
                ldc_prjoectdetailskey.AutoIncrement = true;
                ldc_prjoectdetailskey.AutoIncrementStep = 1;
                ldc_prjoectdetailskey.AutoIncrementSeed = 1;
                ldt_prjdetails.Columns.Add(ldc_prjoectdetailskey);
                ldt_prjdetails.BeginLoadData();
                DataTableReader dtReader = new DataTableReader(lds_projectphases.Tables[0]);
                ldt_prjdetails.Load(dtReader);
                ldt_prjdetails.EndLoadData();

                DataColumn[] PK = new DataColumn[1];
                PK[0] = ldt_prjdetails.Columns["prj_prjKEY"];
                ldt_prjdetails.PrimaryKey = PK;
                this.Session["PrjPhasesTable"] = ldt_prjdetails;
                
                

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
                string lstr_kickoffdate = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_kickoff_date"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_kickoff_date"]).Trim() : string.Empty;
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