using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackIT.Common
{
    /// <summary>
    /// Summary description for StringFunctions
    /// </summary>
    public static class StringFunctions
    {
        #region Public Methods

        public static string ToString(string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            else
                return str;
        }

        public static string ToString(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return string.Empty;
            else
                return obj.ToString();
        }

        public static bool IsNullOrEmpty(object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return true;
            else
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                    return true;
                else
                    return false;
            }
        }

        public static string Nvl(object obj, string substituteValue)
        {
            if (obj == null || obj == DBNull.Value)
                return substituteValue;
            else
            {
                if (string.IsNullOrEmpty(obj.ToString()))
                    return substituteValue;
                else
                    return obj.ToString();
            }
        }

        #endregion
    }
}


