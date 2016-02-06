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


namespace TrackIT.WebApp.Setup
{
    public partial class UserMaster : BasePage
    {

        #region Declarations
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        WebDataGrid lwdg_UserMasterGrid;
        private  byte[] ibyt_Password { get; set; }
        private string istr_EncryptionKey= Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["EncryptionKey"]);
        private static string istr_tablename = string.Empty;
        #endregion

        #region Page Load
        /// <summary>
        /// Page Load Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - UserMaster.aspx.cs_1
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_1 PageLoad");

                ControlNames();
                lwdg_UserMasterGrid = new WebDataGrid();
                pnl_UserGrid.Controls.Add(lwdg_UserMasterGrid);
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_UserMasterGrid);
                if (!IsPostBack)
                {
                    //Unit Testing ID - UserMaster.aspx.cs_2
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_2 PageLoad IsPostBack  ");
                    //Assign all dropdown data Roles Drop Down
                    DataSet lds_role = ldbh_QueryExecutors.ExecuteDataSet("SELECT Role_ID AS [Value], Role_Name AS TextValue FROM prj_roles (NOLOCK) WHERE Active = 1 ORDER BY Role_Name");
                    if (lds_role.Tables[0].Rows.Count > 0)
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_3
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_3 Roles dataset count"+lds_role.Tables[0].Rows.Count);
                        ddlRole.Items.Clear();
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlRole.DataSource = lds_role;
                        ddlRole.DataTextField = "TextValue";
                        ddlRole.DataValueField = "Value";
                        ddlRole.DataBind();
                        ddlRole.Items.Insert(0, li);
                    }

                    //Assign all dropdown data User Location Drop Down
                    DataSet lds_userLocation = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='LOC' and cp.Active = 1 ORDER BY parameter_name");
                    if (lds_userLocation.Tables[0].Rows.Count > 0)
                    { 
                        //Unit Testing ID - UserMaster.aspx.cs_4
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_4 userLoaction dataset count" + lds_userLocation.Tables[0].Rows.Count);
                        ddlUserLocation.Items.Clear();
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlUserLocation.DataSource = lds_userLocation;
                        ddlUserLocation.DataTextField = "TextValue";
                        ddlUserLocation.DataValueField = "Value";
                        ddlUserLocation.DataBind();
                        ddlUserLocation.Items.Insert(0, li);
                    }

                    //Assign all dropdown datar User Title Drop Down
                    DataSet lds_title = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TIT' and cp.Active = 1 ORDER BY parameter_name");
                    if (lds_title.Tables[0].Rows.Count > 0)
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_5
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_5 usertitle dataset count" + lds_title.Tables[0].Rows.Count);
                        ddlUserTitle.Items.Clear();
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlUserTitle.DataSource = lds_title;
                        ddlUserTitle.DataTextField = "TextValue";
                        ddlUserTitle.DataValueField = "Value";
                        ddlUserTitle.DataBind();
                        ddlUserTitle.Items.Insert(0, li);
                    }

                    //Assign all dropdown data User Timezone Drop Down
                    DataSet lds_timeZone = ldbh_QueryExecutors.ExecuteDataSet("SELECT cp.parameter_key AS [Value],cp.parameter_name AS TextValue FROM com_parameters cp (NOLOCK) inner join com_parameter_type cpt on cpt.parameter_type_code=cp.parameter_type WHERE cpt.parameter_type_code='TZO' and cp.Active = 1 ORDER BY parameter_name");
                    if (lds_timeZone.Tables[0].Rows.Count > 0)
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_6
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_6 usertimezone dataset count" + lds_timeZone.Tables[0].Rows.Count);
                        ddlUserTimeZone.Items.Clear();
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem("Select", "");
                        ddlUserTimeZone.DataSource = lds_timeZone;
                        ddlUserTimeZone.DataTextField = "TextValue";
                        ddlUserTimeZone.DataValueField = "Value";
                        ddlUserTimeZone.DataBind();
                        ddlUserTimeZone.Items.Insert(0, li);
                    }
                    //GetUserDetails();
                    ClearControls();
                }
                GetUserDetails();
                if (!string.IsNullOrEmpty(hdnUserID.Value) && hdnpop.Value == "1")
                {
                    //Edit User Details
                    //Unit Testing ID - UserMaster.aspx.cs_7
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_7 Edit User Details popId and Unique ID" + hdnUserID.Value + hdnpop.Value); 
                    Int64? userid = Convert.ToInt64(hdnUserID.Value.ToString());
                    EnableDisableControls(true);
                    btnSave.Visible = bitEdit;
                    EditUserDetails(userid);
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                    mpe_UserPopup.Show();
                }

            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #region "Post Back Events"

        #region Save Button Click
        /// <summary>
        /// Save Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                 DataSet lds_Result;
                 if (!string.IsNullOrEmpty(hdnUserID.Value))
                {
                    lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select User_Name from Security_Users where active=1 and Users_Key<>'" + hdnUserID.Value + "' and User_Name='" + txtUserID.Text + "'");
                    if (lds_Result.Tables[0].Rows.Count > 0)
                    {
                        reqvuserIdUNQ.ErrorMessage = RollupText("UserMaster", "reqvUnqUserID");
                        reqvuserIdUNQ.Enabled = true;
                        reqvuserIdUNQ.Visible = true;
                        ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + RollupText("UserMaster", "reqvUnqUserID") + "')</script>", false);
                       // Unit Testing ID - UserMaster.aspx.cs_17
                       System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_17 User Id Unique check");
                        mpe_UserPopup.Show();
                    }
                    else
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_9
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_9 validate Page and Insert/Update User" + Page.IsValid);
                        InsertorUpdateUserDetails();
                        //Unit Testing ID - UserMaster.aspx.cs_10
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_10 Pagevalidation Fails" + Page.IsValid);
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                    }
                }
                else
                {
                     lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select User_Name from Security_Users where active=1 and User_Name='" + txtUserID.Text + "'");
                    if (lds_Result.Tables[0].Rows.Count > 0)
                    {
                        reqvuserIdUNQ.ErrorMessage = RollupText("UserMaster", "reqvUnqUserID");
                        ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + RollupText("UserMaster", "reqvUnqUserID") + "')</script>", false);
                        mpe_UserPopup.Show();
                        reqvuserIdUNQ.Enabled = true;
                        reqvuserIdUNQ.Visible = true;
                        // Unit Testing ID - UserMaster.aspx.cs_17
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_17 User Id Unique check");
                    }
                    else
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_9
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_9 validate Page and Insert/Update User" + Page.IsValid);
                        InsertorUpdateUserDetails();
                        //Unit Testing ID - UserMaster.aspx.cs_10
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_10 Pagevalidation Fails" + Page.IsValid);
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "show();", true);
                    }
                }
                //Unit Testing ID - UserMaster.aspx.cs_8
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_8 SaveButtonClick");
                
                
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion
        

        #region Clear Button Click
        /// <summary>
        /// Clear page,Controls and popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - UserMaster.aspx.cs_11
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_11 bUtton Clear_Click");
                EnableDisableControls(true);
                //GetUserDetails();
                ClearControls();
                mpe_UserPopup.Hide();
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }
        #endregion

        #endregion

        #region "User Defined Functions"

        #region InsertorUpdateUserDetails
        protected void InsertorUpdateUserDetails()
        {
            try
            {
                
                //Unit Testing ID - UserMaster.aspx.cs_12
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_12 InsertUpdateUser Details");
                
               // if (StringFunctions.IsNullOrEmpty(objUserBO)) objUserBO = new UserMasterBo();
                ibyt_Password  = Crypto.CreateHash(txtConfrimPass.Text);
                string lstr_user_id = Convert.ToString(Guid.NewGuid());
               
                string lstr_outMessage = string.Empty;
                // hdnUserID to check weather Insert / Update.
                if (string.IsNullOrEmpty(hdnUserID.Value))
                {

                    //Unit Testing ID - UserMaster.aspx.cs_13
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_13 Insert"+hdnUserID.Value);
                    string astr_Users_ID = Convert.ToString(Guid.NewGuid());

                    istr_tablename = "prj_users";
                    string lstr_id = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                    {
                       // {"user_id", lstr_user_id},
                        {"user_first_name", txtuserfirstname.Text.Replace("'", "''")},
                        {"user_middle_name", txtusersmidlename.Text.Replace("'", "''")},
                        {"user_last_name", txtuserlastname.Text.Replace("'", "''")},
                        {"user_phone", txtphone.Text.Replace("'", "''")},
                        {"user_email", txtmailID.Text.Replace("'", "''")},
                        {"user_location", ddlUserLocation.SelectedValue},
                        {"user_password", Fintek.EncryptorDecryptor.Encrypt(txtPassword.Text, istr_EncryptionKey)},
                        {"user_title", ddlUserTitle.SelectedValue},
                        {"user_timezone_id", ddlUserTimeZone.SelectedValue},
                        {"user_role_key", ddlRole.SelectedValue},
                        {"Active", (chkinactive.Checked ? 1 : 0).ToString()},
                        {"Created_By", this.LoggedInUserId },
                        {"Created_Date", DateTime.Now},
                        {"last_modified_By", this.LoggedInUserId },
                        {"last_modified_date", DateTime.Now}
                      }
                           , "scope");

                    //insert User for Login 
                    istr_tablename = "Security_Users";
                    string lstr_returnmsg = ldbh_QueryExecutors.SqlInsert(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                     {
                   // {"Users_ID", astr_Users_ID},
                    {"User_Name",txtUserID.Text },
                    {"Password ", ibyt_Password },
                    {"Role_ID", ddlRole.SelectedValue},
                    {"User_Type", "OTH"},
                    {"Super_User", "0"},
                    {"Users_Key", lstr_id},
                    {"Active",(chkinactive.Checked ? 1 : 0).ToString()},
                    {"Created_By", this.LoggedInUserId },
                    {"Created_Date", DateTime.Now},
                    {"Is_First_Login", "0"},
                    {"last_modified_by", this.LoggedInUserId },
                    {"last_modified_date", DateTime.Now}
                       }, "nonQuery");
                    

                    
                     lstr_outMessage = "SUCCESS";

                }
                // Update User Login and User Details.
                else
                {

                    //Unit Testing ID - UserMaster.aspx.cs_14
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_14 Update User"+hdnUserID.Value);
                    istr_tablename = "Security_Users";
                    string msg = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                {
                   
                    {"User_Name",txtUserID.Text },
                    {"Password", ibyt_Password },
                    {"Role_ID", ddlRole.SelectedValue},
                    {"Active", (chkinactive.Checked ? 1 : 0).ToString()},
                },
                    new System.Collections.Generic.Dictionary<string, object>()
                     {
                         {"Users_Key", hdnUserID.Value},
                     },
                    "nonQuery");

                    istr_tablename = "prj_users";
                    string id = ldbh_QueryExecutors.SqlUpdate(istr_tablename, new System.Collections.Generic.Dictionary<string, object>()
                {
                    {"user_first_name", txtuserfirstname.Text.Replace("'", "''")},
                    {"user_middle_name", txtusersmidlename.Text.Replace("'", "''")},
                    {"user_last_name", txtuserlastname.Text.Replace("'", "''")},
                    {"user_phone", txtphone.Text.Replace("'", "''")},
                    {"user_email", txtmailID.Text.Replace("'", "''")},
                    {"user_location", ddlUserLocation.SelectedValue},
                    {"user_password", Fintek.EncryptorDecryptor.Encrypt(txtPassword.Text, istr_EncryptionKey)},
                    {"user_title", ddlUserTitle.SelectedValue},
                    {"user_timezone_id", ddlUserTimeZone.SelectedValue},
                    {"user_role_key", ddlRole.SelectedValue},
                    {"Active", (chkinactive.Checked ? 1 : 0).ToString()},
                    {"last_modified_date", DateTime.Now}
                     },
                         new System.Collections.Generic.Dictionary<string, object>()
                     {
                         {"user_id", hdnUserID.Value},
                     },
                         "nonscope");

                    lstr_outMessage = "SUCCESS";
                }
                

                //Sucess Message After Insert/Update
                if (lstr_outMessage.Contains("SUCCESS"))
                {

                    //Unit Testing ID - UserMaster.aspx.cs_15
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_15 success Measage" + lstr_outMessage);
                    string[] sBUID = lstr_outMessage.Split('^');
                    GetUserDetails();
                    SaveMessage();
                    ClearControls();
                    mpe_UserPopup.Hide();
                    return;
                }
                else
                {

                    //Unit Testing ID - UserMaster.aspx.cs_16
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_16 ErrorMessage");
                    Response.Redirect("~/Setup/UserMaster.aspx", false);
                }
            }
            catch (Exception ex)
            {
               
                    if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                        throw;
            }
            finally
            {
            }
        }
        #endregion

        #region GetUserDetails
        /// <summary>
        /// Get User Details
        /// </summary>
        private void GetUserDetails()
        {
            try
            {

                //Unit Testing ID - UserMaster.aspx.cs_18
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_18 GetUserDetails");
                //if (StringFunctions.IsNullOrEmpty(objUserBO)) objUserBO = new UserMasterBo();
                lwdg_UserMasterGrid.InitializeRow+=lwdg_UserMasterGrid_InitializeRow;
                lwdg_UserMasterGrid.Columns.Clear();
                TemplateDataField td = new TemplateDataField();
                td.ItemTemplate = new CustomItemTemplateView();
                td.Key = "Action";
                td.Width = 30;
                lwdg_UserMasterGrid.Columns.Add(td);
                //Query to Get Landing Page Grid Details
                DataSet lds_Result;
                lds_Result = ldbh_QueryExecutors.ExecuteDataSet("select sm.user_id,su.User_Name,sm.user_first_name,sm.user_middle_name,sm.user_last_name,sr.Role_Name,cp1.parameter_name as User_Title,sm.user_phone,sm.user_email,sm.user_location,cp.parameter_type,cp.parameter_code,cp.parameter_name, sm.Active from prj_users sm inner join prj_roles sr on sr.Role_ID=sm.user_role_key inner join com_parameters cp on cp.parameter_key= sm.user_location inner join com_parameters cp1 on  sm.user_title=cp1.parameter_key  inner join Security_Users su on su.Users_Key=sm.user_id ");
                lwdg_UserMasterGrid.Visible = false;
               
                if (lds_Result != null)
                {
                    if (lds_Result.Tables.Count > 0)
                    {
                        //Unit Testing ID - UserMaster.aspx.cs_19
                         System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_19 lds_Result set"+ lds_Result.Tables.Count);
                        if (lds_Result.Tables[0].Rows.Count > 0)
                        {
                            ViewState["export"]= (DataTable)lds_Result.Tables[0];
                            lwdg_UserMasterGrid.DataSource = lds_Result.Tables[0];
                            lwdg_UserMasterGrid.DataBind();
                            DataColumn[] keyColumns = new DataColumn[1];
                            DataTable ldt_dt = lds_Result.Tables[0];
                            lwdg_UserMasterGrid.DataKeyFields = "user_id";
                            keyColumns[0] = ldt_dt.Columns["user_id"];
                            ldt_dt.PrimaryKey = keyColumns;
                            lwdg_UserMasterGrid.Visible = true;
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
        #endregion

        #region initializerow for user grid
        /// <summary>
        /// initialize Each row in the grid and Choose columns to display
        /// </summary>
        protected void lwdg_UserMasterGrid_InitializeRow(object sender, Infragistics.Web.UI.GridControls.RowEventArgs e)
        {
            try
            {
                //Unit Testing ID - UserMaster.aspx.cs_20
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_20 intialize grid row " + e.Row.Index);
                if (e.Row.Index == 0)
                {

                    //Unit Testing ID - UserMaster.aspx.cs_21
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_21 row Index" + e.Row.Items.Count);
                    e.Row.Items.FindItemByKey("user_id").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("parameter_type").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("parameter_code").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("user_location").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("user_middle_name").Column.Hidden = true;
                    e.Row.Items.FindItemByKey("User_Name").Column.Header.Text = RollupText("UserMaster", "gridUserName");
                    e.Row.Items.FindItemByKey("User_Name").Column.CssClass = "Dataalign";
                    e.Row.Items.FindItemByKey("user_first_name").Column.Header.Text = RollupText("UserMaster", "gridFirstName");
                    e.Row.Items.FindItemByKey("user_first_name").Column.CssClass = "Dataalign";
                    e.Row.Items.FindItemByKey("user_last_name").Column.Header.Text = RollupText("UserMaster", "gridlastname");
                    e.Row.Items.FindItemByKey("Role_Name").Column.Header.Text = RollupText("UserMaster", "gridRoleName");
                    e.Row.Items.FindItemByKey("User_Title").Column.Header.Text = RollupText("UserMaster", "gridTitle");
                    e.Row.Items.FindItemByKey("user_phone").Column.Header.Text = RollupText("UserMaster", "gridphone");
                    e.Row.Items.FindItemByKey("user_email").Column.Header.Text = RollupText("UserMaster", "gridemail");
                    e.Row.Items.FindItemByKey("parameter_name").Column.Header.Text = RollupText("UserMaster", "gridLocation");
                    e.Row.Items.FindItemByKey("Active").Column.Header.Text = RollupText("Common", "gvActive");
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
                                lwdg_UserMasterGrid.Behaviors.Filtering.ColumnFilters.Add(filter);
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

        #region EditUserDetails on Edit
        /// <summary>
        /// Get User Details on Edit
        /// </summary>
        private void EditUserDetails(Int64? guidBUID)
        {
            try
            {

                //Unit Testing ID - UserMaster.aspx.cs_23
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_23 GetUser Details Edit" +guidBUID);
               // if (StringFunctions.IsNullOrEmpty(objUserBO)) objUserBO = new UserMasterBo();
                Int64? BU_ID = guidBUID;
                string lstr_password;
                //Fetch Single Record from table and assign to Edit
                DataSet lds_userdetail = ldbh_QueryExecutors.ExecuteDataSet("select * from prj_users smu inner join Security_Users su on su.Users_Key=smu.user_id where smu.user_id='" + guidBUID + "'");
                if (lds_userdetail.Tables[0].Rows.Count > 0)
                {
                    if (lds_userdetail.Tables[0].Rows[0]["user_location"] != null)
                    {
                        if (ddlUserLocation.Items.FindByValue((lds_userdetail.Tables[0].Rows[0]["user_location"]).ToString().ToLower()) != null)
                        {
                            ddlUserLocation.SelectedValue = lds_userdetail.Tables[0].Rows[0]["user_location"].ToString();
                        }
                        else
                            ddlUserLocation.SelectedIndex = 0;
                    }
                    else
                        ddlUserLocation.SelectedIndex = 0;

                    if (lds_userdetail.Tables[0].Rows[0]["user_timezone_id"] != null)
                    {
                        if (ddlUserTimeZone.Items.FindByValue((lds_userdetail.Tables[0].Rows[0]["user_timezone_id"]).ToString().ToLower()) != null)
                        {
                            ddlUserTimeZone.SelectedValue = lds_userdetail.Tables[0].Rows[0]["user_timezone_id"].ToString();
                        }
                        else
                            ddlUserTimeZone.SelectedIndex = 0;
                    }
                    else
                        ddlUserTimeZone.SelectedIndex = 0;

                    if (lds_userdetail.Tables[0].Rows[0]["user_title"] != null)
                    {
                        if (ddlUserTitle.Items.FindByValue((lds_userdetail.Tables[0].Rows[0]["user_title"]).ToString().ToLower()) != null)
                        {
                            ddlUserTitle.SelectedValue = lds_userdetail.Tables[0].Rows[0]["user_title"].ToString();
                        }
                        else
                            ddlUserTitle.SelectedIndex = 0;
                    }
                    else
                        ddlUserTitle.SelectedIndex = 0;

                    if (lds_userdetail.Tables[0].Rows[0]["user_role_key"] != null)
                    {
                        if (ddlRole.Items.FindByValue((lds_userdetail.Tables[0].Rows[0]["user_role_key"]).ToString().ToLower()) != null)
                        {
                            ddlRole.SelectedValue = lds_userdetail.Tables[0].Rows[0]["user_role_key"].ToString();
                        }
                        else
                            ddlRole.SelectedIndex = 0;
                    }
                    else
                        ddlRole.SelectedIndex = 0;

                    hdnUserID.Value = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_id"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_id"]).Trim() : string.Empty;
                    txtUserID.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["User_Name"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["User_Name"]).Trim() : string.Empty;
                    txtuserfirstname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_first_name"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_first_name"]).Trim() : string.Empty;
                    txtuserfirstname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_first_name"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_first_name"]).Trim() : string.Empty;
                    txtuserlastname.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_last_name"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_last_name"]).Trim() : string.Empty;
                    txtusersmidlename.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_middle_name"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_middle_name"]).Trim() : string.Empty;
                    lstr_password = Fintek.EncryptorDecryptor.Decrypt((lds_userdetail.Tables[0].Rows[0]["user_password"]).ToString(), istr_EncryptionKey);
                    txtPassword.Text = (!string.IsNullOrEmpty(lstr_password)) ? lstr_password.Trim() : string.Empty;
                    txtConfrimPass.Text = (!string.IsNullOrEmpty(lstr_password)) ? lstr_password.Trim() : string.Empty;
                    txtmailID.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_email"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_email"]).Trim() : string.Empty;
                    txtphone.Text = (!string.IsNullOrEmpty(Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_phone"]))) ? Convert.ToString(lds_userdetail.Tables[0].Rows[0]["user_phone"]).Trim() : string.Empty;
                    chkinactive.Checked = Convert.ToInt32(lds_userdetail.Tables[0].Rows[0]["Active"]) == 1 ? true : false;
                    chkinactive.Enabled = true;
                   
                }
                else
                {
                    ClearControls();
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
        #endregion

        #region EnableDisableControls
        private void EnableDisableControls(bool bValue)
        {
            try
            {
                //Unit Testing ID - UserMaster.aspx.cs_24
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_24 EnableDiasble Controls");

                txtuserfirstname.Enabled = bValue;
                txtuserfirstname.Enabled = bValue;
                txtusersmidlename.Enabled = bValue;
                txtuserlastname.Enabled = bValue; ;
                ddlUserLocation.Enabled = bValue; ;
                ddlUserTimeZone.Enabled = bValue;
                ddlUserTitle.Enabled = bValue;
                txtmailID.Enabled = bValue;
                txtphone.Enabled = bValue;
                txtConfrimPass.Enabled = bValue;
                txtPassword.Enabled = bValue;
                ddlRole.Enabled = bValue;
                chkinactive.Enabled = bValue;
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region ClearControls
        /// <summary>
        /// Clear Each Control in the Screen on PageLoad
        /// </summary>
        private void ClearControls()
        {
            try
            {

                //Unit Testing ID - UserMaster.aspx.cs_24
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_24 ClearControls");
                hdnUserID.Value = string.Empty;
                txtUserID.Text = string.Empty;
                txtuserfirstname.Text = string.Empty;
                txtusersmidlename.Text = string.Empty;
                txtuserlastname.Text = string.Empty;
                ddlUserLocation.SelectedIndex =-1;
                ddlUserTitle.SelectedIndex = -1;
                ddlUserTimeZone.SelectedIndex = -1;
                txtmailID.Text = string.Empty;
                txtphone.Text = string.Empty;
                chkinactive.Checked = false;
                ddlRole.SelectedIndex = -1;
                btnSave.Visible = bitAdd;
                txtConfrimPass.Text = string.Empty;
                txtPassword.Text = string.Empty;
                hdnUserID.Value = string.Empty;
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

        #region ControlNames
        /// <summary>
        /// ControlNames Assign Values to label and Validators
        /// </summary>
        private void ControlNames()
        {
            try
            {

                //Unit Testing ID - UserMaster.aspx.cs_25
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_25 ControlNames");
                ((Label)this.Master.FindControl("ucPageHeader").FindControl("lblPageHeaderCaption")).Text = RollupText("UserMaster", "lblListCaption");
                lblCreateUser.Text = RollupText("UserMaster", "lblCreateBusinessUnit");
                lblUserID.Text = RollupText("UserMaster", "lblUserID");
                lblusername.Text = RollupText("UserMaster", "lblusername");
                lblmiddlename.Text = RollupText("UserMaster", "lblmiddlename");
                lbllastname.Text = RollupText("UserMaster", "lbllastname");
                lblLocation.Text = RollupText("UserMaster", "lblLocation");
                lblphone.Text = RollupText("UserMaster", "lblphone");
                lblmailID.Text = RollupText("UserMaster", "lblEmailID");
                lblusertitle.Text = RollupText("UserMaster", "lblusertitle");
                lblusertimezoneid.Text = RollupText("UserMaster", "lblusertimezoneid");
                lblRole.Text = RollupText("UserMaster", "lblRole");
                lblPassword.Text = RollupText("UserMaster", "lblPassword");
                lblConfirmPassword.Text = RollupText("UserMaster", "lblConfirmPassword");
                lblInactive.Text = RollupText("Common", "lblActive");

                txtPassword.Attributes["type"] = "password";
                txtConfrimPass.Attributes["type"] = "password";                    

                sConfirmation = RollupText("Common", "DeleteRecord");
                btnSave.Text = RollupText("Common", "btnSave");

                reqvRole.ErrorMessage = RollupText("UserMaster", "reqvRole");
                reqvtxtUserId.ErrorMessage = RollupText("UserMaster", "reqvtxtUserId");
                reqvUsername.ErrorMessage = RollupText("UserMaster", "reqvUsername");
                reqvtxtlastname.ErrorMessage = RollupText("UserMaster", "reqvtxtlastname");
                reqvphone.ErrorMessage = RollupText("UserMaster", "reqvphone");
                reqvtxtLocation.ErrorMessage = RollupText("UserMaster", "reqvtxtLocation");
                reqvEmailId.ErrorMessage = RollupText("UserMaster", "reqvEmailId");
                reqvPassword.ErrorMessage = RollupText("UserMaster", "reqvPassword");
                reqvConfirmPassword.ErrorMessage = RollupText("UserMaster", "reqvConfirmPassword");
                reqvTimeZoneID.ErrorMessage = RollupText("UserMaster", "reqvTimeZoneID");
                reqvUserTitle.ErrorMessage = RollupText("UserMaster", "reqvUserTitle");
                CmprPassword.ErrorMessage = RollupText("UserMaster", "CmprPassword");

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

        #endregion

        #region Export to Excel And PDF
        /// <summary>
        /// Button Export Excel click Event
        /// </summary>
        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - UserMaster.aspx.cs_26
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_26 Export Excel");

                DataTable ldt_ExcelExp = (DataTable)ViewState["export"];
                lwdg_UserMasterGrid.DataSource = ldt_ExcelExp;
                lwdg_UserMasterGrid.DataBind();
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebExcelExporter.Export(lwdg_UserMasterGrid);
                WebExcelExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebExcelExporter.Export(this.lwdg_UserMasterGrid);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        /// <summary>
        /// Export to pdf button click Event
        /// </summary>
        protected void btnExportPDF_Click(object sender, EventArgs e)
        {
            try
            {
                //Unit Testing ID - UserMaster.aspx.cs_27
                System.Diagnostics.Debug.WriteLine("Unit testing ID - UserMaster.aspx.cs_27 ExportPdf");
                DataTable ldt_PdfExp = (DataTable)ViewState["export"];
                TrackIT.WebApp.CommonSettings.ApplyGridSettings(lwdg_UserMasterGrid);
                lwdg_UserMasterGrid.DataSource = ldt_PdfExp;
                lwdg_UserMasterGrid.DataBind();
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Custom;
                WebPDFExporter.Export(lwdg_UserMasterGrid);
                WebPDFExporter.ExportMode = Infragistics.Web.UI.GridControls.ExportMode.Download;
                this.WebPDFExporter.Export(this.lwdg_UserMasterGrid);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
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
