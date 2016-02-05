using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data;

namespace TrackIT.Security
{
    public static class RoleDAL
    {
        #region Private Variables

        private const string INSERT_OR_UPDATE_ROLES = "Security_InsertOrUpdateRoles";
        private const string GET_ROLES = "Security_GetRoleDetails";
        private const string DELETE_RECORD = "Security_DeleteRole";
        private const int OUTMESSAGE_SIZE = 1000;

        #endregion

        public static RoleBO InsertOrUpdateRoles(RoleBO objRole)
        {
            using (SqlConnection Conn = new SqlConnection(objRole.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = {
                        new SqlParameter ("@RoleID", objRole.RoleID),
                        new SqlParameter ("@RoleCode", objRole.RoleCode),
                        new SqlParameter ("@RoleName", objRole.RoleName),
                        //new SqlParameter ("@BUID", objRole.BUID),
                        new SqlParameter ("@RoleType", objRole.RoleType),
                        new SqlParameter ("@Active", objRole.Active),
                        new SqlParameter ("@XMLData", objRole.XMLData),
                        new SqlParameter ("@CreatedBy", objRole.CreatedBy),
                        new SqlParameter ("@RowVersion", objRole.RowVersion)
                    };
                    objRole.dsResult = SqlHelper.ExecuteDataset(Conn, INSERT_OR_UPDATE_ROLES, objParams);
                    if (objRole.dsResult.Tables[0].Rows.Count > 0)
                    {
                        objRole.OutMessage = objRole.dsResult.Tables[0].Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
                return objRole;
            }
        }

        public static RoleBO GetRoleDetails(RoleBO objRole)
        {
            using (SqlConnection Conn = new SqlConnection(objRole.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@guidLoginUserID", objRole.LoginUserID),
                        new SqlParameter ("@bitIsAdminRole", objRole.IsAdminRole),
                        new SqlParameter ("@RoleID", objRole.RoleID),
                        new SqlParameter ("@BUID", objRole.BUID),
                        new SqlParameter ("@RoleName", objRole.RoleName),
                        new SqlParameter ("@BUName", objRole.BUName),
                        new SqlParameter ("@Order", objRole.SortOrder),
                        new SqlParameter ("@Pageindex", objRole.PageIndex),
                        new SqlParameter ("@Pagesize",  objRole.PageSize),
                        new SqlParameter ("@SortBy", objRole.SortBy)
                    };

                    if (!string.IsNullOrEmpty(objRole.RoleID.ToString()))
                    {
                        IDataReader dataReader = SqlHelper.ExecuteReader(Conn,GET_ROLES, objParams);
                        while (dataReader.Read())
                        {
                            if (!string.IsNullOrEmpty(dataReader["Role_ID"].ToString()))
                                objRole.RoleID = new Guid(dataReader["Role_ID"].ToString());
                            else
                                objRole.RoleID = null;

                            //if (!string.IsNullOrEmpty(dataReader["BU_ID"].ToString()))
                            //    objRole.BUID = new Guid(dataReader["BU_ID"].ToString());
                            //else
                            //    objRole.BUID = null;

                            objRole.RoleCode = dataReader["Role_Code"].ToString();
                            objRole.RoleName = dataReader["Role_Name"].ToString();
                            objRole.RoleType = dataReader["Role_Type"].ToString();
                            objRole.Active = Convert.ToBoolean(dataReader["Active"]);
                            objRole.is_Used = Convert.ToBoolean(dataReader["Is_Used"]) == true ? 1 : 0;
                            objRole.RowVersion = (byte[])dataReader["ROW_VERSION"];
                        }
                    }
                    else
                    {
                        objRole.dsRole = SqlHelper.ExecuteDataset(Conn, GET_ROLES, objParams);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objRole;
            }
        }

        public static string DeleteRole(RoleBO objRole)
        {
            using (SqlConnection Conn = new SqlConnection(objRole.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = {
                        new SqlParameter ("@RoleID", objRole.RoleID),                        
                        new SqlParameter ("@CreatedBy", objRole.CreatedBy)                        
                    };

                    objRole.dsResult = SqlHelper.ExecuteDataset(Conn, DELETE_RECORD, objParams);
                    if (objRole.dsResult.Tables[0].Rows.Count > 0)
                    {
                        objRole.OutMessage = objRole.dsResult.Tables[0].Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }

                return objRole.OutMessage;
            }
        }
    }
}
