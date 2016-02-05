using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using TrackIT.Security;
using TrackIT.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;
using System.Text;
using TrackIT.WebApp.Common;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
//using TrackIT.BusinessObjects;
using TrackIT.WebApp.TrackITEnum;
using System.Net.Mail;

namespace TrackIT.WebApp
{
    public class UserBO : TrackITAbstractDAL
    {

        public Guid? UsersID { get; set; }
        public string UserName { get; set; }
        public string LoginUserName { get; set; }
        public byte[] Password { get; set; }
        public Guid? RoleID { get; set; }
        public DataSet dsUser { get; set; }
        public Boolean IsSuperUser { get; set; }
        public Boolean IsAdminRole { get; set; }
        public string ChildScreenURL { get; set; }
        public string UserType { get; set; }
        public Guid? FirmID { get; set; }
        public string FirmCode { get; set; }

        public int IsFirstLogin { get; set; }

        public Guid? UsersAccessID { get; set; }
        public int ModuleID { get; set; }
        public int ScreenID { get; set; }
        //public string Image_Path { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int View { get; set; }
        public int Delete { get; set; }
        public string XMLData { get; set; }

        public string LoginSessionID { get; set; }
        public string LoginType { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogOffTime { get; set; }

        public int LoginID { get; set; }

        public Guid? logged_in_UserID { get; set; }
        

        public string DisplayName { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string User_Photo_Path { get; set; }

        public string DepartmentCode { get; set; }

        public bool LoginAccess { get; set; }

        public UserBO(string sConn)
        {
            ConnectionString = sConn;
        }
    }
    public partial class Login : BasePage
    {
        #region Declarations
        UserBO objUser;
        UserBO objResult;
        static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
        static string alphaLow = "qwertyuiopasdfghjklzxcvbnm";
        static string numerics = "1234567890";
        static string special = "@#$";
        //create another string which is a concatenation of all above
        string allChars = alphaCaps + alphaLow + numerics + special;
        DBHelper.DBConnect ldbh_QueryExecutors = new DBHelper.DBConnect();
        Random r = new Random();
        public string GET_LOGIN_DETAILS = "Security_GetLoginDetails";
        public string INSERT_LOGIN_DETAILS = "Security_InsertLoginDetails";
        #endregion

        #region "Events"

        #region Page Load Events
        /// <summary>
        /// Page Load Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // FS_ID 4.2.1.1
            //Unit Testing ID - Login_CS_1
            System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_1 - Page Load Execution");
            try
            {
                ControlNames();

                if (!IsPostBack)
                {
                    //Unit Testing ID - Login_CS_3
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_3 - Post Back Execution");
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Write(ex.Message.ToString());
            }
        }
        #endregion

        #region  Clear Button Click
        /// <summary>
        /// Clear Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtUsername.Text = string.Empty;
                txtPassword.Text = string.Empty;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Write(ex.Message.ToString());
            }
        }
        #endregion

        #region  Login Button Click
        /// <summary>
        /// Clear Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Unit Testing ID - Login_CS_4
            System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_4 - Login Method Execution");
            try
            {
                if (objUser == null) objUser = new UserBO(this.ConnectionString);
                objResult = new UserBO(this.ConnectionString);

                objUser.UserName = string.Empty;
                objUser.Password = null;

                if (!string.IsNullOrEmpty(txtUsername.Text))
                    objUser.UserName = txtUsername.Text.ToString().Trim();
                //FS_ID 4.2.1.4

                //Unit Testing ID - Login_CS_16
                System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_16 - " + txtUsername.Text);

              //  objResult = UserBLL.GetLoginDetails(objUser);

                SqlParameter[] objParams = {
                        new SqlParameter ("@UserName",   objUser.UserName),
                        new SqlParameter ("@Password",   objUser.Password)
                    };
              
               
                DataSet  lds_Result = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, GET_LOGIN_DETAILS, objParams);
                
                    if (!string.IsNullOrEmpty(lds_Result.Tables[0].Rows[0]["Users_ID"].ToString()))
                        objUser.UsersID = new Guid(lds_Result.Tables[0].Rows[0]["Users_ID"].ToString());
                    else
                        objUser.UsersID = null;

                    if (!string.IsNullOrEmpty(lds_Result.Tables[0].Rows[0]["Staff_ID"].ToString()))
                        objUser.logged_in_UserID = new Guid(lds_Result.Tables[0].Rows[0]["Staff_ID"].ToString());
                    else
                        objUser.logged_in_UserID = null;

