using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace TrackIT.Security
{
    public static class RoleAccessDAL
    {
        #region Private Variables

        private const string GET_ROLE_ACCESS = "Security_GetRoleAccess";
        private const string GETMODULES_BYROLEID = "Security_GetModulesByRoleID";

        #endregion

        #region GetRoleAccess
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objRoleAccess"></param>
        /// <returns></returns>
        public static RoleAccessBO GetRoleAccess(RoleAccessBO objRoleAccess)
        {
            using (SqlConnection Conn = new SqlConnection(objRoleAccess.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@RoleID",  objRoleAccess.RoleID)
                    };

                    objRoleAccess.dsResult = SqlHelper.ExecuteDataset(Conn, GET_ROLE_ACCESS, objParams);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objRoleAccess;
            }
        }
        #endregion

        #region GetModulesByRoleID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objRoleAccess"></param>
        /// <returns></returns>
        public static RoleAccessBO GetModulesByRoleID(RoleAccessBO objRoleAccess)
        {
            using (SqlConnection Conn = new SqlConnection(objRoleAccess.ConnectionString))
            {
                try
                {
                    SqlParameter[] objParams = { 
                        new SqlParameter ("@RoleID", objRoleAccess.RoleID),
                        new SqlParameter ("@ParentRoleID", objRoleAccess.ParentRoleID),
                    };

                    objRoleAccess.dsResult = SqlHelper.ExecuteDataset(Conn, GETMODULES_BYROLEID, objParams);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return objRoleAccess;
            }
        }
        #endregion
    }
}