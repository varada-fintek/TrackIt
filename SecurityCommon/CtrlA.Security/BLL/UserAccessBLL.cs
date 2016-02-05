using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TrackIT.Security
{
    public static class UserAccessBLL
    {
        public static UserAccessBO GetUserAccess(UserAccessBO objUserAccess)
        {
            try
            {
                return UserAccessDAL.GetUserAccess(objUserAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetModuleAccessRights(UserAccessBO objUserAccess)
        {
            try
            {
                return UserAccessDAL.GetModuleAccessRights(objUserAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable GetScreenAccessRights(UserAccessBO objUserAccess)
        {
            try
            {
                return UserAccessDAL.GetScreenAccessRights(objUserAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //public static DataTable GetStudentScreenAccessRights(UserAccessBO objUserAccess)
        //{
        //    try
        //    {
        //        return UserAccessDAL.GetStudentScreenAccessRights(objUserAccess);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public static DataTable GetScreenAuthentication(UserAccessBO objUserAccess)
        {
            try
            {
                return UserAccessDAL.GetScreenAuthentication(objUserAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetTreeViewMenuDetails(UserAccessBO objUserAccess)
        {
            try
            {
                return UserAccessDAL.GetTreeViewMenuDetails(objUserAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetParentScreenUrl(UserAccessBO objUserAccess)
        {
            try
            {
                return UserAccessDAL.GetParentScreenUrl(objUserAccess);
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
