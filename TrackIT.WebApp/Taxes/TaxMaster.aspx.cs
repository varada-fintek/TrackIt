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
using DevExpress.Web;

namespace TrackIT.WebApp.Taxes
{
    public partial class TaxMaster : BasePage
    {
        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid iwdg_TaxMasterGrid;
        //WebDataGrid iwdg_TaxDetailsGrid;
        private static string istr_taxid = string.Empty;
        private static string istr_tablename = string.Empty;
        #endregion

        #region  Events

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                ControlNames();
                iwdg_TaxMasterGrid = new WebDataGrid();
                pnl_taxGrid.Controls.Add(iwdg_TaxMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_TaxMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(iwdg_taxdetails_grid);
   //             ((BoundCheckBoxField)this.WebDataGrid1.Columns["UnitPrice"]).ValueConverter = new DecimalBooleanConverter();
   
                GetTaxDetails();
                if (!IsPostBack)
                {
                    iwdg_taxdetails_grid.DataSource = GetDataSource();
                    iwdg_taxdetails_grid.DataBind();
                }
                DataSet lds_taxtype = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TYP' and cp.Active = 1 ORDER BY parameter_name");
                ddptaxestype.EditorControl.DataSource = lds_taxtype;
                ddptaxestype.EditorControl.ValueField = "Value";
                ddptaxestype.EditorControl.TextField = "TextValue";
                ddptaxestype.EditorControl.DataKeyFields = "Value";

                DataSet lds_taxapplied = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TAO' and cp.Active = 1 ORDER BY parameter_name");
                ddpappliedon.EditorControl.DataSource = lds_taxapplied;
                ddpappliedon.EditorControl.ValueField = "Value";
                ddpappliedon.EditorControl.TextField = "TextValue";
                ddpappliedon.EditorControl.DataKeyFields = "Value";

                if (!string.IsNullOrEmpty(hdntaxkey.Value) && hdnpop.Value == "1")
                {
                    Int64? lint_TaxID = Convert.ToInt64(hdntaxkey.Value.ToString());
                    txttaxcode.Enabled = false;
                    btnSave.Visible = bitEdit;
                   // GetTaxinfoDetails();
                    EditTaxDetails(lint_TaxID);
                    iwdg_taxdetails_grid.DataSource = GetDataSource();
                    istr_taxid = hdntaxkey.Value;
                    hdnpop.Value = string.Empty;
                    mpe_taxPopup.Show();
                }
                else
                {
                    iwdg_taxdetails_grid.DataSource = GetDataSource();
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
                InsertorUpdateTaxDetails();
                mpe_taxPopup.Show();
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region Rows Deleting in grid
        protected void iwdg_taxdetails_grid_RowsDeleting(object sender, RowDeletingEventArgs e)
        {
            string result = string.Empty;
            foreach (int key in e.Row.DataKey)
            {
                if (key.Equals("LONEP"))
                {
                    e.Cancel = true;
                    result = "Delete Failed";
                    break;
                }
                else
                {
                    result += key + " ";
                }
            }

            if (!e.Cancel)
            {
                result = string.Format(result.Trim());
            }

            iwdg_taxdetails_grid.CustomAJAXResponse.Message += result + "\n<br />";
        }
        #endregion

        #region Add Row click Event
        protected void lnkAddrow_Click1(object sender, EventArgs e)
        {

            DataTable ldt_taxdetails = new DataTable();
            ldt_taxdetails = (DataTable)this.Session["GridTable"];
            if (string.IsNullOrEmpty(istr_taxid))
            {
                DataRow toInsert = ldt_taxdetails.NewRow();
                // toInsert["tax_tax_details_key"] = DBNull.Value;
                toInsert["tax_from"] = DBNull.Value;
                toInsert["tax_to"] = DBNull.Value;
                toInsert["tax_percent"] = DBNull.Value;
                toInsert["tax_type"] = DBNull.Value;
                toInsert["tax_applied_on"] = DBNull.Value;
                ldt_taxdetails.Rows.InsertAt(toInsert, ldt_taxdetails.Rows.Count + 1);
            }
            else
            {

                DataRow toInsert = ldt_taxdetails.NewRow();
                // toInsert["tax_tax_details_key"] = DBNull.Value;
                toInsert["tax_from"] = DBNull.Value;
                toInsert["tax_to"] = DBNull.Value;
                toInsert["tax_percent"] = DBNull.Value;
                toInsert["tax_type"] = DBNull.Value;
                toInsert["tax_applied_on"] = DBNull.Value;
                ldt_taxdetails.Rows.InsertAt(toInsert, ldt_taxdetails.Rows.Count + 1);

            }
            ldt_taxdetails.AcceptChanges();
            this.Session["GridTable"] = (DataTable)ldt_taxdetails;
            iwdg_taxdetails_grid.DataSource = GetDataSource();
            iwdg_taxdetails_grid.DataBind();
            mpe_taxPopup.Show();
        }
        #endregion

        #region Intialize Row for Grids
        private void iwdg_TaxMasterGrid_InitializeRow(object sender, RowEventArgs e)
        {
            try
            {
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
                lblCreatetaxes.Text = RollupText("Taxes", "lblCreatetaxes");
                lbltaxname.Text = RollupText("Taxes", "lbltaxname");
                lbltaxcode.Text = RollupText("Taxes", "lbltaxcode");
                lnkAddrow.Text = RollupText("Taxes", "lnkAddrow");
                reqvtaxcode.ErrorMessage = RollupText("Taxes", "reqvtaxcode");
                reqvtxttaxname.ErrorMessage = RollupText("Taxes", "reqvtxttaxname");
                reqvtaxcodeUNQ.ErrorMessage = RollupText("Taxes", "reqvtaxcodeUNQ");
               

            }
            catch(Exception ex)
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
                iwdg_TaxMasterGrid.InitializeRow += iwdg_TaxMasterGrid_InitializeRow;
                iwdg_TaxMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 20;
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
                DataSet lds_taxdetailsinfo = ldbh_QueryExecutors.ExecuteDataSet("Select * from prj_taxes_details where tax_key='" + aint_TaxID + "'");
                
                DataTable ldt_taxdetails = new DataTable(lds_taxdetailsinfo.Tables[0].TableName);
                
                DataColumn ldc_taxdetailskey = new DataColumn("tax_details_PK");
                ldc_taxdetailskey.AutoIncrement = true;
                ldc_taxdetailskey.AutoIncrement = true;
                ldc_taxdetailskey.AutoIncrementStep = 1;
                ldc_taxdetailskey.AutoIncrementSeed = 1;
                ldt_taxdetails.Columns.Add(ldc_taxdetailskey);
                ldt_taxdetails.BeginLoadData();
                DataTableReader dtReader = new DataTableReader(lds_taxdetailsinfo.Tables[0]);
                ldt_taxdetails.Load(dtReader);
                ldt_taxdetails.EndLoadData();

                DataColumn[] PK = new DataColumn[1];
                PK[0] = ldt_taxdetails.Columns["tax_details_PK"];
                ldt_taxdetails.PrimaryKey = PK;
                this.Session["GridTable"] = ldt_taxdetails;
             
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion

        #region Get Taxdetails Grid Data Source
        private DataTable GetDataSource()
        {
                DataTable dataSource = null;

                if (Session["GridTable"] == null)
                {
                    dataSource = GetTable();
                    this.Session.Add("GridTable", dataSource);
                }
                else
                {
                    dataSource = (DataTable)this.Session["GridTable"];
                }
            return dataSource;
        }
        #endregion

        #region Create Initial Columns
        static DataTable GetTable()
        {
            // Here we create a DataTable with four columns.
            DataTable ldt_taxdetails = new DataTable();
            ldt_taxdetails.Columns.Add("tax_details_PK", typeof(int));
            ldt_taxdetails.Columns.Add("tax_from", typeof(DateTime));
            ldt_taxdetails.Columns.Add("tax_to", typeof(DateTime));
            ldt_taxdetails.Columns.Add("tax_percent", typeof(int));
            ldt_taxdetails.Columns.Add("tax_type", typeof(Int64));
            ldt_taxdetails.Columns.Add("tax_applied_on", typeof(Int64));
            ldt_taxdetails.Columns["tax_details_PK"].AutoIncrement = true;
            ldt_taxdetails.Columns["tax_details_PK"].AutoIncrementStep = 1;
            ldt_taxdetails.Columns["tax_details_PK"].AutoIncrementSeed = 1;
            ldt_taxdetails.Rows.Add(1, DBNull.Value,DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value);
           
            DataColumn[] PK = new DataColumn[1];
            PK[0] = ldt_taxdetails.Columns["tax_details_PK"];

            ldt_taxdetails.PrimaryKey = PK;
            return ldt_taxdetails;

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

                    DataTable ldt_griddata =(DataTable)this.Session["GridTable"];
                    ldt_griddata.AcceptChanges();
                    istr_tablename = "prj_taxes_details";
                    lbool_type = false;
                    foreach(DataRow lrow in ldt_griddata.Rows)
                    {
                       lstr_outMessage = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>
                            {
                                {"tax_key",Convert.ToInt64(lstr_id)},
                                {"tax_from",Convert.ToDateTime(lrow["tax_from"].ToString())},
                                {"tax_to",Convert.ToDateTime(lrow["tax_to"].ToString())},
                                {"tax_percent",lrow["tax_percent"].ToString()},
                                {"tax_type",Convert.ToInt64(lrow["tax_type"].ToString())},
                                {"tax_applied_on",Convert.ToInt64(lrow["tax_applied_on"].ToString())}
                            },lbool_type);
                    }
                }


                else
                {
                    lbool_type = false;

                    istr_tablename = "prj_taxes";
                    lstr_outMessage = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
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
                    ldbh_QueryExecutors.ExecuteNonQuery("Delete  from prj_taxes_details where tax_key ='"+Convert.ToInt64(hdntaxkey.Value)+"'");
                    istr_tablename = "prj_taxes_details";
                    DataTable ldt_griddata=(DataTable)this.Session["GridTable"];
                    ldt_griddata.AcceptChanges();
                   
                    foreach(DataRow lrow in ldt_griddata.Rows)
                    {
                    lstr_outMessage = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                        {
                            {"tax_key",Convert.ToInt64(hdntaxkey.Value)}, 
                            {"tax_from",Convert.ToDateTime(lrow["tax_from"].ToString())},
                            {"tax_to",Convert.ToDateTime(lrow["tax_to"].ToString())},
                            {"tax_percent",lrow["tax_percent"].ToString()},
                            {"tax_type",Convert.ToInt64(lrow["tax_type"].ToString())},
                            {"tax_applied_on",Convert.ToInt64(lrow["tax_applied_on"].ToString())}
                        },
                        lbool_type);

                     }
                }
                
                if (lstr_outMessage. Contains("SUCCESS"))
                {
                    GetTaxDetails();
                    SaveMessage();
                    ClearControls();
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

        #region Clear values in the Controls
        private void ClearControls()
        {
            try
            {
                txttaxcode.Text = string.Empty;
                txttaxname.Text = string.Empty;
                btnSave.Visible = bitAdd;
                this.Session["GridTable"] = null;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #endregion

        #endregion
      
    }
}