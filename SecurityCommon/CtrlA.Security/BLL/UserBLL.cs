using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TrackIT.Security
{
    public static class UserBLL
    {
        public static UserBO InsertOrUpdateUsers(UserBO objUser)
        {
            try
            {
                return UserDAL.InsertOrUpdateUsers(objUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static UserBO ChangePassword(UserBO objUser)
        {
            try
            {
                return UserDAL.ChangePassword(objUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static UserBO GetLoginDetails(UserBO objUser)
        {
            try
            {
                return UserDAL.GetLoginDetails(objUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static UserBO GetChangePasswordDetails(UserBO objUser)
        {
            try
            {
                return UserDAL.GetChangePasswordDetails(objUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static UserBO GetStudentLoginDetails(UserBO objUser)
        {
            try
            {
                return UserDAL.GetStudentLoginDetails(objUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static UserBO GetUserDetails(UserBO objUser)
        {
            try
            {
                return UserDAL.GetUserDetails(objUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string DeleteUser(UserBO objUser)
        {
            try
            {
                return UserDAL.DeleteUser(objUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void InsertLoginDetails(UserBO objUser)
        {
            UserDAL.InsertLoginDetails(objUser);
        }
    }
}
