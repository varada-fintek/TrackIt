using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace TrackIT.Security
{
    public static class UserAccessDAL
    {
        #region Private Variables

        private const string GET_USER_ACCESS = "Security_GetUserAccess";
        private const string GET_MODULE_ACCESS_RIGHTS = "Security_GetModuleAccessRights";
        private const string GET_SCREEN_ACCESS_RIGHTS = "Security_GetScreenAccessRights";
       
        private const string GET_SCREEN_AUTHENTICATION = "Security_GetScreenAuthentication";
        private const string GET_TREEVIEW_MENU_DETAILS = "Security_GetTreeViewMenuDetails";
        private const string GET_PARENT_SCREEN_URL = "Security_GetParentScreenUrl";

        #endregion

        #region GetUserAccess
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserAccess"></param>
        /// <returns></returns>
        public static UserAccessBO GetUserAccess(UserAccessBO objUserAccess)
        {
            using (SqlConnection Conn = new SqlConnection(objUserAccess.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@UsersID",  objUserAccess.UsersID),
                        new SqlParameter ("@RoleID",  objUserAccess.RoleID),                        
                        new SqlParameter ("@ModuleID",  objUserAccess.ModuleID)
                    };

                    objUserAccess.dsUserAccess = SqlHelper.ExecuteDataset(Conn, GET_USER_ACCESS, objParams);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objUserAccess;
            }
        }
        #endregion

        #region GetModuleAccessRights
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserAccess"></param>
        /// <returns></returns>
        public static DataTable GetModuleAccessRights(UserAccessBO objUserAccess)
        {
            using (SqlConnection Conn = new SqlConnection(objUserAccess.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@UsersID",  objUserAccess.UsersID)                     
                    };

                    objUserAccess.dsUserAccess = SqlHelper.ExecuteDataset(Conn, GET_MODULE_ACCESS_RIGHTS, objParams);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objUserAccess.dsUserAccess.Tables[0];
            }
        }
        #endregion

        #region GetScreenAccessRights
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserAccess"></param>
        /// <returns></returns>
        public static DataTable GetScreenAccessRights(UserAccessBO objUserAccess)
        {
            using (SqlConnection Conn = new SqlConnection(objUserAccess.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@UsersID",  objUserAccess.UsersID) ,
                        new SqlParameter ("@ModuleID",  objUserAccess.ModuleID)
                    };

                    objUserAccess.dsUserAccess = SqlHelper.ExecuteDataset(Conn, GET_SCREEN_ACCESS_RIGHTS, objParams);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objUserAccess.dsUserAccess.Tables[0];
            }
        }

    
        #endregion

        #region GetScreenAuthentication
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserAccess"></param>
        /// <returns></returns>
        public static DataTable GetScreenAuthentication(UserAccessBO objUserAccess)//string sUserID, string sFilePath)
        {
            using (SqlConnection Conn = new SqlConnection(objUserAccess.ConnectionString))
            {
                DataSet dsResult = new DataSet();
                DataTable dtResult = new DataTable();

                try
                {
                    SqlParameter[] objParams = { 
                        //new SqlParameter ("@UserID",  objUserAccess.UsersID),
                        new SqlParameter ("@RoleID",  objUserAccess.RoleID),
                        new SqlParameter ("@FileUrl",  objUserAccess.FilePath)
                    };

                    dsResult = SqlHelper.ExecuteDataset(Conn, GET_SCREEN_AUTHENTICATION, objParams);

                    if (dsResult != null)
                    {
                        if (dsResult.Tables.Count > 0)
                        {
                            dtResult = dsResult.Tables[0];
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dtResult;
            }
        }
        #endregion

        #region GetTreeViewMenuDetails
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserID"></param>
        /// <param name="sModule_ID"></param>
        /// <returns></returns>
        public static DataSet GetTreeViewMenuDetails(UserAccessBO objUserAccess) //string sUserID, string sModuleID
        {
            DataSet dsMenu = new DataSet();

            using (SqlConnection Conn = new SqlConnection(objUserAccess.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        //new SqlParameter ("@UserID",  objUserAccess.UsersID) ,
                        new SqlParameter ("@Role_ID",  objUserAccess.RoleID) 
                        //new SqlParameter ("@ModuleID",  objUserAccess.ModuleID)
                    };

                    dsMenu = SqlHelper.ExecuteDataset(Conn, GET_TREEVIEW_MENU_DETAILS, objParams);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return dsMenu;
            }
        }
        #endregion

        #region GetParentScreenUrl
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sUserID"></param>
        /// <param name="sModule_ID"></param>
        /// <returns></returns>
        public static string GetParentScreenUrl(UserAccessBO objUserAccess) //string sChildScreenUrl
        {
            string sParentScreenUrl = string.Empty;

            using (SqlConnection Conn = new SqlConnection(objUserAccess.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@ChildScreenUrl",  objUserAccess.FilePath)
                    };

                    IDataReader dataReader = SqlHelper.ExecuteReader(Conn, GET_PARENT_SCREEN_URL, objParams);
                    while (dataReader.Read())
                    {
                        sParentScreenUrl = dataReader["Parent_Screen_Url"].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return sParentScreenUrl;
            }
        }
        #endregion

    }
}