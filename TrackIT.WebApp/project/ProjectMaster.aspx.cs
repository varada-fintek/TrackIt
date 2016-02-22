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
        private static string istr_tablename = string.Empty;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                iwdg_projectMasterGrid = new WebDataGrid();
                pnl_projectGrid.Controls.Add(iwdg_projectMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_projectMasterGrid);
                ControlNames();
            clearcontrols();
            lblCreateProjects.Text = RollupText("Projects", "lblCreateProjects");
            iwdg_projectMasterGrid = new WebDataGrid();
            pnl_projectGrid.Controls.Add(iwdg_projectMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_projectMasterGrid);

   
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
                    //Unit Testing ID - UserMaster.aspx.cs_3
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_3 Roles dataset count" + lds_projowners.Tables[0].Rows.Count);
                    ddlowner.Items.Clear();
                    System.Web.UI.WebControls.ListItem llstm_li = new System.Web.UI.WebControls.ListItem("Select", "");
                    ddlowner.DataSource = lds_projowners;
                    ddlowner.DataTextField = "TextValue";
                    ddlowner.DataValueField = "Value";
                    ddlowner.DataBind();
                    ddlowner.Items.Insert(0, llstm_li);
                }
                   

                }
                GetprojectDetails();
                if (!string.IsNullOrEmpty(hdnprjID.Value) && hdnpop.Value == "1")
                {
                    //Edit User Details
                   Int64? lint_projectid = Convert.ToInt64(hdnprjID.Value.ToString());
                    EditprojectDetails(lint_projectid);
                    btnSave.Visible = bitEdit;
                   
                    // System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                    mpe_projectPopup.Show(); 
                }

            }
             catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

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
        private void ControlNames()
        {
            try
            {
                lblactive.Text = RollupText("Projects", "lblactive");
                lblclientname.Text = RollupText("Projects", "lblclientname");
                lblprojectcode.Text = RollupText("Projects", "lblprojectcode");
                lblprojectname.Text = RollupText("Projects", "lblprojectname");
                lblprojectowner.Text = RollupText("Projects", "lblprojectowner");
                reqvClient.ErrorMessage = RollupText("Projects", "reqvcode");
                reqvpname.ErrorMessage = RollupText("Projects", "reqvpname");
                lblkickdate.Text = RollupText("Projects", "lblkickdate");
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        private void clearcontrols()
        {
            try
            {
                chkinactive.Checked = true;
                chkinactive.Enabled = false;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
    

        protected void InsertorUpdateProjectDetails()
        {
            try
            {
                istr_tablename = "prj_projects";
                Boolean lbool_type = true;
                string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename,new System.Collections.Generic.Dictionary<string, object>
                {
                    {"project_code",txtprojectcode.Text.Replace("'", "''") },
                    {"project_name",txtprojectname.Text.Replace("'", "''") },
                    {"client_key",ddlClients.SelectedValue },
                    { "project_kickoff_date",igwdp_kickoffdate.Date},
                    {"project_owner",ddlowner.SelectedValue },
                    { "is_active",(chkinactive.Checked ? 1 : 0).ToString()},
                    {"created_by",this.LoggedInUserId },
                    {"Created_Date", DateTime.Now},
                    {"last_modified_By", this.LoggedInUserId },
                    {"last_modified_date", DateTime.Now}
                },lbool_type
                );
            }
            catch (Exception ex)
            {

                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        private void GetprojectDetails()
        {
            try
            {

                //Unit Testing ID - UserMaster.aspx.cs_18
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_18 GetUserDetails");
                //if (StringFunctions.IsNullOrEmpty(objUserBO)) objUserBO = new UserMasterBo();
                iwdg_projectMasterGrid.InitializeRow += iwdg_projectMasterGrid_InitializeRow;
                iwdg_projectMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
                iwdg_projectMasterGrid.Columns.Add(td);
                //Query to Get Landing Page Grid Details
                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select pp.project_key,pc.client_name,pp.project_code,pp.project_name,pp.project_kickoff_date,pp.project_owner,pp.is_active from prj_projects pp inner join prj_clients pc on pc.client_key = pp.client_key "); 
                iwdg_projectMasterGrid.Visible = false;

                if (lds_Result != null)
                {
                    if (lds_Result.Tables.Count > 0)
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_19
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_19 lds_Result set" + lds_Result.Tables.Count);
                        if (lds_Result.Tables[0].Rows.Count > 0)
                        {
                            ViewState["export"] = (DataTable)lds_Result.Tables[0];
                            iwdg_projectMasterGrid.DataSource = lds_Result.Tables[0];
                            iwdg_projectMasterGrid.DataBind();
                            DataColumn[] keyColumns = new DataColumn[1];
                            DataTable ldt_dt = lds_Result.Tables[0];
                            iwdg_projectMasterGrid.DataKeyFields = "project_key";
                            keyColumns[0] = ldt_dt.Columns["project_key"];
                            ldt_dt.PrimaryKey = keyColumns;
                            iwdg_projectMasterGrid.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                // if (objUserBO != null) objUserBO = null;
            }
        }
        private void EditprojectDetails(Int64? aint_UserID)
        {
            try
            {

              
                Int64? lint_projectID = aint_UserID;
              
               
                DataSet lds_projectdetail = ldbh_QueryExecutors.ExecuteDataSet("select * from prj_projects pp where pp.project_key='" + lint_projectID + "'");
                if (lds_projectdetail.Tables[0].Rows.Count > 0)
                {
                   

                   

                    

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

                    hdnprjID.Value = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_key"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_key"]).Trim() : string.Empty;
                    
                    
                  txtprojectcode.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_code"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_code"]).Trim() : string.Empty;
                    txtprojectname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_name"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_name"]).Trim() : string.Empty;
                   
                    
                   
                    igwdp_kickoffdate.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_kickoff_date"]))) ? Convert.ToString(lds_projectdetail.Tables[0].Rows[0]["project_kickoff_date"]).Trim() : string.Empty;
                    chkinactive.Checked = Convert.ToInt32(lds_projectdetail.Tables[0].Rows[0]["is_active"]) == 1 ? true : false;
                    chkinactive.Enabled = true;

                }
                else
                {
                    
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            finally
            {
                // if (objUserBO != null) objUserBO = null;
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            mpe_projectPopup.Hide();

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertorUpdateProjectDetails();
           // mpe_projectPopup.Show();
        }
    }
}