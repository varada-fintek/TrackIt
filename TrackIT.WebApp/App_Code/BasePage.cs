using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using AjaxControlToolkit;


//using TrackIT.BusinessObjects;
using TrackIT.WebApp.Common;

using TrackIT.WebApp.TrackITEnum;

using TrackIT.Common;
using TrackIT.Security;

using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
namespace TrackIT.WebApp
{
    public class UserAccessBO : TrackITAbstractDAL
    {
        public Guid? UsersAccessID { get; set; }
        public Guid? UsersID { get; set; }
        public Guid? RoleID { get; set; }
        public int ModuleID { get; set; }
        public int ScreenID { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int View { get; set; }
        public int Delete { get; set; }
        public DataSet dsUserAccess { get; set; }
        public string XMLData { get; set; }
        public string FilePath { get; set; }
        public string ChildScreenUrl { get; set; }

        public UserAccessBO(string sConn)
        {
            ConnectionString = sConn;
        }
    }
    public class BasePage : System.Web.UI.Page
    {

        #region "Constants"
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        public const string EXISTS = "EXISTS";
        public const string CONCURRENCY = "CONCURRENCY";
        public const string SUCCESS = "SUCCESS";
        public const string APPROVED = "APPROVED";
        public const string PENDING = "PENDING";
        public const string GENERATED = "GENERATED";
        public const string WORKFLOWNOTMAPPED = "WORKFLOWNOTMAPPED";
        public const string APPLICATIONTYPEEXIST = "APPLICATIONTYPEEXIST";

        public string GET_USER_ACCESS = "Security_GetUserAccess";
        public string GET_MODULE_ACCESS_RIGHTS = "Security_GetModuleAccessRights";
        public string GET_SCREEN_ACCESS_RIGHTS = "Security_GetScreenAccessRights";

        public string GET_SCREEN_AUTHENTICATION = "Security_GetScreenAuthentication";
        public string GET_TREEVIEW_MENU_DETAILS = "Security_GetTreeViewMenuDetails";
        public string GET_PARENT_SCREEN_URL = "Security_GetParentScreenUrl";


        public const string PREVIOUS = "previous";
        public const string NEXT = "next";

        public const string Log_Only_Policy = "Log_Only_Policy";
        public const string Rethrow_Policy = "Rethrow_Policy";
        public string Log_Only_Policy_Global = "Log_Only_Policy";
        public string Rethrow_Policy_Global = "Rethrow_Policy";

        public const string Employment_Type_Guid = "367C7E48-966D-4624-A8DB-D6EACC2C305F";
        public const string Designation_Guid = "E02E14CE-1C6D-4371-B2EC-4D9AE2618FFC";
        public const string Action_Type_Guid = "3B970E23-B871-4387-8AEA-306EAB734D74";

        public const string Email_FromMailAddress = "fintekdemo@gmail.com";
        public const string Email_CredentialEmailID = "fintekdemo@gmail.com";
        public const string Email_CredentialEmailPassword = "ABC123456789";
        public const string Email_SmtpClient = "smtp.gmail.com";
        public const int Email_SmtpPort = 587;
        public const bool Email_IsBodyHtml = true;
        public const bool Email_UseDefaultCredentials = false;
        public const bool Email_EnableSsl = true;
        #endregion

        #region "Private Variables"


        private Guid? _guidLoggedInUserRoleID;
        private Guid?  _guidLoggedInUserID;
        private Guid? _guidLoggedInFirmID;
        private Guid? _guidLoggedInStaffID;
        private string _sLoggedInUserName;
        private string _sLoggedInUserDisplayName;
        private string _sLoggedInUserPhotoPath;
        private string _sLoggedInUserRoleName;
        private string _sLoggedInUserRoleType;
        
        private string _sModuleID;
        private string _sLeftNode;
        private string _sScreenID;
        private string _sCurrencyCode;
        private string _isSuperUser;
        private bool   _bitIsAdminRole; 
        private string _sCurrentPageIndex;        
        private string _sConnectionString;
        private string _sCtrlAImage;
        private string _sCurrentCountry;
        private string _sDepartmentCode;
        //private CreateRequestBO _objCreateRequestBO;

