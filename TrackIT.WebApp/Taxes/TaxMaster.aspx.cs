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
        //WebDataGrid iwdg_Taxdetailsinfo;
        //WebDataGrid iwdg_TaxDetailsGrid;
        private static string istr_tablename = string.Empty;
        #endregion

        #region  Events

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_1
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_1 PageLoad");

                ControlNames();
                iwdg_TaxMasterGrid = new WebDataGrid();
                pnl_taxGrid.Controls.Add(iwdg_TaxMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxMasterGrid);

                //iwdg_Taxdetailsinfo = new WebDataGrid();
               pnl_taxdetailsGrid.Controls.Add(iwdg_Taxdetailsinfo);
              //  iwdg_TaxDetailsGrid.Visible = false;
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_Taxdetailsinfo);
                GetTaxDetails();
                if (!IsPostBack)
                {
                    //Unit Testing ID - TaxMaster.aspx.cs_2
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_2 PageLoad IsPostBack  ");

                    //GetTaxinfoDetails();

                    if (!string.IsNullOrEmpty(hdntaxkey.Value) && hdnpop.Value == "1")
                    {
                        //Unit Testing ID - TaxMaster.aspx.cs_3
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_3 Edit Tax Details popId and Unique ID" + hdntaxkey.Value + hdnpop.Value);
                        Int64? lint_TaxID = Convert.ToInt64(hdntaxkey.Value.ToString());
                        txttaxcode.Enabled = false;
                        btnSave.Visible = bitEdit;
                        GetTaxinfoDetails();
                        EditTaxDetails(lint_TaxID);
                        hdnpop.Value = string.Empty;
                        mpe_taxPopup.Show();
                    }
                }
               
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region button clear event

        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_4
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_4 button Clear_Click");

                ClearControls();
                mpe_taxPopup.Hide();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region btn save click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_5
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_5 validate Page and Insert/Update Tax" + Page.IsValid);
                InsertorUpdateTaxDetails();
                mpe_taxPopup.Show();
                reqvtaxcodeUNQ.Enabled = true;
                reqvtaxcodeUNQ.Visible = true;
                // Unit Testing ID - TaxMaster.aspx.cs_6
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_6 Tax code Unique check");
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region add row 
        protected void lnkAddrow_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_7
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_7 lnkAddrow_Click");
  
                // GetTaxinfoDetails();

                
                DataSet lds_taxResult = (DataSet)ViewState["vsTaxdetails"];
                DataRow toInsert = lds_taxResult.Tables[0].NewRow();
                //toInsert["tax_from"] = DateTime.Now;
                //toInsert["tax_to"] = DateTime.Now;
                //toInsert["tax_percent"] = string.Empty;
                //toInsert["tax_type"] = 0;
                //toInsert["tax_applied_on"] = 0;
                lds_taxResult.Tables[0].Rows.InsertAt(toInsert, lds_taxResult.Tables[0].Rows.Count + 1);
                ViewState["vsTaxdetails"] = (DataSet)lds_taxResult;
                GetTaxinfoDetails();
                //iwdg_TaxDetailsGrid.DataSource = lds_taxResult;
                //iwdg_TaxDetailsGrid.DataBind();
                mpe_taxPopup.Show();
                
                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        
        #endregion

        #region Intialize Row for Grids
        private void iwdg_TaxDetailsGrid_InitializeRow(object sender, RowEventArgs e)
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_8
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_8 intialize grid row " + e.Row.Index);

                if (e.Row.Index == 0)
                {
                    e.Row.Items.FindItemByKey("tax_from").Column.Header.Text = RollupText("Taxes", "detailsgridtaxfrom");
                    e.Row.Items.FindItemByKey("tax_to").Column.Header.Text = RollupText("Taxes", "detailsgridtaxto");
                    e.Row.Items.FindItemByKey("tax_percent").Column.Header.Text = RollupText("Taxes", "detailsgridtaxpercent");
                    e.Row.Items.FindItemByKey("tax_type").Column.Header.Text = RollupText("Taxes", "detailsgridtaxtype");
                    e.Row.Items.FindItemByKey("tax_applied_on").Column.Header.Text = RollupText("Taxes", "detailsgridtaxappliedon");
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

        private void iwdg_TaxMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_9
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_9 intialize grid row " + e.Row.Index);

                //throw new NotImplementedException();
                if (e.Row.Index == 0)
                {
                    e.Row.Items.FindItemByKey("tax_key").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("tax_tax_code").Column.Header.Text = RollupText("Taxes", "gridtaxcode");
                    e.Row.Items.FindItemByKey("tax_tax_name").Column.Header.Text = RollupText("Taxes", "gridtaxname");
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #endregion

        #region User Defined Functions

        #region Control Names
        private void ControlNames()
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_10
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_10 ControlNames");

                lblCreatetaxes.Text = RollupText("Taxes", "lblCreatetaxes");
                lbltaxname.Text = RollupText("Taxes", "lbltaxname");
                lbltaxcode.Text = RollupText("Taxes", "lbltaxcode");
                lnkAddrow.Text = RollupText("Taxes", "lnkAddrow");
                reqvtaxcode.ErrorMessage = RollupText("Taxes", "reqvtaxcode");
                reqvtxttaxname.ErrorMessage = RollupText("Taxes", "reqvtxttaxname");
                reqvtaxcodeUNQ.ErrorMessage = RollupText("Taxes", "reqvtaxcodeUNQ");
               
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region Get Tax Details
        private void GetTaxDetails()
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_10
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_10 GetTaxDetails");

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
                //Unit Testing ID - TaxMaster.aspx.cs_11
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_11 Edit Tax Details" + aint_TaxID);
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
                //Unit Testing ID - TaxMaster.aspx.cs_12
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_12 GetTaxinfoDetails");

                    DataSet lds_taxResult;
                    lds_taxResult = ldbh_QueryExecutors.ExecuteDataSet("select tax_from,tax_to,tax_percent,tax_type,tax_applied_on from prj_taxes_details");
                    
                    if (lds_taxResult.Tables[0].Rows.Count > 0)
                    {
                        //lds_taxResult.Tables[0].Columns[0].DataType = typeof(System.DateTime);
                       // DataTable ldt_sessiontax=(DataTable)ViewState["vsTaxdetails"];
                        if (ViewState["vsTaxdetails"]!=null)
                        {
                            //Unit Testing ID - TaxMaster.aspx.cs_13
                            System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_13 Taxdetailsinfo dataset count" + lds_taxResult.Tables[0].Rows.Count);

                            iwdg_Taxdetailsinfo.Columns.Clear();
                            iwdg_Taxdetailsinfo.DataSource = null;
                           lds_taxResult.Tables.Remove(lds_taxResult.Tables[0]);
                           lds_taxResult = (DataSet)ViewState["vsTaxdetails"];
                           lds_taxResult.Tables[0].Columns[0].DataType = typeof(System.DateTime);
                           iwdg_Taxdetailsinfo.DataSource = lds_taxResult;
                           iwdg_Taxdetailsinfo.DataBind();
                           ViewState["vsTaxdetails"] = (DataSet)lds_taxResult;
                        }
                        else
                        {
                            //Unit Testing ID - TaxMaster.aspx.cs_14
                            System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_14 Taxdetailsinfo dataset count" + lds_taxResult.Tables[0].Rows.Count);

                            iwdg_Taxdetailsinfo.DataSource = lds_taxResult.Tables[0];
                            ViewState["vsTaxdetails"] = (DataSet)lds_taxResult;
                            iwdg_Taxdetailsinfo.InitializeRow += iwdg_TaxDetailsGrid_InitializeRow;
                            iwdg_Taxdetailsinfo.DataSource = lds_taxResult;
                            iwdg_Taxdetailsinfo.DataBind();
                        }
                    }

                    iwdg_Taxdetailsinfo.InitializeRow+=iwdg_TaxDetailsGrid_InitializeRow;
                    // Enable cell editing
                    this.iwdg_Taxdetailsinfo.Behaviors.CreateBehavior<EditingCore>();
                    this.iwdg_Taxdetailsinfo.Behaviors.EditingCore.Behaviors.CreateBehavior<CellEditing>();
                    this.iwdg_Taxdetailsinfo.EditorProviders.Add(FromdateProvider);
                    this.iwdg_Taxdetailsinfo.EditorProviders.Add(TodateProvider);
                    this.iwdg_Taxdetailsinfo.EditorProviders.Add(Taxpercentage);
                    this.iwdg_Taxdetailsinfo.EditorProviders.Add(TaxtypeProvider);
                    this.iwdg_Taxdetailsinfo.EditorProviders.Add(TaxappliedonProvider);

                    EditingColumnSetting fromdatecolumn = new EditingColumnSetting();
                    fromdatecolumn.ColumnKey = "tax_from";
                    
                    fromdatecolumn.EditorID = FromdateProvider.ID;

                    EditingColumnSetting todatecolumn = new EditingColumnSetting();
                    todatecolumn.ColumnKey = "tax_to";
                    todatecolumn.EditorID = TodateProvider.ID;

                    EditingColumnSetting taxpercentcolumn = new EditingColumnSetting();
                    taxpercentcolumn.ColumnKey = "tax_percent";
                    taxpercentcolumn.EditorID = Taxpercentage.ID;


                    this.iwdg_Taxdetailsinfo.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(fromdatecolumn);
                    this.iwdg_Taxdetailsinfo.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(todatecolumn);
                    this.iwdg_Taxdetailsinfo.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(taxpercentcolumn);

                    DataSet lds_taxtyperesult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TYP' and cp.Active = 1 ORDER BY parameter_name");
                    this.iwdg_Taxdetailsinfo.EditorProviders.Add(TaxtypeProvider);
                    EditingColumnSetting taxtypecolumn = new EditingColumnSetting();
                    taxtypecolumn.ColumnKey = "tax_type";
                    taxtypecolumn.EditorID = TaxtypeProvider.ID;
                    TaxtypeProvider.EditorControl.ValueField = "Value";
                    TaxtypeProvider.EditorControl.TextField = "TextValue";
                    TaxtypeProvider.EditorControl.DataSource = lds_taxtyperesult.Tables[0];
                    this.iwdg_Taxdetailsinfo.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(taxtypecolumn);

                    DataSet lds_taxappliedonresult = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TAO' and cp.Active = 1 ORDER BY parameter_name");
                    this.iwdg_Taxdetailsinfo.EditorProviders.Add(TaxtypeProvider);
                    EditingColumnSetting taxappliedoncolumn = new EditingColumnSetting();
                    taxappliedoncolumn.ColumnKey = "tax_applied_on";
                    taxappliedoncolumn.EditorID = TaxappliedonProvider.ID;
                    TaxappliedonProvider.EditorControl.ValueField = "Value";
                    TaxappliedonProvider.EditorControl.TextField = "TextValue";
                    TaxappliedonProvider.EditorControl.DataSource = lds_taxappliedonresult.Tables[0];
                    this.iwdg_Taxdetailsinfo.Behaviors.EditingCore.Behaviors.CellEditing.ColumnSettings.Add(taxappliedoncolumn);
                    iwdg_Taxdetailsinfo.DataBind();
            }
           
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region InsertorUpdate
        protected void InsertorUpdateTaxDetails()
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_15
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_15 InsertUpdate TaxDetails");

                string astr_tax_key = Convert.ToString(Guid.NewGuid());
                istr_tablename = "prj_taxes";
                Boolean lbool_type = true;
                string lstr_outMessage = string.Empty;
                if (string.IsNullOrEmpty(hdntaxkey.Value))
                {
                    //Unit Testing ID - TaxMaster.aspx.cs_16
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_16 Insert Taxdetails" + hdntaxkey.Value);

                    string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object> 
                    {
                        {"tax_tax_name",txttaxname.Text.Replace("'","''")},
                        {"tax_tax_code",txttaxcode.Text.Replace("'","''")},
                    }, lbool_type
                    );
                }
                    

            
                

                    
                else
                {
                    //Unit Testing ID - TaxMaster.aspx.cs_17
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_17 Update Taxdetails" + hdntaxkey.Value);

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
                    //Unit Testing ID - TaxMaster.aspx.cs_18
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_18 success Measage" + lstr_outMessage);

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
       // protected void InsertorUpdateTaxDetailsinfo()
       // {
            //try
           // {
                //Unit Testing ID - TaxMaster.aspx.cs_15
               // System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_15 InsertUpdateTax Details");

               // string astr_tax_key = Convert.ToString(Guid.NewGuid());
               // istr_tablename = "prj_taxes_details";
               // Boolean lbool_type = true;
               // string lstr_outMessage = string.Empty;
               // if (string.IsNullOrEmpty(hdntaxkey.Value))
               // {
                    //Unit Testing ID - TaxMaster.aspx.cs_16
                   // System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_16 Insert Taxdetailsinfo" + hdntaxkey.Value);

                   // string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object> 
                   // {
                       // {"tax_from",txttaxname.Text.Replace("'","''")},
                      //  {"tax_to",txttaxcode.Text.Replace("'","''")},
                   // }, lbool_type
                   // );
               // }






               // else
               // {
                    //Unit Testing ID - TaxMaster.aspx.cs_17
                   // System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_17 Update Taxdetails" + hdntaxkey.Value);

                   // lbool_type = false;
        
                   // istr_tablename = "prj_taxes";
                   // string id = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                   // {
                      //  {"tax_tax_name",txttaxname.Text.Replace("'", "''") },
                       // {"tax_tax_code",txttaxcode.Text.Replace("'", "''") }
                   // },
                   // new System.Collections.Generic.Dictionary<string, object>()
                    // {
                     //    {"tax_key", hdntaxkey.Value},
                    // },
                   // lbool_type
                  // );

                   // lstr_outMessage = "SUCCESS";
               // }

               // if (lstr_outMessage.Contains("SUCCESS"))
               // {
                    //Unit Testing ID - TaxMaster.aspx.cs_18
                   // System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_18 success Measage" + lstr_outMessage);

                   // string[] sBUID = lstr_outMessage.Split('^');
                   // GetTaxDetails();
                   // SaveMessage();
                    // ClearControls();
                   // mpe_taxPopup.Hide();
                   // return;
               // }
//else
               // {


                  //  Response.Redirect("~/Taxes/TaxMaster.aspx", false);
                //}



           // }
           // catch (Exception ex)
            //{
               // if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
           //         throw;
           // }
       // }
        #endregion

        #region Clear controls
        private void ClearControls()
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_19
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_19 ClearControls");
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

        


        protected void iwdg_Taxdetailsinfo_RowAdded(object sender, RowAddedEventArgs e)
        {

        }

       

        protected void iwdg_Taxdetailsinfo_RowAdding(object sender, RowAddingEventArgs e)
        {

        }
        #endregion
        #region Export to Excel And PDF
        /// <summary>
        /// Button Export Excel click Event
        /// </summary>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - TaxMaster.aspx.cs_20
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_20 Export Excel");

                DataTable ldt_ExcelExp = (DataTable)ViewState["export"];
                iwdg_TaxMasterGrid.DataSource = ldt_ExcelExp;
                iwdg_TaxMasterGrid.DataBind();
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebExcelExporter.Export(iwdg_TaxMasterGrid);
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebExcelExporter.Export(this.iwdg_TaxMasterGrid);

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
                //Unit Testing ID - TaxMaster.aspx.cs_21
                System.Diagnostics.Debug.WriteLine("Unit testing ID - TaxMaster.aspx.cs_21 ExportPdf");
                DataTable ldt_PdfExp = (DataTable)ViewState["export"];
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxMasterGrid);
                iwdg_TaxMasterGrid.DataSource = ldt_PdfExp;
                iwdg_TaxMasterGrid.DataBind();
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebPDFExporter.Export(iwdg_TaxMasterGrid);
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebPDFExporter.Export(this.iwdg_TaxMasterGrid);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                // Response.Redirect("~/Error.aspx", false);
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