                    objUser.UserName = lds_Result.Tables[0].Rows[0]["User_Name"].ToString();
                    objUser.LoginUserName = lds_Result.Tables[0].Rows[0]["Login_UserName"].ToString();
                    objUser.Password = (byte[])lds_Result.Tables[0].Rows[0]["Password"];

                    if (!string.IsNullOrEmpty(lds_Result.Tables[0].Rows[0]["Role_ID"].ToString()))
                        objUser.RoleID = new Guid(lds_Result.Tables[0].Rows[0]["Role_ID"].ToString());
                    else
                        objUser.RoleID = null;

                    //  if (!string.IsNullOrEmpty(dataReader["Firm_ID"].ToString()))
                    //    objUser.FirmID = new Guid(dataReader["Firm_ID"].ToString());
                    // else
                    //     objUser.FirmID = null;

                    //if (!string.IsNullOrEmpty(dataReader["Firm_Code"].ToString()))
                    //    objUser.FirmCode = dataReader["Firm_Code"].ToString();
                    //else
                    //    objUser.FirmCode = null;

                    if (!string.IsNullOrEmpty(lds_Result.Tables[0].Rows[0]["Email_ID"].ToString()))
                        objUser.EmailID = lds_Result.Tables[0].Rows[0]["Email_ID"].ToString();
                    else
                        objUser.EmailID = string.Empty;

                    objUser.UserType = lds_Result.Tables[0].Rows[0]["User_Type"].ToString();
                    objUser.DisplayName = lds_Result.Tables[0].Rows[0]["Display_Name"].ToString();
                    objUser.RoleName = lds_Result.Tables[0].Rows[0]["Role_Name"].ToString();
                    objUser.RoleType = lds_Result.Tables[0].Rows[0]["Role_Type"].ToString();
                    objUser.IsSuperUser = Convert.ToBoolean(lds_Result.Tables[0].Rows[0]["Super_User"].ToString());
                    objUser.IsAdminRole = Convert.ToBoolean(lds_Result.Tables[0].Rows[0]["Admin_Role"].ToString());
                    objUser.DepartmentCode = lds_Result.Tables[0].Rows[0]["Department_Code"].ToString();
                    objUser.User_Photo_Path = lds_Result.Tables[0].Rows[0]["User_Photo_Path"].ToString();
                    objUser.IsFirstLogin = Convert.ToBoolean(lds_Result.Tables[0].Rows[0]["Is_First_Login"].ToString()) == true ? 1 : 0;
              

                Session.Clear();
                //FS_ID 4.2.1.5

