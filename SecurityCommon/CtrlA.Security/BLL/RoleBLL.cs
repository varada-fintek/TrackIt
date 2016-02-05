using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackIT.Security
{
    public static class RoleBLL
    {
        public static RoleBO InsertOrUpdateRoles(RoleBO objRole)
        {
            try
            {
                return RoleDAL.InsertOrUpdateRoles(objRole);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static RoleBO GetRoleDetails(RoleBO objRole)
        {
            try
            {
                return RoleDAL.GetRoleDetails(objRole);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static String DeleteRole(RoleBO objRole)
        {
            try
            {
                return RoleDAL.DeleteRole(objRole);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