        //For Web.Config Keys
        private string _sObjCreator;
        private string _sPageNavigationSize;
        private string _sPageSize;

        //For Grid Paging
        protected int iPageSize;
        protected int iPageIndex;
        protected int iTotalPage;
        //protected int iLastRequestID;
        #endregion

        #region "Protected Variables"
                
        protected string sParameterName = "STAFF";
        protected string sCtrlAImage = ConfigurationManager.AppSettings["CtrlAIMAGE"];
        protected string sCurrentCountry = ConfigurationManager.AppSettings["CURRENTCOUNTRY"];

        #endregion

        #region "Public Variables"

        public string DateFormat = "MM/dd/yyyy";
        public string sShortDateFormat = "dd/MM/yyyy";
        public string sShortDateDBFormat = "yyyy/MM/dd";
        public string sMarketingCalendarFormat = "MM/dd/yyyy";
        public string sYearFormat = "yyyy";
        public string sMonthYearFormat = "MMM, yyyy";
        public string sShortYearMonthFormat = "yyyy-MM";
        public string sCurrentDate = DateTime.Now.ToString();
        public string sNoImage = ConfigurationManager.AppSettings["NOIMAGE"].ToString();
        public string sUserTypeStudentCode = "STU";
        public string sUserTypeStaffCode = "STF";

        public Boolean bitAdd;
        public Boolean bitEdit;
        public Boolean bitDelete;
        public Boolean bitView;

        public string sAlreadyExists = string.Empty;
        public string sConCurrency = string.Empty;
        public string sDeleteSuccess = string.Empty;
        public string sInvalidMsg = string.Empty;
        public string sReturnMessage = string.Empty;
        public string sErrorMsg = string.Empty;
        public string sConfirmation = string.Empty;

        #endregion

        #region "Constructor"
        
        public BasePage()
        {

        }
        
        #endregion        

        #region "Properties"

        public Guid? LoggedInFirmId
        {
            get { return _guidLoggedInFirmID; }
            set { _guidLoggedInFirmID = value; }
        }

        public Guid? LoggedInUserRoleId
        {
            get { return _guidLoggedInUserRoleID; }
            set { _guidLoggedInUserRoleID = value; }
        }

        public Guid? LoggedInUserId
        {
            get { return _guidLoggedInUserID; }
            set { _guidLoggedInUserID = value; }
        }

        public Guid? LoggedInStaffID
        {
            get { return _guidLoggedInStaffID; }
            set { _guidLoggedInStaffID = value; }
        }

        public string LoggedInUserName
        {
            get { return _sLoggedInUserName; }
            set { _sLoggedInUserName = value; }
        }

        public string LoggedInUserDisplayName
        {
            get { return _sLoggedInUserDisplayName; }
            set { _sLoggedInUserDisplayName = value; }
        }

        public string LoggedInUserPhotoPath
        {
            get { return _sLoggedInUserPhotoPath; }
            set { _sLoggedInUserPhotoPath = value; }
        }

        public string LoggedInUserRoleName
        {
            get { return _sLoggedInUserRoleName; }
            set { _sLoggedInUserRoleName = value; }
        }

        public string LoggedInUserRoleType
        {
            get { return _sLoggedInUserRoleType; }
            set { _sLoggedInUserRoleType = value; }
        }       

        public string ModuleID
        {
            get { return _sModuleID; }
            set { _sModuleID = value; }
        }

        public string LeftNode
        {
            get { return _sLeftNode; }
            set { _sLeftNode = value; }
        }

        public string ScreenID
        {
            get { return _sScreenID; }
            set { _sScreenID = value; }
        }

        public string CurrencyCode
        {
            get { return _sCurrencyCode; }
            set { _sCurrencyCode = value; }
        }

        public string IsSuperUser
        {
            get { return _isSuperUser; }
            set { _isSuperUser = value; }
        }

        public string CurrentPageIndex
        {
            get { return _sCurrentPageIndex; }
            set { _sCurrentPageIndex = value; }
        }

        public Boolean IsAdminRole
        {
            get { return _bitIsAdminRole; }
            set { _bitIsAdminRole = value; }
        }

        public string ConnectionString
        {
            get { return _sConnectionString; }
            set { _sConnectionString = value; }
        }