                //Unit Testing ID - Login_CS_17
                System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_17 - " + txtPassword.Text);
                if (objUser.UsersID != null)
                {
                    //Unit Testing ID - Login_CS_5
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_5 - " + objUser.UsersID);

                    if (Crypto.CompareHash(Encoding.UTF8.GetBytes(txtPassword.Text), objUser.Password))
                    {
                        //Unit Testing ID - Login_CS_6
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_6 - Crypto of Password");

                        SetSessionValue(SessionItems.User_ID, objUser.UsersID);
                        SetSessionValue(SessionItems.Login_Name, txtUsername.Text.ToString().Trim());
                        SetSessionValue(SessionItems.User_Name, objUser.LoginUserName);
                        SetSessionValue(SessionItems.User_Type, objUser.UserType);
                        SetSessionValue(SessionItems.Super_User, objUser.IsSuperUser);
                       // SetSessionValue(SessionItems.Firm_ID, objResult.FirmID);
                       // SetSessionValue(SessionItems.Firm_Code, objResult.FirmCode);
                       // SetSessionValue(SessionItems.ImageNameForPaySlip, sCtrlAImage);
                       // SetSessionValue(SessionItems.CurrentCountry, sCurrentCountry);
                        SetSessionValue(SessionItems.loggedin_User_ID, objResult.logged_in_UserID);
                       // SetSessionValue(SessionItems.Department_Code, objUser.DepartmentCode);
                        SetSessionValue(SessionItems.Role_ID, objUser.RoleID);
                        SetSessionValue(SessionItems.User_Display_Name, objUser.DisplayName);
                        SetSessionValue(SessionItems.Role_Name, objUser.RoleName);
                        SetSessionValue(SessionItems.Role_Type, objUser.RoleType);
                        SetSessionValue(SessionItems.Is_Admin_Role, objUser.IsAdminRole);
                        SetSessionValue(SessionItems.User_Photo_Path, objUser.User_Photo_Path);
                        SetSessionValue(SessionItems.Is_First_Login, objUser.IsFirstLogin);
                        
                        /* Get all the dropdown values and store in the cache */
                        //LookupCacheManager.CacheLookupValues();

                        /*Insert the Login in User Details*/
                        objResult.LoginSessionID = Session.SessionID.ToString();
                        //Unit Testing ID - Login_CS_7
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_7 - " + objUser.LoginSessionID);

                        objUser.LoginType = "L";
                        //Unit Testing ID - Login_CS_8  
                    //    System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_8 - " + objUser.LoginType);
                    //    SqlParameter[] objParams_logindetails = { 
                    //    new SqlParameter ("@SessionID",  objUser.LoginSessionID),
                    //    new SqlParameter ("@UserID", objUser.UsersID),
                    //    new SqlParameter ("@Type", objUser.LoginType)
                    //};
                    //    objUser.dsResult = SqlHelper.ExecuteDataset(ldbh_QueryExecutors.isqlcon_connection, INSERT_LOGIN_DETAILS, objParams);

               

                        //UserBLL.InsertLoginDetails(objResult);

                        if (objResult.IsFirstLogin == 1)
                            Response.Redirect("~/My_Profile.aspx?New=1", false);
                        else
                            Response.Redirect("~/Home.aspx", false);
                    }
                    else
                    {
                        //Unit Testing ID - Login_CS_15  
                        System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_15 - " + objResult.LoginType);

                        ScriptManager.RegisterClientScriptBlock(this.Page, GetType(), "key", "<script>alert('" + RollupText("Login", "errormsg") + "')</script>", false);
                        txtUsername.Text = string.Empty;
                        txtPassword.Text = string.Empty;
                    }
                }
                else
                {
                    //Unit Testing ID - Login_CS_9  
                    System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_9 - " + objUser.UsersID);

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "EnterError();", true);
                    txtUsername.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    //mdpForgetPW.Hide();
                }
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, Log_Only_Policy);
                Response.Write(ex.Message.ToString());
            }
            finally
            {
                if (objUser != null) objUser = null;
                if (objResult != null) objResult = null;
            }
        }
        #endregion

        #region Save Button Click
        /// <summary>
        /// Save Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSavePW_Click(object sender, EventArgs e)
        {
            //// FS_ID 4.2.1.1

            ////Unit Testing ID - Login_CS_10
            //System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_10 - btnSavePW_Click Method Execution");
            //reqInvalidUserName.Visible = false;
            //try
            //{
            //    if (objUser == null) objUser = new UserBO(this.ConnectionString);
            //    objResult = new UserBO(this.ConnectionString);

            //    if (!string.IsNullOrEmpty(txtForgetUserName.Text))
            //        objUser.UserName = (txtForgetUserName.Text);
            //    //FS_ID 4.2.2.6

            //    //Unit Testing ID - Login_CS_19
            //    System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_19 - " + objResult.UserName);

            //    objUser.Password = null;

            //    objResult = UserBLL.GetChangePasswordDetails(objUser);
            //    string User_Mail_ID = objResult.EmailID;
            //    string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //    string path = HttpContext.Current.Request.Url.AbsolutePath;
            //    string username = objUser.UserName;
            //    if (objResult.UsersID != null || objResult.EmailID != null)
            //    {
            //        //FS_ID 4.2.2.4

            //        //Unit Testing ID - Login_CS_11
            //        System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_11 - " + objResult.UsersID);

            //        MailMessage msg = new MailMessage();
            //        msg.From = new MailAddress("fintekdemo@gmail.com");
            //        msg.To.Add(new MailAddress(User_Mail_ID));
            //        var Generated_Password = GeneratePassword(8).ToString().Trim();
            //        msg.Body = "Welcome To Ctrl A...Your Username is : " + username + " Your Password is : " + Generated_Password + "               You can Login into CtrlA by using this Link" + url;

            //        //"Welcome To Ctrl A...Your Password is : " + GeneratePassword(8);
            //        Session["msg_body"] = Generated_Password;
            //        //msg.Body.Replace("Welcome To Ctrl A...Your Password is : ", ""); ;
            //        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));//465,25-gmail
            //        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("fintekdemo@gmail.com", "ABC123456789");
            //        smtpClient.Credentials = credentials;
            //        smtpClient.EnableSsl = true;
            //        smtpClient.Send(msg);
            //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "sentmail();", true);


            //        string PassWord = Generated_Password;
            //        objUser.Password = Crypto.CreateHash(PassWord);
            //        objUser.CreatedBy = objResult.UsersID;
            //        objUser.IsFirstLogin = 1;
            //        objResult = UserBLL.ChangePassword(objUser);
            //        //FS_ID 4.2.2.5

            //        //Unit Testing ID - Login_CS_18
            //        System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_18 - Session is cleared");


            //        Session.Clear();

            //        // Response.Redirect("~/Login.aspx", false);
            //    }
            //    else
            //    {
            //        //FS_ID 4.2.2.1

            //        //Unit Testing ID - Login_CS_12
            //        System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_12 - " + objResult.UsersID);
            //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(), "Script", "styleload();", true);
            //        //mdpForgetPW.Show();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    ExceptionPolicy.HandleException(ex, Log_Only_Policy);
            //    Response.Write(ex.Message.ToString());
            //}
            //finally
            //{
            //    if (objUser != null) objUser = null;
            //    if (objResult != null) objResult = null;
            //}

        }
        #endregion

        #region  Clear Button Click
        /// <summary>
        /// Clear Button Click Events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelPW_Click(object sender, EventArgs e)
        {
            txtForgetUserName.Text = string.Empty;
        }
        #endregion

        #endregion

        #region "User Defined Functions"

        #region ControlNames
        /// <summary>
        /// ControlNames
        /// </summary>
        private void ControlNames()
        {
            //Unit Testing ID - Login_CS_2
            System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_2 - Retrieval of Control Names");
            try
            {
                lblUserNameMail.Text = RollupText("Login", "lblUserNameMail");
                lblForgetPasswordCaption.Text = RollupText("Login", "lblForgetPasswordCaption");
                // txtForgetUserName_FilteredTextBoxExtender
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, Rethrow_Policy))
                    throw;
            }
        }
        #endregion

        #region GeneratePassword
        /// <summary>
        /// GeneratePassword
        /// </summary>
        public string GeneratePassword(int length)
        {
            //FS_ID 4.2.2.2

            //Unit Testing ID - Login_CS_13
            System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_13 - Execution of Generate Password Method");
            String generatedPassword = "";

            if (length < 4)
                throw new Exception("Number of characters should be greater than 4.");

            // Generate four repeating random numbers are postions of lower,
            // upper, numeric and special characters
            // By filling these positions with corresponding characters,
            // we can ensure the password has atleast one
            // character of those types
            int pLower, pUpper, pNumber, pSpecial;
            string posArray = "0123456789";
            if (length < posArray.Length)
                posArray = posArray.Substring(0, length);
            string randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble() * posArray.Length)].ToString();
            pLower = int.Parse(randomChar); posArray = posArray.Replace(randomChar, "");
            randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble() * posArray.Length)].ToString();
            pUpper = int.Parse(randomChar); posArray = posArray.Replace(randomChar, "");
            randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble() * posArray.Length)].ToString();
            pNumber = int.Parse(randomChar); posArray = posArray.Replace(randomChar, "");
            randomChar = posArray.ToCharArray()[(int)Math.Floor(r.NextDouble() * posArray.Length)].ToString();
            pSpecial = int.Parse(randomChar); posArray = posArray.Replace(randomChar, "");

            for (int i = 0; i < length; i++)
            {
                double rand = r.NextDouble();
                if (i == pLower)
                    generatedPassword += alphaCaps.ToCharArray()[(int)Math.Floor(rand * alphaCaps.Length)];
                else if (i == pUpper)
                    generatedPassword += alphaLow.ToCharArray()[(int)Math.Floor(rand * alphaLow.Length)];
                else if (i == pNumber)
                    generatedPassword += numerics.ToCharArray()[(int)Math.Floor(rand * numerics.Length)];
                else if (i == pSpecial)
                    generatedPassword += special.ToCharArray()[(int)Math.Floor(rand * special.Length)];
                else
                    generatedPassword += allChars.ToCharArray()[(int)Math.Floor(rand * allChars.Length)];
                //FS_ID 4.2.2.3

                //Unit Testing ID - Login_CS_14
                System.Diagnostics.Debug.WriteLine("Unit testing ID - Login_CS_14 -" + generatedPassword);
            }
            return generatedPassword;

        }
        #endregion

        #endregion
    }
}
