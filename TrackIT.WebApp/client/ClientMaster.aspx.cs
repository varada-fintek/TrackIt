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

namespace TrackIT.WebApp.client
{
    public partial class ClientMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid lwdg_clientMasterGrid;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ControlNames();            
            lwdg_clientMasterGrid = new WebDataGrid();
            pnl_clientGrid.Controls.Add(lwdg_clientMasterGrid);
            TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_clientMasterGrid);
            
            lwdg_clientMasterGrid.InitializeRow += Lwdg_clientMasterGrid_InitializeRow;
            lwdg_clientMasterGrid.Columns.Clear();
            TemplateDataField td = new TemplateDataField();
            td.ItemTemplate = new CustomItemTemplateView();
            td.Key = "Edit";
            td.Width = 30;
            lwdg_clientMasterGrid.Columns.Add(td);

            DataSet lds_Result;
            lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select client_code,client_name,client_address_1,client_address_2,client_city,client_state,client_zip,client_country,client_contact_name,client_contact_designation from prj_clients");
            if (lds_Result.Tables[0].Rows.Count > 0)
            {
                lwdg_clientMasterGrid.DataSource = lds_Result.Tables[0];
                lwdg_clientMasterGrid.DataBind();
                
            }
            

            if (!IsPostBack)
            {
                

                //Assign all dropdown data User Location Drop Down
                DataSet lds_ClientLocation = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='CON' and cp.Active = 1 ORDER BY parameter_name");
                if (lds_ClientLocation.Tables[0].Rows.Count > 0)
                {
                    //Unit Testing ID - UserMaster.aspx.cs_4
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_4 userLoaction dataset count" + lds_ClientLocation.Tables[0].Rows.Count);
                    ddlbillCountry.Items.Clear();
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                    ddlbillCountry.DataSource = lds_ClientLocation;
                    ddlbillCountry.DataTextField = "TextValue";
                    ddlbillCountry.DataValueField = "Value";
                    ddlbillCountry.DataBind();
                    ddlbillCountry.Items.Insert(0, li);

                    ddladdresscountry.Items.Clear();                    
                    ddladdresscountry.DataSource = lds_ClientLocation;
                    ddladdresscountry.DataTextField = "TextValue";
                    ddladdresscountry.DataValueField = "Value";
                    ddladdresscountry.DataBind();
                    ddladdresscountry.Items.Insert(0, li);
                }
            }
        }

        private void Lwdg_clientMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Index == 0)
            {
                
                e.Row.Items.FindItemByKey("client_code").Column.Header.Text = RollupText("client", "gridclientscode");
                e.Row.Items.FindItemByKey("client_name").Column.Header.Text = RollupText("client", "gridclientsname");
                e.Row.Items.FindItemByKey("client_address_1").Column.Header.Text = RollupText("client", "gridclientsaddress1");
                e.Row.Items.FindItemByKey("client_address_2").Column.Header.Text = RollupText("client", "gridclientsaddress2");
                e.Row.Items.FindItemByKey("client_city").Column.Header.Text = RollupText("client", "gridclientscity");
                e.Row.Items.FindItemByKey("client_state").Column.Header.Text = RollupText("client", "gridclientsstate");
                e.Row.Items.FindItemByKey("client_zip").Column.Header.Text = RollupText("client", "gridclientszip");
                e.Row.Items.FindItemByKey("client_country").Column.Header.Text = RollupText("client", "gridclientscountry");
                e.Row.Items.FindItemByKey("client_contact_name").Column.Header.Text = RollupText("client", "gridclientscontactname");
                e.Row.Items.FindItemByKey("client_contact_designation").Column.Header.Text = RollupText("client", "gridclientsdesignation");
            }
            if (!IsPostBack)
            {
                //Grid Postback to onRowSorting and Grid Filtering

                for (int lint_i = 0; lint_i < e.Row.Items.Count; lint_i++)
                {
                    if (e.Row.Items[lint_i].Column.Type.FullName.ToString().Equals("System.String") && !string.IsNullOrEmpty(e.Row.Items[lint_i].Column.Key))
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_22
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_22 " + e.Row.Items[lint_i].Column.Key);
                        ColumnFilter filter = new ColumnFilter();
                        filter.ColumnKey = e.Row.Items[lint_i].Column.Key;
                        filter.Condition = new RuleTextNode(TextFilterRules.Contains, "");
                        lwdg_clientMasterGrid.Behaviors.Filtering.ColumnFilters.Add(filter);
                    }
                }
            }           
            //throw new NotImplementedException();
        }
        private void ControlNames()
        {
            try
            {

                lblCreateClient.Text = RollupText("Client", "lblCreateClients");                
                lblclientname.Text = RollupText("Client", "lblclientname");
                lblIsactive.Text = RollupText("Client", "lblIsactive");
                lblclientcode.Text = RollupText("Client", "lblclientcode");
                lblbilladdress1.Text = RollupText("Client", "lblbilladdress1");
                lblbilladdress2.Text = RollupText("Client", "lblbilladdress2");
                lblbillcity.Text = RollupText("Client", "lblbillcity");
                lblbillstate.Text = RollupText("Client", "lblbillstate");
                lblbillzip.Text = RollupText("Client", "lblbillzip");
                lblbillcountry.Text = RollupText("Client", "lblbillcountry");
                lblbillinfosame.Text = RollupText("Client", "lblbillinfosame");
                lblclientcontactname.Text = RollupText("Client", "lblclientcontactname");
                lblclientcontactdesignation.Text=RollupText("Client","lblclientcontactdesignation");
                lbladdresscity.Text = RollupText("Client", "lbladdresscity");
                lbladdresscountry.Text = RollupText("Client", "lbladdresscountry");
                lbladdressline1.Text = RollupText("Client", "lbladdressline1");
                lbladdressline2.Text = RollupText("Client", "lbladdressline2");
                lbladdressstate.Text = RollupText("Client", "lbladdressstate");
                lbladdresszip.Text = RollupText("Client", "lbladdresszip");               

            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            
        }

        protected void chkbillinfosame_CheckedChanged(object sender, EventArgs e)
        {
            mpe_clientPopup.Show();
            if (chkbillinfosame.Checked == true)
            {
                txtaddresscity.Text = txtbillcity.Text;
                txtaddressline1.Text = txtbilladdress1.Text;
                txtaddressline2.Text = txtbilladdress2.Text;
                txtaddressstate.Text = txtbillstate.Text;
                txtaddresszip.Text = txtbillzip.Text;
                ddladdresscountry.SelectedIndex = ddlbillCountry.SelectedIndex;

            }
            else if (chkbillinfosame.Checked == false)
            {
                txtaddresscity.Text = "";
                txtaddressline1.Text = "";
                txtaddressline2.Text = "";
                txtaddressstate.Text = "";
                txtaddresszip.Text = "";
                ddladdresscountry.SelectedIndex = 0;
               
            }
        }
    }
}