        public string CtrlAImage
        {
            get { return _sCtrlAImage; }
            set { _sCtrlAImage = value; }
        }

        public string CurrentCountry
        {
            get { return _sCurrentCountry; }
            set { _sCurrentCountry = value; }
        }

        public string DepartmentCode
        {
            get { return _sDepartmentCode; }
            set { _sDepartmentCode = value; }
        }

       

        //public CreateRequestBO objCreateRequestBO
        //{
        //    get { return _objCreateRequestBO; }
        //    set { _objCreateRequestBO = value; }
        //}

        /* Start - Properties for Web.Config Keys */
        public string ObjectCreatorOption
        {
            get { return _sObjCreator; }
            set { _sObjCreator = value; }
        }

        public string PageNavigationSize
        {
            get { return _sPageNavigationSize; }
            set { _sPageNavigationSize = value; }
        }

        public string PageSize
        {
            get { return _sPageSize; }
            set { _sPageSize  = value; }
        }

        /* End - Properties for Web.Config Keys */

        #endregion
        
        #region "Function Override"

        protected override void InitializeCulture()
        {
            try
            {
                _sConnectionString = ConfigurationManager.ConnectionStrings["ConnectionDB"].ToString();

                if (Request.Url.LocalPath.Substring(1) != "Login.aspx")
                {
                    if (GetSessionValue(SessionItems.User_ID) != null)
                    {
                        _guidLoggedInUserID = Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.User_ID).ToString());
                       // _guidLoggedInFirmID = Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.Firm_ID).ToString());
                        _guidLoggedInUserRoleID = Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.Role_ID).ToString());

                        if (GetSessionValue(SessionItems.loggedin_User_ID) != null)
                            _guidLoggedInStaffID = Conversion.ConvertStringToGuid(GetSessionValue(SessionItems.loggedin_User_ID).ToString());
                        _sLoggedInUserName = StringFunctions.ToString(GetSessionValue(SessionItems.User_Name)).Trim();
                        _sModuleID = StringFunctions.ToString(GetSessionValue(SessionItems.Module_ID)).Trim();
                        _sLeftNode = StringFunctions.ToString(GetSessionValue(SessionItems.Left_Node)).Trim();
                        _sScreenID = StringFunctions.ToString(GetSessionValue(SessionItems.Screen_ID)).Trim();
                       // _sCurrencyCode = StringFunctions.ToString(GetSessionValue(SessionItems.CurrencyCode)).Trim();
                        _isSuperUser = StringFunctions.ToString(GetSessionValue(SessionItems.Super_User)).Trim();
                        //_sCurrentPageIndex = StringFunctions.ToString(GetSessionValue(SessionItems.Current_Page_Index)).Trim();
                        //_sCtrlAImage = StringFunctions.ToString(GetSessionValue(SessionItems.ImageNameForPaySlip)).Trim();
                        //_sCurrentCountry = StringFunctions.ToString(GetSessionValue(SessionItems.CurrentCountry)).Trim();
                       // _sDepartmentCode = StringFunctions.ToString(GetSessionValue(SessionItems.Department_Code)).Trim();
                        _sLoggedInUserDisplayName = StringFunctions.ToString(GetSessionValue(SessionItems.User_Display_Name)).Trim();
                        _sLoggedInUserRoleName = StringFunctions.ToString(GetSessionValue(SessionItems.Role_Name)).Trim();
                        _sLoggedInUserRoleType = StringFunctions.ToString(GetSessionValue(SessionItems.Role_Type)).Trim();
                        _bitIsAdminRole = Convert.ToBoolean(GetSessionValue(SessionItems.Is_Admin_Role));
                        _sLoggedInUserPhotoPath = StringFunctions.ToString(GetSessionValue(SessionItems.User_Photo_Path)).Trim();

