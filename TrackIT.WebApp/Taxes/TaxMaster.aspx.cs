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
    public partial class TaxMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid iwdg_TaxMasterGrid;
        WebDataGrid iwdg_TaxDetailsGrid;
        private static string istr_tablename = string.Empty;
        #endregion

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                ControlNames();
                iwdg_TaxMasterGrid = new WebDataGrid();
                pnl_taxGrid.Controls.Add(iwdg_TaxMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxMasterGrid);

                iwdg_TaxDetailsGrid = new WebDataGrid();
                pnl_taxdetailsGrid.Controls.Add(iwdg_TaxDetailsGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxDetailsGrid);
                GetTaxDetails();

                if (!string.IsNullOrEmpty(hdntaxkey.Value) && hdnpop.Value == "1")
                {
                    Int64? lint_TaxID = Convert.ToInt64(hdntaxkey.Value.ToString());
                    txttaxcode.Enabled = false;
                    btnSave.Visible = bitEdit;
                    EditTaxDetails(lint_TaxID);
                    // System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                    mpe_taxPopup.Show();

                }


                if (!IsPostBack)
                {
                    GetTaxinfoDetails();  
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region User Defined Functions

        #region Intialize Row for Grids
        private void iwdg_TaxDetailsGrid_InitializeRow(object sender, RowEventArgs e)
        {
            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("tax_from").Column.Header.Text = RollupText("Taxes", "detailsgridtaxfrom");
                e.Row.Items.FindItemByKey("tax_to").Column.Header.Text = RollupText("Taxes", "detailsgridtaxto");
                e.Row.Items.FindItemByKey("tax_percent").Column.Header.Text = RollupText("Taxes", "detailsgridtaxpercent");
                e.Row.Items.FindItemByKey("tax_type").Column.Header.Text = RollupText("Taxes", "detailsgridtaxtype");
                e.Row.Items.FindItemByKey("tax_applied_on").Column.Header.Text = RollupText("Taxes", "detailsgridtaxappliedon");
            }
        }

        private void iwdg_TaxMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Row.Index == 0)
            {
                e.Row.Items.FindItemByKey("tax_key").Column.Hidden = true;
                e.Row.Items.FindItemByKey("tax_tax_code").Column.Header.Text = RollupText("Taxes", "gridtaxcode");
                e.Row.Items.FindItemByKey("tax_tax_name").Column.Header.Text = RollupText("Taxes", "gridtaxname");
            }
        }
        #endregion

        #region Control Names
        private void ControlNames()
        {
            lblCreatetaxes.Text = RollupText("Taxes", "lblCreatetaxes");
            lbltaxname.Text = RollupText("Taxes", "lbltaxname");
            lbltaxcode.Text = RollupText("Taxes", "lbltaxcode");
            lnkAddrow.Text = RollupText("Taxes", "lnkAddrow");
            reqvtaxcode.ErrorMessage = RollupText("Taxes", "reqvtaxcode");
            reqvtxttaxname.ErrorMessage = RollupText("Taxes", "reqvtxttaxname");
            reqvtaxcodeUNQ.ErrorMessage = RollupText("Taxes", "reqvtaxcodeUNQ");
        }
        #endregion

        #region Get Tax Details
        private void GetTaxDetails()
        {
            try
            {
                iwdg_TaxMasterGrid.InitializeRow += iwdg_TaxMasterGrid_InitializeRow;
                iwdg_TaxMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
                iwdg_TaxMasterGrid.Columns.Add(td);
                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select tax_key,tax_tax_code,tax_tax_name from prj_taxes");
                if (lds_Result.Tables[0].Rows.Count > 0)
                {
                    iwdg_TaxMasterGrid.DataSource = lds_Result.Tables[0];
                    iwdg_TaxMasterGrid.DataBind();
                }

                
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region Edit Tax Details
        private void EditTaxDetails(Int64? aint_TaxID)
        {
            try
            {
                Int64? lint_TaxID = aint_TaxID;

                //Fetch Single Record from table and assign to Edit
                DataSet lds_taxdetail = ldbh_QueryExecutors.ExecuteDataSet("select * from prj_taxes pt where pt.tax_key='" + aint_TaxID + "'");

                if (lds_taxdetail.Tables[0].Rows[0]["tax_key"] != null)
                {
                    hdntaxkey.Value = (!string.IsNullOrEmpty(Convert.ToString(lds_taxdetail.Tables[0].Rows[0]["tax_key"]))) ? Convert.ToString(lds_taxdetail.Tables[0].Rows[0]["tax_key"]).Trim() : string.Empty;
                    txttaxcode.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_taxdetail.Tables[0].Rows[0]["tax_tax_code"]))) ? Convert.ToString(lds_taxdetail.Tables[0].Rows[0]["tax_tax_code"]).Trim() : string.Empty;
                    txttaxname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_taxdetail.Tables[0].Rows[0]["tax_tax_name"]))) ? Convert.ToString(lds_taxdetail.Tables[0].Rows[0]["tax_tax_name"]).Trim() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion

        #region Get Tax info Details
        private void GetTaxinfoDetails()
        {
            try
            {
                
                    DataSet lds_taxResult;
                    lds_taxResult = ldbh_QueryExecutors.ExecuteDataSet("select tax_from,tax_to,tax_percent,tax_type,tax_applied_on from prj_taxes_details");
                    
                    if (lds_taxResult.Tables[0].Rows.Count > 0)
                    {
                       
                        //iwdg_TaxDetailsGrid.DataSource = lds_taxResult.Tables[0];
                        //iwdg_TaxDetailsGrid.DataBind();
                        DataTable ldt_sessiontax=(DataTable)ViewState["vsTaxdetails"];
                        if (ldt_sessiontax != null)
                        {
                            if (ldt_sessiontax.Rows.Count > 0)
                            {
                                checkgrid.DataSource = ldt_sessiontax;
                            }
                        }
                        else
                        {
                            checkgrid.DataSource = lds_taxResult.Tables[0];
                        }
                        checkgrid.DataBind();
                        //iwdg_TaxDetailsGrid.DataBind();
                        ViewState["vsTaxdetails"] = (DataTable)lds_taxResult.Tables[0];
                    }

                    //iwdg_TaxDetailsGrid.InitializeRow += iwdg_TaxDetailsGrid_InitializeRow;
                    checkgrid.InitializeRow +=iwdg_TaxDetailsGrid_InitializeRow;
                    // Enable cell editing
                    this.checkgrid.Behaviors.CreateBehavior<EditingCore>();
                    this.checkgrid.Behaviors.EditingCore.Behaviors.CreateBehavior<CellEditing>();
                    this.checkgrid.EditorProviders.Add(FromdateProvider);
                    this.checkgrid.EditorProviders.Add(TodateProvider);
                    
                    this.checkgrid.EditorProviders.Add(TaxappliedonProvider);

                    EditingColumnSetting fromdatecolumn = new EditingColumnSetting();
                    fromdatecolumn.ColumnKey = "tax_from";
                    
                    fromdatecolumn.EditorID = FromdateProvider.ID;

                    EditingColumnSetting todatecolumn = new EditingColumnSetting();
                    todatecolumn.ColumnKey = "tax_to";
                    todatecolumn.EditorID = TodateProvider.ID;


                    this.checkgrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(fromdatecolumn);
                    this.checkgrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(todatecolumn);


                    DataSet lds_taxtyperesult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TYP' and cp.Active = 1 ORDER BY parameter_name");
                    this.checkgrid.EditorProviders.Add(TaxtypeProvider);
                    EditingColumnSetting taxtypecolumn = new EditingColumnSetting();
                    taxtypecolumn.ColumnKey = "tax_type";
                    taxtypecolumn.EditorID = TaxtypeProvider.ID;
                    TaxtypeProvider.EditorControl.ValueField = "Value";
                    TaxtypeProvider.EditorControl.TextField = "TextValue";
                    TaxtypeProvider.EditorControl.DataSource = lds_taxtyperesult.Tables[0];
                    this.checkgrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(taxtypecolumn);

                    DataSet lds_taxappliedonresult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TAO' and cp.Active = 1 ORDER BY parameter_name");
                    this.checkgrid.EditorProviders.Add(TaxtypeProvider);
                    EditingColumnSetting taxappliedoncolumn = new EditingColumnSetting();
                    taxappliedoncolumn.ColumnKey = "tax_applied_on";
                    taxappliedoncolumn.EditorID = TaxappliedonProvider.ID;
                    TaxappliedonProvider.EditorControl.ValueField = "Value";
                    TaxappliedonProvider.EditorControl.TextField = "TextValue";
                    TaxappliedonProvider.EditorControl.DataSource = lds_taxappliedonresult.Tables[0];
                    this.checkgrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(taxappliedoncolumn);

                    //if (!IsPostBack)
                    //{
                    //    // Enable cell editing
                    //    this.iwdg_TaxDetailsGrid.Behaviors.CreateBehavior<EditingCore>();
                    //    this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CreateBehavior<CellEditing>();

                    //    // Create an editor provider
                    //    DatePickerProvider taxfromdate = new DatePickerProvider();

                    //    taxfromdate.ID = "taxfromdateID";


                    //    // Add to collection
                    //    this.iwdg_TaxDetailsGrid.EditorProviders.Add(taxfromdate);

                    //    // Create a column setting to use the editor provider
                    //    EditingColumnSetting columnSettingfrom = new EditingColumnSetting();
                    //    columnSettingfrom.ColumnKey = "tax_from";

                    //    // Assign editor for column to use
                    //    columnSettingfrom.EditorID = taxfromdate.ID;

                    //    // Add column setting
                    //    this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSettingfrom);



                    //    // Create an editor provider
                    //    DatePickerProvider taxtodate = new DatePickerProvider();
                    //    taxtodate.ID = "taxtodateID";

                    //    // Add to collection
                    //    this.iwdg_TaxDetailsGrid.EditorProviders.Add(taxtodate);

                    //    // Create a column setting to use the editor provider
                    //    EditingColumnSetting columnSettingto = new EditingColumnSetting();
                    //    columnSettingto.ColumnKey = "tax_to";

                    //    // Assign editor for column to use
                    //    columnSettingto.EditorID = taxtodate.ID;
                    //    taxfromdate.EditorControl.CssClass = "chzn-container";

                    //    // Add column setting
                    //    this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSettingto);

                    //    DataSet lds_taxtype = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TYP' and cp.Active = 1 ORDER BY parameter_name");

                    //    //Adding dropdown to DetailsGrid
                    //    DropDownProvider ddl_taxtype = new DropDownProvider();
                    //    ddl_taxtype.ID = "taxtypedropdown";
                    //    this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxtype);
                    //    EditingColumnSetting columnSettingtaxtype = new EditingColumnSetting();
                    //    columnSettingtaxtype.ColumnKey = "tax_type";
                    //    columnSettingtaxtype.EditorID = ddl_taxtype.ID;
                    //    ddl_taxtype.EditorControl.ValueField = "Value";
                    //    ddl_taxtype.EditorControl.TextField = "TextValue";
                    //    ddl_taxtype.EditorControl.DataSource = lds_taxtype.Tables[0];

                    //    this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxtype);

                    //    this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSettingtaxtype);

                    //    DataSet lds_taxappliedon = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TAO' and cp.Active = 1 ORDER BY parameter_name");

                    //    DropDownProvider ddl_taxappliedon = new DropDownProvider();
                    //    ddl_taxappliedon.ID = "taxappliedondropdown";
                    //    this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxappliedon);
                    //    EditingColumnSetting columnSettingtaxappliedon = new EditingColumnSetting();
                    //    columnSettingtaxappliedon.ColumnKey = "tax_applied_on";
                    //    columnSettingtaxappliedon.EditorID = ddl_taxappliedon.ID;
                    //    ddl_taxappliedon.EditorControl.ValueField = "Value";
                    //    ddl_taxappliedon.EditorControl.TextField = "TextValue";
                    //    ddl_taxappliedon.EditorControl.DataSource = lds_taxappliedon.Tables[0];

                    //    this.iwdg_TaxDetailsGrid.EditorProviders.Add(ddl_taxappliedon);

                    //    this.iwdg_TaxDetailsGrid.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(columnSettingtaxappliedon);
                    //}
                
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region InsertorUpdate
        protected void InsertorUpdateTaxDetails()
        {
            try
            {
                string astr_tax_key = Convert.ToString(Guid.NewGuid());
                istr_tablename = "prj_taxes";
                Boolean lbool_type = true;
                string lstr_outMessage = string.Empty;
                if (string.IsNullOrEmpty(hdntaxkey.Value))
                {

                    string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object> 
                {
                    {"tax_tax_name",txttaxname.Text.Replace("'","''")},
                    {"tax_tax_code",txttaxcode.Text.Replace("'","''")},
                }, lbool_type
                    );
                }
                else
                {
                    lbool_type = false;

                    istr_tablename = "prj_taxes";
                    string id = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                    {
                        {"tax_tax_name",txttaxname.Text.Replace("'", "''") },
                        {"tax_tax_code",txttaxcode.Text.Replace("'", "''") }
                    },
                    new System.Collections.Generic.Dictionary<string, object>()
                     {
                         {"tax_key", hdntaxkey.Value},
                     },
                    lbool_type
                   );

                    lstr_outMessage = "SUCCESS";
                }
                
                if (lstr_outMessage.Contains("SUCCESS"))
                {

                    string[] sBUID = lstr_outMessage.Split('^');
                    GetTaxDetails();
                    SaveMessage();
                   // ClearControls();
                    mpe_taxPopup.Hide();
                    return;
                }
                else
                {
                   
                    
                    Response.Redirect("~/Taxes/TaxMaster.aspx", false);
                }
            
                
                 
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        
        
        private void ClearControls()
        {
            try
            {
                txttaxcode.Text = string.Empty;
                txttaxname.Text = string.Empty;
                btnSave.Visible = bitAdd;
                
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #endregion

        #region Postback Events

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearControls();
            mpe_taxPopup.Hide();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            InsertorUpdateTaxDetails();
            mpe_taxPopup.Show();
        }

        protected void lnkAddrow_Click(object sender, EventArgs e)
        {
           // GetTaxinfoDetails();
           
            DataTable lds_taxResult = (DataTable) ViewState["vsTaxdetails"];
            DataRow toInsert = lds_taxResult.NewRow();
            toInsert["tax_from"] = DateTime.Now;
            toInsert["tax_to"] = DateTime.Now;
            toInsert["tax_percent"] = string.Empty;
            toInsert["tax_type"] = -1;
            toInsert["tax_applied_on"] = -1;
            lds_taxResult.Rows.InsertAt(toInsert, lds_taxResult.Rows.Count);
            ViewState["vsTaxdetails"] = (DataTable)lds_taxResult;
            GetTaxinfoDetails();
            iwdg_TaxDetailsGrid.DataSource = lds_taxResult;
            iwdg_TaxDetailsGrid.DataBind();
            mpe_taxPopup.Show();
        }
        #endregion
    }
}