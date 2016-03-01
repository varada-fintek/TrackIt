﻿using System;
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
                //Unit Testing ID - ProjectMaster.aspx.cs_1
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_1 PageLoad");
                ControlNames();

                iwdg_projectMasterGrid = new WebDataGrid();
                iwdg_panelGrid = new WebDataGrid();

                pnl_projectGrid.Controls.Add(iwdg_projectMasterGrid);

                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_projectMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_panelGrid);
                if (!IsPostBack)
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_2
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_2 PageLoad IsPostBack  ");
                    //Assign all dropdown data clientname Drop Down
                    DataSet lds_Client = ldbh_QueryExecutors.ExecuteDataSet("SELECT client_key AS [Value], client_name AS TextValue FROM prj_clients  WHERE is_active = 1 ORDER BY client_name");
                    if (lds_Client.Tables[0].Rows.Count > 0)
                    {
                        //Unit Testing ID - ProjectMaster.aspx.cs_3
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_3 Roles dataset count" + lds_Client.Tables[0].Rows.Count);
                        ddlClients.Items.Clear();
                        System.Web.UI.WebControls.ListItem llstm_li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlClients.DataSource = lds_Client;
                        ddlClients.DataTextField = "TextValue";
                        ddlClients.DataValueField = "Value";
                        ddlClients.DataBind();
                        ddlClients.Items.Insert(0, llstm_li);
                    }
                    //Assign all dropdown data Project Owner Drop Down
                    DataSet lds_projowners = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp  inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='OWN' and cp.Active = 1 ORDER BY parameter_name");
                    if (lds_projowners.Tables[0].Rows.Count > 0)
                    {
                        //Unit Testing ID - ProjectMaster.aspx.cs_4
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_4 ProjectOwner dataset count" + lds_projowners.Tables[0].Rows.Count);

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
                    //Edit Project Details
                    //Unit Testing ID - ProjectMaster.aspx.cs_5
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_5 Edit project Details popId and Unique ID" + hdnprjID.Value + hdnpop.Value);

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
            try
            {
                //Unit Testing ID - ProjectMaster.aspx.cs_18
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_18 intialize grid row " + e.Row.Index);
                //throw new NotImplementedException();
                if (e.Row.Index == 0)
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_19
                    e.Row.Items.FindItemByKey("project_key").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("client_name").Column.Header.Text = RollupText("projects", "gridclientname");
                    e.Row.Items.FindItemByKey("project_code").Column.Header.Text = RollupText("projects", "gridprojectcode");
                    e.Row.Items.FindItemByKey("project_name").Column.Header.Text = RollupText("projects", "gridprojectname");
                    e.Row.Items.FindItemByKey("project_kickoff_date").Column.Header.Text = RollupText("projects", "gridprojectkickoffdate");
                    e.Row.Items.FindItemByKey("project_owner").Column.Header.Text = RollupText("projects", "gridprojectowner");
                    e.Row.Items.FindItemByKey("is_active").Column.Header.Text = RollupText("projects", "gridisactive");
                    if (!IsPostBack)
                    {
                        //Grid Postback to onRowSorting and Grid Filtering

                        for (int lint_i = 0; lint_i < e.Row.Items.Count; lint_i++)
                        {
                            if (e.Row.Items[lint_i].Column.Type.FullName.ToString().Equals("System.String") && !string.IsNullOrEmpty(e.Row.Items[lint_i].Column.Key))
                            {
                                //Unit Testing ID - ProjectMaster.aspx.cs_20
                                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_20 " + e.Row.Items[lint_i].Column.Key);
                                ColumnFilter filter = new ColumnFilter();
                                filter.ColumnKey = e.Row.Items[lint_i].Column.Key;
                                filter.Condition = new RuleTextNode(TextFilterRules.Contains, "");
                                iwdg_projectMasterGrid.Behaviors.Filtering.ColumnFilters.Add(filter);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }

        }
        #endregion

        private void Iwdg_projectphases_InitializeRow(object sender, RowEventArgs e)
        {
            try
            {
                if (e.Row.Index == 0)
                {
                    e.Row.Items.FindItemByKey("Value").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("TextValue").Column.Header.Text = "Phases";
                    e.Row.Items.FindItemByKey("Phases").Column.Header.Text = "Phaseowners";
                    e.Row.Items.FindItemByKey("PhaseOwners").Column.Header.Text = "Phaseresource";
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #region Button Clear click
        protected void btnClear_Click(object sender, EventArgs e)
        {
            //Unit Testing ID - ProjectMaster.aspx.cs_9
            System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_9 bUtton Clear_Click");
            EnableDisableControls(true);
            clearcontrols();
            GetProjectDetails();
            mpe_projectPopup.Hide();
        }
        #endregion

        #region EnableDisableControls
        private void EnableDisableControls(bool bValue)
        {
            try
            {
                //Unit Testing ID - ProjectMaster.aspx.cs_22
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_22 EnableDiasble Controls");


            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region button Save click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Project key Unique validations
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
                        // Unit Testing ID - ProjectMaster.aspx.cs_15
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_15 project_key Unique check");

                    }
                    else
                    {
                        //Unit Testing ID - ProjectMaster.aspx.cs_7
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_9 validate Page and Insert/Update Project" + Page.IsValid);
                        InsertorUpdateProjectDetails();

                        //Unit Testing ID - ProjectMaster.aspx.cs_8
                        // System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_10 Pagevalidation Fails" + Page.IsValid);
                        //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                    }
                }
                else
                {

                    //Unit Testing ID - ProjectMaster.aspx.cs_7
                    InsertorUpdateProjectDetails();
                    //Unit Testing ID - ProjectMaster.aspx.cs_8
                    //System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_8 Pagevalidation Fails" + Page.IsValid);
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                }
                //Unit Testing ID - ProjectMaster.aspx.cs_6
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs6 SaveButtonClick");
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
                //Unit Testing ID - ProjectMaster.aspx.cs_23
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_23 ControlNames");
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
                //Unit Testing ID - ProjectMaster.aspx.cs_10
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_10 InsertUpdateProject Details");
                istr_tablename = "prj_projects";
                Boolean lbool_type = true;
                string lstr_outMessage = string.Empty;
                // hdnprjID to check weather Insert / Update.
                if (string.IsNullOrEmpty(hdnprjID.Value))
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_11
                    string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                {
                        // {"project_key", lstr_project_key},
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
                // Update  Project Details.
                else
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_12
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_14 Update Project" + hdnprjID.Value);
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
                //Sucess Message After Insert/Update
                if (lstr_outMessage.Contains("SUCCESS"))
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_13
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_13 success Measage" + lstr_outMessage);

                    string[] sBUID = lstr_outMessage.Split('^');
                    GetProjectDetails();
                    SaveMessage();
                    // ClearControls();
                    mpe_projectPopup.Hide();
                    return;
                }
                else
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_14
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_14 ErrorMessage");
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
                //Unit Testing ID - ProjectMaster.aspx.cs_16
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_16 GetprojectDetails");
                iwdg_projectMasterGrid.InitializeRow += iwdg_projectMasterGrid_InitializeRow;
                iwdg_projectMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
                iwdg_projectMasterGrid.Columns.Add(td);
                //Query to Get Landing Page Grid Details

                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select pp.project_key, pc.client_name, pp.project_code,pp.project_name,pp.project_kickoff_date,pp.project_owner,pp.is_active from prj_projects pp inner join prj_clients pc on pc.client_key=pp.client_key");
                if (lds_Result.Tables[0].Rows.Count > 0)
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_17
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_17 lds_Result set" + lds_Result.Tables.Count);
                    ViewState["export"] = (DataTable)lds_Result.Tables[0];
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
                //Unit Testing ID - ProjectMaster.aspx.cs_24
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_24 GetpanelDetails");
                iwdg_projectphases.InitializeRow += Iwdg_projectphases_InitializeRow;
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;

                DataSet lds_Result;
                //lds_Result = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS[Value], cp.parameter_name AS TextValue,cp.parameter_key as Phases,cp.parameter_key as Phaseowners FROM com_parameters cp inner join com_parameter_type cpt on cpt.parameter_type_code = cp.parameter_type WHERE cpt.parameter_type_code = 'PHA' and cp.Active = 1 ORDER BY parameter_name");
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS[Value], cp.parameter_name AS TextValue,'' as Phases,'' as PhaseOwners FROM com_parameters cp inner join com_parameter_type cpt on cpt.parameter_type_code = cp.parameter_type WHERE cpt.parameter_type_code = 'PHA' and cp.Active = 1 ORDER BY parameter_name");
                if (lds_Result.Tables[0].Rows.Count > 0)
                {
                    //Unit Testing ID - ProjectMaster.aspx.cs_25
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_25 lds_Result set" + lds_Result.Tables.Count);
                    DataColumn[] keys = new DataColumn[1];
                    keys[0] = lds_Result.Tables[0].Columns[0];
                    // Then assign the array to the PrimaryKey property of the DataTable. 
                    lds_Result.Tables[0].PrimaryKey = keys;
                    iwdg_projectphases.DataSource = lds_Result.Tables[0];
                    iwdg_projectphases.DataBind();
                }
                // Enable cell editing
                //Unit Testing ID - ProjectMaster.aspx.cs_26
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_26 Projectphases dataset count");
                this.iwdg_projectphases.Behaviors.CreateBehavior<EditingCore>();
                this.iwdg_projectphases.Behaviors.EditingCore.Behaviors.CreateBehavior<CellEditing>();
                
                DataSet lds_phaseownerresult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp  inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='OWN' and cp.Active = 1 ORDER BY parameter_name");
                //Unit Testing ID - ProjectMaster.aspx.cs_27
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_27 Projectphaseowner dataset count" + lds_phaseownerresult.Tables[0].Rows.Count);
                this.iwdg_projectphases.EditorProviders.Add(ddpPhaseowner);
                EditingColumnSetting phaseownerecolumn = new EditingColumnSetting();
                phaseownerecolumn.ColumnKey = "Phases";
                phaseownerecolumn.EditorID = ddpPhaseowner.ID;
                ddpPhaseowner.EditorControl.ValueField = "Value";
                ddpPhaseowner.EditorControl.TextField = "TextValue";
                ddpPhaseowner.EditorControl.DataSource = lds_phaseownerresult.Tables[0];
                this.iwdg_projectphases.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(phaseownerecolumn);



                DataSet lds_phaseresourceresult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='RES' and cp.Active = 1 ORDER BY parameter_name");
                //Unit Testing ID - ProjectMaster.aspx.cs_27
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_27 Projectphaseresources dataset count" + lds_phaseresourceresult.Tables[0].Rows.Count);
                this.iwdg_projectphases.EditorProviders.Add(ddpPhaseprovider);
                EditingColumnSetting phaseresourcecolumn = new EditingColumnSetting();
                phaseresourcecolumn.ColumnKey = "PhaseOwners";
                phaseresourcecolumn.EditorID = ddpPhaseprovider.ID;
                ddpPhaseprovider.EditorControl.ValueField = "Value";
                ddpPhaseprovider.EditorControl.TextField = "TextValue";
                ddpPhaseprovider.EditorControl.DataSource = lds_phaseresourceresult.Tables[0];
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
                //Unit Testing ID - 
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_21 GetProject Details Edit" + aint_ProjectID);
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

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - ProjectMaster.aspx.cs_29
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_29 Export Excel");

                DataTable ldt_ExcelExp = (DataTable)ViewState["export"];
                iwdg_projectMasterGrid.DataSource = ldt_ExcelExp;
                iwdg_projectMasterGrid.DataBind();
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebExcelExporter.Export(iwdg_projectMasterGrid);
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebExcelExporter.Export(this.iwdg_projectMasterGrid);

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                //Response.Redirect("~/Error.aspx", false);
            }
        }

        /// <summary>
        /// Export to pdf button click Event
        /// </summary>
        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - ProjectMaster.aspx.cs_30
                System.Diagnostics.Debug.WriteLine("Unit testing ID - ProjectMaster.aspx.cs_30 ExportPdf");
                DataTable ldt_PdfExp = (DataTable)ViewState["export"];
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_projectMasterGrid);
                iwdg_projectMasterGrid.DataSource = ldt_PdfExp;
                iwdg_projectMasterGrid.DataBind();
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebPDFExporter.Export(iwdg_projectMasterGrid);
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebPDFExporter.Export(this.iwdg_projectMasterGrid);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                // Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion


    }
}