                        // Read from Web.config
                        _sObjCreator = Utilities.GetKey("ObjectCreatorOption");
                        _sPageSize = Utilities.GetKey("PageSize");
                        if (StringFunctions.IsNullOrEmpty(_sPageSize)) _sPageSize = "1";
                        _sPageNavigationSize = Utilities.GetKey("PageNavigationSize");
                    }
                    else
                    {
                        Response.Redirect("~/Login.aspx", false);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            try
            {
                if (Request.Url.LocalPath.Substring(1) != "Login.aspx")
                {
						if (this.LoggedInUserId == null)
						{
							if (Request.Url != null)
							{
								Response.Redirect("~/Login.aspx", false);
								//Response.End();
							}
						}
						else
							ScreenAuthentication();
					}
                base.OnPreInit(e);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Login.aspx", false);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            try
            {
                string sApplicationName = "";
                if (!StringFunctions.IsNullOrEmpty(Utilities.GetKey("ApplicationTitle")))
                    sApplicationName = Utilities.GetKey("ApplicationTitle");

                HttpContext.Current.Application["APPLICATION_TITLE"] = sApplicationName;
                base.OnInit(e);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            try
            {
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Redirect("~/Error.aspx", false);
            }
        }

        #endregion

        #region "UDF - Session"
        /// <summary>
        /// To Set the Session Value
        /// </summary>
        /// <param name="eValue">Session Name - Enum value from SessionEnum.cs</param>
        /// <param name="objValue">Session Values</param>
        /// <returns>Will return whether Session value set or not</returns>
        public bool SetSessionValue(Enum eValue, object objValue)
        {
            try
            {
                Session[StringEnum.GetStringValue(eValue)] = objValue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// To Get the Session Value
        /// </summary>
        /// <param name="eValue">Session Name - Enum value from SessionEnum.cs</param>
        /// <returns></returns>
        public object GetSessionValue(Enum eValue)
        {
            try
            {
                return Session[StringEnum.GetStringValue(eValue)];
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region "UDF - Menu"

        public void GetTopandLeftMenuItems()
        {            
            try
            {
                if (GetSessionValue(SessionItems.Top_Menu) == null)
                {
                    UserAccessBO objUserAccess = new UserAccessBO(this.ConnectionString);
                    objUserAccess.UsersID = this.LoggedInUserId;
                    DataSet dtMenu = null;
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@UsersID",  objUserAccess.UsersID)                     
                    };

                    dtMenu = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, GET_MODULE_ACCESS_RIGHTS, objParams);
                   // dtMenu = UserAccessBLL.GetModuleAccessRights(objUserAccess);

                    SetSessionValue(SessionItems.Top_Menu, dtMenu);         
                }

                if (GetSessionValue(SessionItems.Left_Menu) == null)
                {
                    UserAccessBO objUserAccess = new UserAccessBO(this.ConnectionString);
                    objUserAccess.UsersID = this.LoggedInUserId;
                    DataSet dtMenu = null;
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@UsersID",  objUserAccess.UsersID) ,
                        new SqlParameter ("@ModuleID",  objUserAccess.ModuleID)
                    };

                    objUserAccess.dsUserAccess = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, GET_SCREEN_ACCESS_RIGHTS, objParams);

                    //dtMenu = UserAccessBLL.GetScreenAccessRights(objUserAccess);

                    SetSessionValue(SessionItems.Left_Menu, dtMenu);
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }            
        }

        /// <summary>
        /// Gets the Left Menu Items based on the given Module ID
        /// </summary>
        /// <param name="sModuleID">Module ID</param>
        /// <returns>DataTable</returns>
        public DataTable GetLeftMenuItems(string sModuleID)
        {
           // DataSet dtMenus = null;
            DataTable dtMenu =null;
            try
            {
                UserAccessBO objUserAccess = new UserAccessBO(this.ConnectionString);
                objUserAccess.UsersID = this.LoggedInUserId;
                objUserAccess.ModuleID = IntegerFunctions.ToInt32(sModuleID);
                SqlParameter[] objParams = { 
                        new SqlParameter ("@UsersID",  objUserAccess.UsersID) ,
                        new SqlParameter ("@ModuleID",  objUserAccess.ModuleID)
                    };

                dtMenu = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, GET_SCREEN_ACCESS_RIGHTS, objParams).Tables[0];
                //dtMenus = UserAccessBLL.GetScreenAccessRights(objUserAccess);
              // dtMenu = dtMenus.Tables[0];
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            return dtMenu;
        }

        public DataTable GetScreenAuthentication(Guid? guidUserID, string sFilePath)
        {
            DataTable dtRights = null;
            try
            {
                UserAccessBO objUserAccess = new UserAccessBO(this.ConnectionString);
                //objUserAccess.UsersID = guidUserID;
                objUserAccess.RoleID = this._guidLoggedInUserRoleID;
                objUserAccess.FilePath = sFilePath;
                SqlParameter[] objParams = { 
                        //new SqlParameter ("@UserID",  objUserAccess.UsersID),
                        new SqlParameter ("@RoleID",  objUserAccess.RoleID),
                        new SqlParameter ("@FileUrl",  objUserAccess.FilePath)
                    };

                dtRights = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, GET_SCREEN_AUTHENTICATION, objParams).Tables[0];
               // dtRights = UserAccessBLL.GetScreenAuthentication(objUserAccess);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            return dtRights;
        }

        public bool IsUserHasScreenAuthentication(Guid? guidUserID, string sFilePath, string sOption)
        {
            bool isHasRights = false;
            try
            {
                DataTable dtRights = GetScreenAuthentication(guidUserID, sFilePath);
                if (dtRights.Rows.Count > 0)
                {
                    if (sOption.Trim().ToUpper() == "ADD")
                    {
                        if (Convert.ToBoolean(dtRights.Rows[0]["Add"]))
                            isHasRights = true;
                    }
                    else if (sOption.Trim().ToUpper() == "EDIT")
                    {
                        if (Convert.ToBoolean(dtRights.Rows[0]["Edit"]))
                            isHasRights = true;
                    }
                    else if (sOption.Trim().ToUpper() == "DELETE")
                    {
                        if (Convert.ToBoolean(dtRights.Rows[0]["Delete"]))
                            isHasRights = true;
                    }
                    else if (sOption.Trim().ToUpper() == "VIEW")
                    {
                        if (Convert.ToBoolean(dtRights.Rows[0]["View"]))
                            isHasRights = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            return isHasRights;
        }
        

        public void ScreenAuthentication()
        {
            DataTable objRights = new DataTable();
            try
            {
                if (GetSessionValue(SessionItems.User_ID) != null)
                {
                    if (this.IsSuperUser.ToUpper() == "TRUE")
                    {
                        bitAdd = true;
                        bitEdit = true;
                        bitDelete = true;
                        bitView = true;
                    }
                    else if ((Request.CurrentExecutionFilePath != "/Home.aspx") && (Request.CurrentExecutionFilePath != "/Error.aspx")
                             && (Request.CurrentExecutionFilePath != "/Reports/ReportViewer.aspx"))
                    {
                        UserAccessBO objUserAccess = new UserAccessBO(this.ConnectionString);
                        //objUserAccess.UsersID = this.LoggedInUserId;
                        objUserAccess.RoleID = this.LoggedInUserRoleId;  
                        objUserAccess.FilePath = Request.CurrentExecutionFilePath;

                        string[] sRawURL = Request.RawUrl.ToString().Split('&');
                        string sFilePath = sRawURL[0].ToString();
                        if (Request.CurrentExecutionFilePath.Equals("/StudentSearch.aspx"))
                            objUserAccess.FilePath = sFilePath; //Request.RawUrl.ToString();
                        else if (Request.CurrentExecutionFilePath.Equals("/HCM/StaffsList.aspx"))
                            objUserAccess.FilePath = sFilePath; //Request.RawUrl.ToString();
                        else if (Request.CurrentExecutionFilePath.Equals("/HCM/StaffReports.aspx"))
                            objUserAccess.FilePath = sFilePath; //Request.RawUrl.ToString();

                        SqlParameter[] objParams = { 
                        //new SqlParameter ("@UserID",  objUserAccess.UsersID),
                        new SqlParameter ("@RoleID",  objUserAccess.RoleID),
                        new SqlParameter ("@FileUrl",  objUserAccess.FilePath)
                    };

                        objRights = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, GET_SCREEN_AUTHENTICATION, objParams).Tables[0];
                       // objRights = UserAccessBLL.GetScreenAuthentication(objUserAccess);

                        if (objRights.Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(objRights.Rows[0]["Add"]) || Convert.ToBoolean(objRights.Rows[0]["Edit"]) || Convert.ToBoolean(objRights.Rows[0]["Delete"]) || Convert.ToBoolean(objRights.Rows[0]["View"]))
                            {
                                bitAdd = Convert.ToBoolean(objRights.Rows[0]["Add"].ToString());
                                bitEdit = Convert.ToBoolean(objRights.Rows[0]["Edit"].ToString());
                                bitDelete = Convert.ToBoolean(objRights.Rows[0]["Delete"].ToString());
                                bitView = Convert.ToBoolean(objRights.Rows[0]["View"].ToString());
                            }
                            else
                                Response.Redirect("~/Logout.aspx", false);
                        }
                        else
                            Response.Redirect("~/Logout.aspx", false);
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
        
        #region "UDF - Alert Messages"

        public void AlertMessage(string sMessage)
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + sMessage + "')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "key", "<script>alert('" + sMessage + "')</script>", false);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }   
        }

        /// <summary>
        /// For javascript Success message alert
        /// </summary>
        public void SaveMessage()
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "key", "<script>alert('" + RollupText("Common", "SaveMsg") + "', 2)</script>", false);
                //ScriptManager.RegisterClientScriptBlock(Page, GetType(), "key", "<script>alert('" + RollupText("Common", "SaveMsg") + "', 2)</script>", false);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }            
        }

        /// <summary>
        /// For javascript Concurrency message alert
        /// </summary>
        public void ConcurrencyMessage()
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + RollupText("Common", "ConcurrencyMsg") + "')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "key", "<script>alert('" + RollupText("Common", "ConcurrencyMsg") + "')</script>", false);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }            
        }

        /// <summary>
        /// For javascript Delete message alert
        /// </summary>
        public void DeleteMessage()
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + RollupText("Common", "DeleteMsg") + "')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "key", "<script>alert('" + RollupText("Common", "DeleteMsg") + "')</script>", false);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }            
        }

        /// <summary>
        /// For javascript Reference Delete message alert
        /// </summary>
        public void ReferenceDeleteMessage(string sDeleteMsg)
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('Referred in the Following Screen \\n " + sDeleteMsg + "')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "key", "<script>alert('Referred in the Following Screen \\n " + sDeleteMsg + "')</script>", false);
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }            
        }

