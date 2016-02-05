using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackIT.Security
{
    public static class RoleAccessBLL
    {        
        public static RoleAccessBO GetRoleAccess(RoleAccessBO objRoleAccess)
        {
            try
            {
                return RoleAccessDAL.GetRoleAccess(objRoleAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static RoleAccessBO GetModulesByRoleID(RoleAccessBO objRoleAccess)
        {
            try
            {
                return RoleAccessDAL.GetModulesByRoleID(objRoleAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
