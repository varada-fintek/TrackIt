using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace TrackIT.Security
{
    public static class UserDAL
    {
        #region Private Variables

        private const string INSERT_OR_UPDATE_USERS = "Security_InsertOrUpdateUsers";
        private const string GET_LOGIN_DETAILS = "Security_GetLoginDetails";
        private const string GET_CHANGE_PASSWORD_DETAILS = "Security_GetChangePasswordDetails";
        private const string GET_STUDENT_LOGIN_DETAILS = "Security_GetStudentLoginDetails";
        private const string CHANGE_PASSWORD = "Security_ChangePassword";
        private const string GET_USERS = "Security_GetUserDetails";
        private const string DELETE_RECORD = "Security_DeleteUser";
        private const string INSERT_LOGIN_DETAILS = "Security_InsertLoginDetails";
        private const int OUTMESSAGE_SIZE = 1000;

        #endregion

        #region InsertOrUpdateUsers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static UserBO InsertOrUpdateUsers(UserBO objUser)
        {
            using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            {
                try
                {   
                    SqlParameter[] objParams = {
                        new SqlParameter ("@UserID", objUser.UsersID),
                        new SqlParameter ("@UserName", objUser.UserName),
                        new SqlParameter ("@Password", objUser.Password),
                        new SqlParameter ("@RoleID", objUser.RoleID),
                        new SqlParameter ("@XMLData",   objUser.XMLData),
                        new SqlParameter ("@ModuleID",  objUser.ModuleID),
                        new SqlParameter ("@UserType",  objUser.UserType),
                        new SqlParameter ("@CreatedBy",  objUser.CreatedBy),
                        new SqlParameter ("@RowVersion",  objUser.RowVersion)
                    };

                    objUser.dsResult = SqlHelper.ExecuteDataset(Conn, INSERT_OR_UPDATE_USERS, objParams);
                    if (objUser.dsResult.Tables[0].Rows.Count > 0)
                    {
                        objUser.OutMessage = objUser.dsResult.Tables[0].Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
                return objUser;
            }
        }
        #endregion

        #region GetLoginDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static UserBO GetLoginDetails(UserBO objUser)
        {
            using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = {
                        new SqlParameter ("@UserName",   objUser.UserName),
                        new SqlParameter ("@Password",   objUser.Password)
                    };
                    IDataReader dataReader = SqlHelper.ExecuteReader(Conn, GET_LOGIN_DETAILS, objParams);
                    while (dataReader.Read())
                    {
                        if (!string.IsNullOrEmpty(dataReader["Users_ID"].ToString()))
                            objUser.UsersID = new Guid(dataReader["Users_ID"].ToString());
                        else
                            objUser.UsersID = null;

                        if (!string.IsNullOrEmpty(dataReader["Staff_ID"].ToString()))
                            objUser.StaffID = new Guid(dataReader["Staff_ID"].ToString());
                        else
                            objUser.StaffID = null;

                        objUser.UserName = dataReader["User_Name"].ToString();
                        objUser.LoginUserName = dataReader["Login_UserName"].ToString();
                        objUser.Password = (byte[])dataReader["Password"];
                        
                        if (!string.IsNullOrEmpty(dataReader["Role_ID"].ToString()))
                            objUser.RoleID = new Guid(dataReader["Role_ID"].ToString());
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

                        if (!string.IsNullOrEmpty(dataReader["Email_ID"].ToString()))
                            objUser.EmailID = dataReader["Email_ID"].ToString();
                        else
                            objUser.EmailID = string.Empty;

                        objUser.UserType = dataReader["User_Type"].ToString();
                        objUser.DisplayName = dataReader["Display_Name"].ToString();
                        objUser.RoleName = dataReader["Role_Name"].ToString();
                        objUser.RoleType = dataReader["Role_Type"].ToString();
                        objUser.IsSuperUser = Convert.ToBoolean(dataReader["Super_User"].ToString());
                        objUser.IsAdminRole = Convert.ToBoolean(dataReader["Admin_Role"].ToString());
                        objUser.DepartmentCode = dataReader["Department_Code"].ToString();
                        objUser.User_Photo_Path = dataReader["User_Photo_Path"].ToString();
                        objUser.IsFirstLogin = Convert.ToBoolean(dataReader["Is_First_Login"].ToString()) == true ? 1 : 0;
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objUser;
            }
        }
        #endregion

        #region GetChangePasswordDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static UserBO GetChangePasswordDetails(UserBO objUser)
        {
            using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = {
                        new SqlParameter ("@UserName",   objUser.UserName)
                    };
                    IDataReader dataReader = SqlHelper.ExecuteReader(Conn, GET_CHANGE_PASSWORD_DETAILS, objParams);
                    while (dataReader.Read())
                    {
                        if (!string.IsNullOrEmpty(dataReader["Users_ID"].ToString()))
                            objUser.UsersID = new Guid(dataReader["Users_ID"].ToString());
                        else
                            objUser.UsersID = null;

                        if (!string.IsNullOrEmpty(dataReader["Staff_ID"].ToString()))
                            objUser.StaffID = new Guid(dataReader["Staff_ID"].ToString());
                        else
                            objUser.StaffID = null;

                        objUser.UserName = dataReader["User_Name"].ToString();
                        objUser.Password = (byte[])dataReader["Password"];

                        if (!string.IsNullOrEmpty(dataReader["Role_ID"].ToString()))
                            objUser.RoleID = new Guid(dataReader["Role_ID"].ToString());
                        else
                            objUser.RoleID = null;

                        if (!string.IsNullOrEmpty(dataReader["Firm_ID"].ToString()))
                            objUser.FirmID = new Guid(dataReader["Firm_ID"].ToString());
                        else
                            objUser.FirmID = null;

                        if (!string.IsNullOrEmpty(dataReader["Firm_Code"].ToString()))
                            objUser.FirmCode = dataReader["Firm_Code"].ToString();
                        else
                            objUser.FirmCode = null;

                        if (!string.IsNullOrEmpty(dataReader["Email_ID"].ToString()))
                            objUser.EmailID = dataReader["Email_ID"].ToString();
                        else
                            objUser.EmailID = string.Empty;

                        objUser.UserType = dataReader["User_Type"].ToString();
                        objUser.DisplayName = dataReader["Display_Name"].ToString();
                        objUser.RoleName = dataReader["Role_Name"].ToString();
                        objUser.RoleType = dataReader["Role_Type"].ToString();
                        objUser.IsSuperUser = Convert.ToBoolean(dataReader["Super_User"].ToString());
                        objUser.IsAdminRole = Convert.ToBoolean(dataReader["Admin_Role"].ToString());
                        objUser.DepartmentCode = dataReader["Department_Code"].ToString();
                    }
                    dataReader.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objUser;
            }
        }
        #endregion

        #region GetStudentLoginDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static UserBO GetStudentLoginDetails(UserBO objUser)
        {
            //using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            //{
            //    try
            //    {
            //        SqlParameter[] objParams = {
            //            new SqlParameter ("@UserName",   objUser.UserName),
            //            new SqlParameter ("@Password",   objUser.Password)
            //        };
            //        IDataReader dataReader = SqlHelper.ExecuteReader(Conn, GET_STUDENT_LOGIN_DETAILS, objParams);
            //        while (dataReader.Read())
            //        {
            //            if (!string.IsNullOrEmpty(dataReader["Users_ID"].ToString()))
            //                objUser.UsersID = new Guid(dataReader["Users_ID"].ToString());
            //            else
            //                objUser.UsersID = null;
            //            objUser.UserName = dataReader["User_Name"].ToString();
            //            objUser.Password = (byte[])dataReader["Password"];
            //            objUser.UserType = dataReader["User_Type"].ToString();

            //            //using for portal
            //            if (!string.IsNullOrEmpty(dataReader["Login_Time"].ToString()))
            //                objUser.LoginTime = Convert.ToDateTime(dataReader["Login_Time"].ToString());

            //            objUser.LoginAccess = Convert.ToBoolean(dataReader["Login_Access"].ToString());
            //        }
            //        dataReader.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
                return objUser;
            //}
        }
        #endregion

        #region ChangePassword
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static UserBO ChangePassword(UserBO objUser)
        {
            using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            {
                try
                {                    
                    SqlParameter[] objParams = {
                        new SqlParameter ("@UserID",   objUser.UsersID),                        
                        new SqlParameter ("@NewPassword",   objUser.Password),
                        new SqlParameter ("@IsFirstLogin",   objUser.IsFirstLogin),
                        new SqlParameter ("@CreatedBy",   objUser.CreatedBy)                        
                    };
                    objUser.dsResult = SqlHelper.ExecuteDataset(Conn, CHANGE_PASSWORD, objParams);
                    if (objUser.dsResult.Tables[0].Rows.Count > 0)
                    {
                        objUser.OutMessage = objUser.dsResult.Tables[0].Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
                return objUser;
            }
        }
        #endregion

        #region GetUserDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static UserBO GetUserDetails(UserBO objUser)
        {
            using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@UserID",  objUser.UsersID),
                        new SqlParameter ("@UserName",  objUser.UserName),
                        new SqlParameter ("@StaffName",  objUser.StaffName),
                        new SqlParameter ("@RoleID",  objUser.RoleID),
                        new SqlParameter ("@Order",  objUser.SortOrder),
                        new SqlParameter ("@Pageindex",  objUser.PageIndex),
                        new SqlParameter ("@Pagesize",   objUser.PageSize),
                        new SqlParameter ("@SortBy",  objUser.SortBy)
                    };
                    if (!string.IsNullOrEmpty(objUser.UsersID.ToString()))
                    {
                        IDataReader dataReader = SqlHelper.ExecuteReader(Conn, GET_USERS, objParams);

                        while (dataReader.Read())
                        {
                            if (!string.IsNullOrEmpty(dataReader["Users_ID"].ToString()))
                                objUser.UsersID = new Guid(dataReader["Users_ID"].ToString());
                            else
                                objUser.UsersID = null;
                            objUser.UserName = dataReader["User_Name"].ToString();
                            objUser.Password = (byte[])dataReader["Password"];
                            if (!string.IsNullOrEmpty(dataReader["Role_ID"].ToString()))
                                objUser.RoleID = new Guid(dataReader["Role_ID"].ToString());
                            else
                                objUser.RoleID = null;

                            objUser.UserType = dataReader["User_Type"].ToString();

                            if (!string.IsNullOrEmpty(dataReader["Module_ID"].ToString()))
                                objUser.ModuleID = (Int32)dataReader["Module_ID"];

                            objUser.RowVersion = (byte[])dataReader["Row_Version"];
                        }
                    }
                    else
                    {
                        objUser.dsUser = SqlHelper.ExecuteDataset(Conn, GET_USERS, objParams);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objUser;
            }
        }
        #endregion

        #region DeleteUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string DeleteUser(UserBO objUser)
        {
            using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            {
                try
                {
                    //objConn.BeginTransaction();

                    SqlParameter[] objParams = {
                        new SqlParameter ("@UserID",  objUser.UsersID),
                        new SqlParameter ("@CreatedBy",  objUser.CreatedBy)                        
                    };

                    objUser.dsResult = SqlHelper.ExecuteDataset(Conn, DELETE_RECORD, objParams);

                    if (objUser.dsResult.Tables[0].Rows.Count > 0)
                    {
                        objUser.OutMessage = objUser.dsResult.Tables[0].Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    //objConn.Rollback();
                    throw ex;
                }

                return objUser.OutMessage;
            }
        }
        #endregion

        #region InsertLoginDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static void InsertLoginDetails(UserBO objUser)
        {
            using (SqlConnection Conn = new SqlConnection(objUser.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@SessionID",  objUser.LoginSessionID),
                        new SqlParameter ("@UserID", objUser.UsersID),
                        new SqlParameter ("@Type", objUser.LoginType)
                    };
                    objUser.dsResult = SqlHelper.ExecuteDataset(Conn, INSERT_LOGIN_DETAILS, objParams);
                    if (objUser.dsResult.Tables[0].Rows.Count > 0)
                    {
                        objUser.OutMessage = objUser.dsResult.Tables[0].Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion
    }
}