        /// <summary>
        /// For javascript Already Exists message alert
        /// </summary>
        public void AlreadyExistMessage()
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "Key", "<script>alert('" + RollupText("Common", "AlreadyExistMsg") + "')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "Key", "<script>alert('" + RollupText("Common", "AlreadyExistMsg") + "')</script>", false);

            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }            
        }

        /// <summary>
        /// For javascript Workflow Not Mapped Message message alert
        /// </summary>
        public void WorkflowNotMappedMessage()
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "Key", "<script>alert('" + RollupText("Common", "WorkflowNotMappingMsg") + "')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "Key", "<script>alert('" + RollupText("Common", "WorkflowNotMappingMsg") + "')</script>", false);
                
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }

        /// <summary>
        /// For javascript Workflow Not Mapped Message message alert
        /// </summary>
        public void ApplicationTypeExistMessage(string sApplicationType, string sBuName)
        {
            try
            {
                //ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "Key", "<script>alert('" + sApplicationType + RollupText("Common", "ApplicationTypeExistMsg") + sBuName + "')</script>", false);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "Key", "<script>alert('" + sApplicationType + RollupText("Common", "ApplicationTypeExistMsg") + sBuName + "')</script>", false);

            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion        

        //#region "UDF - Grid Events"

        //protected void RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            if (e.Row.RowState == DataControlRowState.Alternate)
        //                e.Row.Attributes.Add("onmouseout", "this.className='AlternatingRowStyle';");
        //            else
        //                e.Row.Attributes.Add("onmouseout", "this.className='gvRowStyle';");

        //            e.Row.Style.Add("Cssclass", "label");
        //            e.Row.Attributes.Add("onmouseover", "this.className='AlternatingOnMouseOver';");

        //            if (e.Row.FindControl("imgView") != null)
        //                ((ImageButton)e.Row.FindControl("imgEdit")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

        //            if (e.Row.FindControl("imgEdit") != null)
        //                ((ImageButton)e.Row.FindControl("imgEdit")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

        //            if (e.Row.FindControl("imgDelete") != null)
        //                ((ImageButton)e.Row.FindControl("imgDelete")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

        //            if (e.Row.FindControl("imgBtnView") != null)
        //                ((ImageButton)e.Row.FindControl("imgBtnEdit")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

        //            if (e.Row.FindControl("imgBtnEdit") != null)
        //                ((ImageButton)e.Row.FindControl("imgBtnEdit")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");

        //            if (e.Row.FindControl("imgBtnDelete") != null)
        //                ((ImageButton)e.Row.FindControl("imgBtnDelete")).Attributes.Add("onmouseover", "javascript:this.style.cursor='pointer'");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, Log_Only_Policy);
        //        Response.Redirect("~/Error.aspx", false);
        //    }
        //}

        //protected void RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            e.Row.Attributes.Add("style", "word-break:break-all;word-wrap:break-word");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionPolicy.HandleException(ex, Log_Only_Policy);
        //        Response.Redirect("~/Error.aspx", false);
        //    }
        //}

        //#endregion

        
        #region "UDF - Common Functions"

        public string RollupText(string sResFileName, string sKey)
        {
            string sGetResource = String.Empty;
            try
            {
                if (StringFunctions.IsNullOrEmpty(sGetResource))
                {
                    sGetResource = ((string)base.GetGlobalResourceObject(sResFileName, sKey));
                }
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
            return sGetResource;
        }

        //public string DeleteRecords(CommonBO objCommonBO)
        //{            
        //    string sOutput = string.Empty;
        //    try
        //    {
        //        ICommon objBase = CommonFactory.CreateCommonObject(ObjectCreatorOption);
        //        sOutput = objBase.DeleteRecords(objCommonBO);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
        //            throw;
        //    }
        //    finally
        //    {
                
        //    }
        //    return sOutput;
        //}

        public static string GetName(Type oType)
        {
            return oType.Name;
            //return typeof(oType).Name;
        }

        public static bool SendEmail(string From, string To, string Subject, string Body, string AttachmentPath, MailPriority Priority)
        {
            MailAddress SendFrom = new MailAddress(From);
            MailAddress SendTo = new MailAddress(To);
            MailMessage objEmail = new MailMessage(SendFrom, SendTo);

            objEmail.Subject = Subject;
            objEmail.Body = Body;
            objEmail.Priority = Priority;

            if (!StringFunctions.IsNullOrEmpty(AttachmentPath))
            {
                Attachment attachFile = new Attachment(AttachmentPath);
                objEmail.Attachments.Add(attachFile);
            }

            SmtpClient emailClient = new SmtpClient();
            emailClient.Send(objEmail);
            return true;
        }    

        #endregion

        #region "Email - GenerateEmail"
        public static bool GenerateEmail(string From, string To, string Subject, string Body)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("fintekdemo@gmail.com");
                msg.To.Add(new MailAddress("manikandan.velmurugan@fintek.com.sg"));
                //msg.Body = "Welcome To Ctrl A...Your Password is : ";
                string mesgbody = msg.Body.Replace("Welcome To Ctrl A...Your Password is : ", "");

                //StreamReader reader = new StreamReader(Microsoft.SqlServer.Server.MapPath("testemail.html"));

                //string readFile = reader.ReadToEnd();
                //string StrContent = "";
                //StrContent = readFile;

                msg.Subject = "Request Status";
                msg.Body = "<html xmlns='http://www.w3.org/1999/xhtml'><head>    <title></title></head><body>    <div style='padding:0px;'><br /><h3>Dear manikandan.velmurugan@fintek.com.sg</h3><p><b>Request NO</b>:REQ/08/2015/0041 has been successfully reviewed by Jhon &amp; completed.</p>        <p>Please find the tickets details below. Thank You.</p>        <div>            <div style='background-color:lightblue; padding:0px;'>                <h5 style='padding:5px;'>Request Details</h5>            </div>           <table>                <tr style='clear:both; padding-bottom:10px;'>                    <td style='float:left; width:50%;'><b>Request ID    </b>:REQ/08/2015/0041</td>                    <td style='float:left; width:40%;'><b>Status        </b>:Reviewed &amp; Closed</td>                </tr>                <tr style='clear:both; padding-bottom:10px;'>                    <td style='float:left; width:50%;'><b> Business Unit    </b>:Singapore</td>                    <td style='float:left;width:40%;'><b> Department        </b>:Reviewed &amp; Closed</td>                </tr>                <tr style='clear:both;padding-bottom:10px;'>                    <td style='float:left;width:50%;'><b>Employee No   </b>:REQ/08/2015/0041</td>                    <td style='float:left;width:40%;'><b>Employee Name </b>:Reviewed &amp; Closed</td>                </tr>                <tr style='clear:both;padding-bottom:10px;'>                    <td style='float:left;width:50%;'><b>Manager 1 </b>:REQ/08/2015/0041</td>                    <td style='float:left;width:40%;'><b>Manager 2 </b>:Reviewed &amp; Closed</td>                </tr>                <tr style='clear:both;padding-bottom:10px;'>                    <td style='float:left;width:50%;'><b>Requested By   </b>:REQ/08/2015/0041</td>                    <td style='float:left;width:40%;'><b>Requested Date </b>:Reviewed &amp; Closed</td>                </tr>                <tr style='clear:both;padding-bottom:10px;'>                    <td style='float:left;width:50%;'><b>Access Start Date </b>:REQ/08/2015/0041</td>                    <td style='float:left;width:40%;'><b>Access End Date </b>:Reviewed &amp; Closed</td>                </tr>            </table>            <div style='background-color:lightblue; padding:0px;clear:both;'>                <h5 style='padding:5px;'>Request Application</h5>            </div>            <table>                <tr style='clear:both;padding-bottom:10px;'>                    <td style='float:left;width:50%;padding-bottom:10px;'><b>Application Icon </b>:REQ/08/2015/0041</td>                    <td style='width:40%;float:left;padding-bottom:10px;'><b>Application Code </b>:Reviewed &amp; Closed</td>   </tr>                <tr> <td style='float:left;width:50%;padding-bottom:10px;'><b>Application Name </b>:Reviewed &amp; Closed</td>                    <td style='width:40%;float:left;padding-bottom:10px;'><b>Equivalent User </b>:Reviewed &amp; Closed</td>        </tr> <tr>           <td style='float:left;width:50%;padding-bottom:10px;'><b>Access Role </b>:Reviewed &amp; Closed</td>                    <td style='float:left;width:40%;padding-bottom:10px;'><b>Requestor Comments </b>:Reviewed &amp; Closed</td>                </tr>            </table>            <br />            <div style='background-color:lightblue; padding:0px;clear:both;'>                <h5 style='padding:5px;'>Approved by David (EMP0019)</h5>            </div>            <table>                <tr>                    <td style='float:left;width:50%;'><b>Action Date </b>:09/07/2015</td>                    <td style='float:left;'><b>Comments </b>:Approved Via application</td>                                   </tr>            </table><br />            <div style='background-color:lightblue; padding:0px; clear:both;'>                <h5 style='padding:5px;'>Created by Joe (EMP0008)</h5>            </div>            <table>                <tr>                    <td style='float:left;width:50%;'><b>Action Date </b>:09/07/2015</td>                    <td style='float:left;'><b>Comments </b>:Approved Via application</td>                </tr>            </table>            <br />            <div style='background-color:lightblue; padding:0px;clear:both;'>                <h5 style='padding:5px;'>Reviewed by Jhon(EMP0002)</h5>            </div>            <table>                <tr>                    <td style='float:left;width:50%;'><b>Action Date </b>:09/07/2015</td>                    <td style='float:left;'><b>Comments </b>:Approved Via application</td>                </tr>            </table>                    </div>    </div></body></html>";
                msg.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));//465,25-gmail
                smtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("fintekdemo@gmail.com", "ABC123456789");
                smtpClient.Credentials = credentials;
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "sentmail();", true);
                //string PassWord = mesgbody;
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "styleload();", true);
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                //Response.Write(ex.Message.ToString());
            }
            finally
            {
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "msgsuccessfully();", true);
            }
            return true;
        }
        #endregion

        #region "Password - GenerateRandamPassword"
        public string GenerateRandamPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        #endregion
    }
   
}
