using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.Globalization;

namespace TrackIT.Common
{
    public static class Utilities
    {
        public static string GetKey(string keyName)
        {
            string strReturn = string.Empty;
            try
            {
                strReturn = ConfigurationManager.AppSettings[keyName].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strReturn;
        }

        public static int GetAppSettingInt(string strAppSetting)
        {
            int PageSetting;
            try
            {
                PageSetting = Convert.ToInt32(ConfigurationManager.AppSettings[strAppSetting].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PageSetting;
        }


        #region GetAppSettingInt()
        /// <summary>
        /// Attempt to retrieve the specified Configuration Setting as an integer.  
        /// If the value is null or was not provided, then the default value 
        /// is returned.
        /// </summary>
        /// <param name="key">The appSetting key to look for.</param>
        /// <param name="defaultValue">The default value to return if no value was specified.</param>
        /// <returns>The value out of web.config or the default value if not found.</returns>
        public static int GetAppSettingInt(string key, int defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] != null &&
                ConfigurationManager.AppSettings[key].Trim().Length > 0)
            {
                try
                {
                    return (Convert.ToInt32(ConfigurationManager.AppSettings[key].Trim()));
                }
                catch { }
            }
            return (defaultValue);
        }
        #endregion

        #region GetAppSettingString()
        /// <summary>
        /// Attempt to retrieve the specified Configuration Setting as a string.  
        /// If the value is null or was not provided, then the default value 
        /// is returned.
        /// </summary>
        /// <param name="key">The appSetting key to look for.</param>
        /// <param name="defaultValue">The default value to return if no value was specified.</param>
        /// <returns>The value out of web.config or the default value if not found.</returns>
        public static string GetAppSettingString(string key, string defaultValue)
        {
            if (ConfigurationManager.AppSettings[key] != null &&
                ConfigurationManager.AppSettings[key].Length > 0)
            {
                try
                {
                    return (ConfigurationManager.AppSettings[key]);
                }
                catch { }
            }
            return (defaultValue);
        }
        #endregion

        #region "Validate Email Address"

        /// <summary>
        /// Returns whether or not the email address is a valid email address
        /// using regular expressions.
        /// </summary>
        /// 
        /// <param name="emailAddress">
        /// The email to check.
        /// </param>
        /// 
        /// <returns>
        /// True if the format of the email address is correct.
        /// </returns>
        public static bool IsValidEmailAddress(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
                return false;

            string strRegex = @"^([a-zA-Z0-9_'+*$%\^&!\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9:]{2,4})+$";
            Regex Rex = new Regex(strRegex);
            return Rex.IsMatch(emailAddress);
        }

        #endregion

    }
}
