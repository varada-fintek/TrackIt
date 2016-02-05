using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrackIT.Common
{
    /// <summary>
    /// Summary description for IntegerFunctions
    /// </summary>
    public static class IntegerFunctions
    {

        #region Public Methods

        public static int ToInt32(string value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch(Exception ex)
            {
                throw ex;                
            }
            
        }

        public static short ToInt16(string value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch (Exception ex)
            {
                throw ex;                
            }            
        }

        public static short ToInt16(object value)
        {
            try
            {
                return Convert.ToInt16(value);
            }
            catch (Exception ex)
            {
                throw ex;                
            }            
        }
        public static long ToInt64(string value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception ex)
            {
                throw ex;                
            }
            
        }

        public static int ToInt32(object value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex)
            {
                throw ex;                
            }            
        }

        public static long ToInt64(object value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception ex)
            {
                throw ex;                
            }            
        }
        #endregion
    }
}
