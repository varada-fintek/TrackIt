using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Data;

namespace TrackIT.Common
{
    public static class Conversion
    {
        #region "Convert Object To Byte"

        public static byte[] ObjectToByte(object objItem)
        {
            byte[] bArray;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();                    
                    binFormatter.Serialize(ms, objItem);
                    bArray = ms.ToArray();
                    ms.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bArray;
        }

        #endregion

        #region "Convert Object To Byte With Compression"

        public static byte[] ObjectToByte(object objItem, bool compress)
        {
            byte[] bArray;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    if (compress)
                        using (GZipStream _gzipStream = new GZipStream(ms, CompressionMode.Compress))
                        {
                            binFormatter.Serialize(_gzipStream, objItem);
                        }
                    else
                        binFormatter.Serialize(ms, objItem);
                    bArray = ms.ToArray();
                    ms.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bArray;
        }

        #endregion

        #region "Convert Byte To Object"

        public static object ByteToObject(byte[] bArray)
        {
            object objItem = new object();

            try
            {
                using (MemoryStream ms = new MemoryStream(bArray))
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    objItem = (object)binFormatter.Deserialize(ms);
                    ms.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objItem;
        }
        
        #endregion

        #region "Convert Byte To Object With Compression"

        public static object ByteToObject(byte[] bArray, bool compress)
        {
            object objItem = new object();

            try
            {
                using (MemoryStream ms = new MemoryStream(bArray))
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    if (compress)
                        using (GZipStream _gzipStream = new GZipStream(ms, CompressionMode.Decompress))
                        {
                            objItem = (object)binFormatter.Deserialize(_gzipStream);
                        }
                    else
                        objItem = (object)binFormatter.Deserialize(ms);
                    ms.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return objItem;
        }

        #endregion

        #region "Convert String to Byte"

        public static byte[] StringToByte(string strValue)
        {
            byte[] bValue;
            string[] arrBytes;
            int nCounter;

            try
            {
                arrBytes = strValue.Split(';');

                bValue = new byte[arrBytes.Length - 1];

                for (nCounter = 0; nCounter < arrBytes.Length - 1; nCounter++)
                {

                    if (arrBytes[nCounter] == "0")
                    {
                        bValue[nCounter] = 0;
                    }
                    else
                    {
                        bValue[nCounter] = Byte.Parse(arrBytes[nCounter]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bValue;
        }

        #endregion

        #region "Convert Byte to String"

        public static string ByteToString(byte[] bValue)
        {
            string strValue = string.Empty;
            int nCounter;

            try
            {
                for (nCounter = 0; nCounter < bValue.Length; nCounter++)
                {
                    if (bValue[nCounter] <= 0)
                    {
                        strValue += "0;";
                    }
                    else
                    {
                        strValue += bValue[nCounter].ToString();
                        strValue += ";";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strValue;
        }

        public static string ByteToStringFormat(object objRowVersion)
        {
            string strResult = string.Empty;

            try
            {
                if (!StringFunctions.IsNullOrEmpty(objRowVersion))
                    strResult = Conversion.ByteToString((byte[])objRowVersion);
                else
                {
                    byte[] arr = new byte[8];
                    strResult = Conversion.ByteToString(arr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strResult;
        }

        #endregion

        #region "Convert Byte to DataSet"
        
        public static DataSet ByteToDataset(byte[] bLibrary)
        {
            DataSet dsMaterial = new DataSet();

            try
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(bLibrary);

                dsMaterial = (DataSet)bformatter.Deserialize(stream);
                stream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsMaterial;
        }

        #endregion

        #region "Convert DataSet to Byte"

        public static byte[] DataSetToByte(DataSet objDs)
        {
            byte[] bArray;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter binFormatter = new BinaryFormatter();
                    binFormatter.Serialize(ms, objDs);
                    bArray = ms.ToArray();
                    ms.Flush();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return bArray;
        }

        #endregion

        #region "Convert DataView to DataTable"

        public static System.Data.DataTable DataViewToDataTable(System.Data.DataView obDataView)
        {
            if (null == obDataView)
            {
                throw new ArgumentNullException
                ("DataView", "Invalid DataView object specified");
            }

            System.Data.DataTable obNewDt = obDataView.Table.Clone();
            int idx = 0;

            try
            {
                string[] strColNames = new string[obNewDt.Columns.Count];
                foreach (System.Data.DataColumn col in obNewDt.Columns)
                {
                    strColNames[idx++] = col.ColumnName;
                }

                System.Collections.IEnumerator viewEnumerator = obDataView.GetEnumerator();
                while (viewEnumerator.MoveNext())
                {
                    System.Data.DataRowView drv = (System.Data.DataRowView)viewEnumerator.Current;
                    System.Data.DataRow dr = obNewDt.NewRow();
                    try
                    {
                        foreach (string strName in strColNames)
                            dr[strName] = drv[strName];
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    obNewDt.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return obNewDt;
        }

        #endregion

        #region "Convert String to Guid"

        public static Guid? ConvertStringToGuid(string sValue)
        {
            Guid? guidNew = null;

            try
            {
                if (!string.IsNullOrEmpty(sValue))
                    guidNew = new Guid(sValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return guidNew;
        }

        #endregion       
        
        #region "Convert String to Byte"

        #endregion
    }
}
