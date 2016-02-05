using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using System.Net;
using System.Configuration;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Data.OleDb;
using System.Reflection;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.ComponentModel;

using Microsoft.Office.Interop.Excel;

namespace TrackIT.Common
{
    public static class CommonFunctions
    {   
        public static Hashtable GetEnumForBind(Type enumeration)
        {
            Hashtable ht = new Hashtable();
            try
            {
                string[] names = Enum.GetNames(enumeration);
                Array values = Enum.GetValues(enumeration);
                
                for (int i = 0; i < names.Length; i++)
                {
                    ht.Add(Convert.ToInt32(values.GetValue(i)).ToString(), names[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ht;
        }

        public static Hashtable GetEnumDescriptionForBind(Type enumeration)
        {
            Hashtable ht = new Hashtable();
            try
            {
                string[] names = Enum.GetNames(enumeration);
                Array values = Enum.GetValues(enumeration);

                for (int i = 0; i < names.Length; i++)
                    ht.Add(Convert.ToInt32(values.GetValue(i)).ToString(), GetDescription((Enum)Enum.Parse(enumeration, names[i])));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ht;
        }

        public static string GetDescription(Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return string.Empty;
        }

        public static System.Data.DataTable GetEnumDataTableForBind(Type EnumType)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            try
            {
                //Get the Name (String) values from the Enum
                string[] EnumNames = System.Enum.GetNames(EnumType);

                //Get the Values (Integer) from the Enum
                Array EnumValues = System.Enum.GetValues(EnumType);

                //Declare a new DataTable with column names "key" and "value"

                dt.Columns.Add("key", typeof(int));
                dt.Columns.Add("value", typeof(string));

                //Loop through all of the name,value pairs in the Enum and add them as new rows to the DataTable
                for (int i = 0; i <= EnumNames.Length - 1; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["key"] = EnumValues.GetValue(i);
                    //dr["value"] = EnumNames[i];
                    dr["value"] = GetDescription((Enum)Enum.Parse(EnumType, EnumNames[i]));
                    dt.Rows.Add(dr);
                }

                //Sort the DataTable by the value column
                DataView dv = dt.DefaultView;
                dv.Sort = "key asc";

                //Apply the sorted DataView back to the DataTable to be returned
                dt = dv.ToTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
                
        public static string UrlRoot
        {
            get
            {
                string newUrl;
                try
                {
                    Uri url = HttpContext.Current.Request.Url;                    
                    newUrl = string.Format("{0}{1}{2}", url.Scheme, Uri.SchemeDelimiter, url.Authority);                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return newUrl;
            }
        }

        public static string VirtualPath
        {
            get
            {
                string path = string.Empty;
                try
                {
                    path = HttpRuntime.AppDomainAppVirtualPath;
                    if (!path.StartsWith("/"))
                    {
                        path = "/" + path;
                    }
                    if (!path.EndsWith("/"))
                    {
                        path += "/";
                    }                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return path;
            }
        }
                
        public static string ApplicationFolder
        {
            get
            {
                string newUrl = string.Empty;
                try
                {
                    string[] segments = HttpContext.Current.Request.Url.Segments;
                    
                    for (int x = 0; x < segments.Length - 1; x++)
                    {
                        newUrl += segments[x];
                    }
                    string virtualPath = VirtualPath;
                    if (newUrl.StartsWith(virtualPath))
                    {
                        newUrl = newUrl.Substring(virtualPath.Length);
                    }                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return newUrl;
            }
        }       

        public static string parseString(string str)
        {
            string sOutput = string.Empty;
            try
            {
                if (null != str)
                {
                    sOutput = Convert.ToString(str.Trim());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sOutput;
        }        
        
        public static string Left(string param, int length)
        {
            string result = string.Empty;
            try
            {
                //we start at 0 since we want to get the characters starting from the
                //left and with the specified lenght and assign it to a variable
                result = param.Substring(0, length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static string Right(string param, int length)
        {
            string result = string.Empty;
            try
            {
                //start at the index based on the lenght of the sting minus
                //the specified lenght and assign it a variable
                //string result = param.Substring((param.Length() – length), length);
                result = param.Substring(param.Length - length, length);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static string Mid(string param, int startIndex, int length)
        {
            string result = string.Empty;
            try
            {
                //start at the specified index in the string ang get N number of
                //characters depending on the lenght and assign it to a variable
                result = param.Substring(startIndex, length);                
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            return result;
        }

        public static DataSet GetExcelDatas(string sUploadFilePath)
        {
            //KillProcess("EXCEL");
            Application oXL = null;
            Workbook oWB = null;
            Worksheet oSheet = null;
            Range oRng = null;
            DataSet ds = new DataSet();

            try
            {
                //Create a Application object    
                oXL = new ApplicationClass();

                //Getting WorkBook object
                oWB = oXL.Workbooks.Open(sUploadFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                    Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                //Getting WorkSheet object    
                oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oWB.Sheets[1];
                System.Data.DataTable dt = new System.Data.DataTable("dtExcel");

                ds.Tables.Add(dt);
                DataRow dr;

                int jValue = oSheet.UsedRange.Cells.Columns.Count;
                int iValue = oSheet.UsedRange.Cells.Rows.Count;

                //Getting Data Columns
                for (int j = 1; j <= jValue; j++)
                {
                    dt.Columns.Add("column" + j, System.Type.GetType("System.String"));
                }

                //Getting Data in Cell    
                for (int i = 1; i <= iValue; i++)
                {
                    dr = ds.Tables["dtExcel"].NewRow();
                    for (int j = 1; j <= jValue; j++)
                    {
                        oRng = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[i, j];
                        string strValue = oRng.Text.ToString();
                        dr["column" + j] = strValue;
                    }
                    ds.Tables["dtExcel"].Rows.Add(dr);
                }

                //ds.Tables["dtExcel"].Rows.RemoveAt(0);
                //DataSet dsNew = new DataSet();
                //System.Data.DataTable dtNew = new System.Data.DataTable();
                //dsNew.Tables.Add(dtNew);

                //for (int i = 1; i < ds.Tables["dtExcel"].Columns.Count; i++)
                //{
                //    dsNew.Tables[0].Columns.Add(ds.Tables["dtExcel"].Rows[0][i].ToString());
                //}
                //for (int j = 1; j < ds.Tables["dtExcel"].Rows.Count; j++)
                //{
                //    DataRow drNew = ds.Tables["dtExcel"].NewRow();
                //    dsNew.Tables[0].Rows.Add(drNew);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Release the Excel objects
                oWB.Close(false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                oXL.Workbooks.Close();
                oXL.Quit();
                oXL = null;
                oWB = null;
                oSheet = null;
                oRng = null;
                GC.GetTotalMemory(false);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                GC.GetTotalMemory(true);
                //Release the Excel objects
            }
            return ds;
        }
    }

    [Serializable]
    public static class DBHelper
    {
        public static string DBString(Object s)
        {
            try
            {
                if (s is DBNull)
                    s = string.Empty;

                return Convert.ToString(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime DBDate(Object dt)
        {
            try
            {
                DateTime dateTime = DateTime.MinValue;
                if (dt is DBNull)
                    return dateTime;
                else
                    dateTime = (DateTime)dt;

                return Convert.ToDateTime(dateTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DateTime DBDate(string sDate)
        {
            string lang = "en-US";
            System.Globalization.CultureInfo cInfo = new System.Globalization.CultureInfo(lang);

            DateTime retDatetime = DateTime.MinValue;
            string[] expectedFormats = { "dd/MM/yyyy", "dd/MM/yyyy" };

            try
            {
                retDatetime = DateTime.ParseExact(sDate, expectedFormats, cInfo, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDatetime;
        }

        public static Boolean DBBoolean(Object bol)
        {
            try
            {
                if (bol is DBNull)
                    return false;
                else
                    bol = Convert.ToBoolean(bol);
                return (Boolean)bol;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}