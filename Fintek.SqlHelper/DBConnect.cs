using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DBHelper;
using System.Configuration;
using System.Data.OleDb;
using System.Web;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common;


using Microsoft.ApplicationBlocks.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace DBHelper
{
    public class DBConnect
    {
        private string istr_context;
        public string iconnectionstring=ConfigurationManager.ConnectionStrings[1].ToString();
        public SqlConnection isqlcon_connection;
        public OleDbConnection ObledbC;
        public SqlTransaction isqltrans_Tr;
        public int iint_Timeout = 120000;
        private IList<SqlParameter> ilst_Params;

        public DBConnect()
        {
            isqlcon_connection = new SqlConnection(iconnectionstring);
            ilst_Params = new List<SqlParameter>();
        }
        public string SqlInsert(string astr_tablename, IDictionary<string, object> adict_parameterMap,string astr_type)
           {
            SqlConnection lsqlcon_connection = new SqlConnection(iconnectionstring); //("Data Source=172.16.1.234;Initial Catalog=TrackIT_Dev; USER ID=TrackIT; PASSWORD=TrackIT@123");
            SqlCommand lsqlcmd_command;
            string lstr_id = string.Empty;
            try
            {
                lsqlcon_connection.Open();
                isqltrans_Tr = lsqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = lsqlcon_connection;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                lsqlcmd_command.CommandText = CreateInsertSql(astr_tablename, adict_parameterMap);
                foreach (var lvar_pair in adict_parameterMap)
                    lsqlcmd_command.Parameters.AddWithValue(lvar_pair.Key, lvar_pair.Value);

                
                //lsqlcmd_command.Connection = C;
                if (astr_type == "scope")
                {
                    lsqlcmd_command.CommandText += ";SELECT SCOPE_IDENTITY();";
                    lsqlcmd_command.CommandType = CommandType.Text;
                  lstr_id = lsqlcmd_command.ExecuteScalar().ToString();
                }
                else
                {
                    lsqlcmd_command.CommandType = CommandType.Text;
                    lsqlcmd_command.ExecuteNonQuery();
                    lstr_id = "SUCCESS^";
                }
                isqltrans_Tr.Commit();
                lsqlcon_connection.Close();
                isqltrans_Tr.Dispose();
                return lstr_id;                    
            }
            catch (Exception ex)
            {
                if (ExceptionPolicy.HandleException(ex, "Rethrow_Policy")) throw;
                lsqlcon_connection.Close();
               // if (ExceptionPolicy.HandleException(ex, "Rethrow_Policy")) throw;
                isqltrans_Tr.Dispose();
                return ex.Message;

            }
            //finally
            //{
            //    isqlcon_connection.Close();
            //    isqlcon_connection.Dispose();
            //}
        }

        private static string CreateInsertSql(string table,
                                      IDictionary<string, object> parameterMap)
        {
            var keys = parameterMap.Keys.ToList();
            // ToList() LINQ extension method used because order is NOT
            // guaranteed with every implementation of IDictionary<TKey, TValue>

           
                var sql = new StringBuilder("INSERT INTO ").Append(table).Append("(");

                for (var i = 0; i < keys.Count; i++)
                {
                    sql.Append(keys[i]);
                    if (i < keys.Count - 1)
                        sql.Append(", ");
                }

                sql.Append(") VALUES(");

                for (var i = 0; i < keys.Count; i++)
                {
                    sql.Append("@").Append(keys[i]);
                    if (i < keys.Count - 1)
                        sql.Append(", ");
                }
                return sql.Append(")").ToString();
           
           
         
        }


        public string SqlUpdate(string table, IDictionary<string, object> parameterMap,IDictionary<string, object> parameterMapforwhere, string type)
        {
            SqlConnection con = new SqlConnection(iconnectionstring); 
            //String strConnString = SqlConnection("Data Source=192.168.1.96;Initial Catalog=PMTS;User ID=pmts;Password=pmts;");
            
            SqlCommand lsqlcmd_command;
            string id = string.Empty;
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                lsqlcmd_command.CommandText = updateSQL(table, parameterMap, parameterMapforwhere);
                foreach (var pair in parameterMap)
                    lsqlcmd_command.Parameters.AddWithValue(pair.Key, pair.Value);
                foreach (var pair in parameterMapforwhere)
                    lsqlcmd_command.Parameters.AddWithValue(pair.Key, pair.Value);

                lsqlcmd_command.CommandType = CommandType.Text;
                //lsqlcmd_command.Connection = C;
                if (type == "scope")
                    id = (string)lsqlcmd_command.ExecuteScalar();
                else
                {
                    lsqlcmd_command.ExecuteNonQuery();
                    id = "SUCCESS^";
                }
                isqltrans_Tr.Commit();
                isqlcon_connection.Close();
                con.Close();
                return id;
            }
            catch (Exception ex)
            {
                isqlcon_connection.Close();
                return "Error";

            }
            finally
            {
                con.Close();
                con.Dispose();
                isqlcon_connection.Close();
            }
        }



   public static String updateSQL( string table, IDictionary<string, object> columnValueMappingForSet, IDictionary<string, object> columnValueMappingForCondition) 
   {
    var updateQueryBuilder = new StringBuilder();
     
    /**
     * Removing column that holds NULL value or Blank value...
     */
    if (columnValueMappingForSet!=null) 
    {
          foreach (var pair in columnValueMappingForSet)
          {
              if(pair.Key==null || pair.Value==null)
              {
                  columnValueMappingForSet.Remove(pair);
              }
              }
    }

    /**
     * Removing column that holds NULL value or Blank value...
     */
   if (columnValueMappingForCondition!=null) 
    {
          foreach (var pair in columnValueMappingForCondition)
          {
              if(pair.Key==null || pair.Value==null)
              {
                  columnValueMappingForCondition.Remove(pair);
              }
              }
    }

    var SetKeys = columnValueMappingForSet.Keys.ToList();
       var SetValues = columnValueMappingForSet.Values.ToList();
       var WhereKeys = columnValueMappingForCondition.Keys.ToList();
       var WhereValues = columnValueMappingForCondition.Values.ToList();
    /* Making the UPDATE Query */
    updateQueryBuilder.Append("UPDATE");
    updateQueryBuilder.Append(" ").Append(table);
    updateQueryBuilder.Append(" SET");
    updateQueryBuilder.Append(" ");

    if (columnValueMappingForSet!=null) {
        for(var i=0;i<SetKeys.Count;i++)
        {
            updateQueryBuilder.Append(SetKeys[i]).Append("=").Append("@").Append(SetKeys[i]);
                    if (i < SetKeys.Count - 1)
                        updateQueryBuilder.Append(",");
        }
        //foreach (var pair in columnValueMappingForSet) {
        //    updateQueryBuilder.Append(pair.Key).Append("=").Append("@").Append(pair.Value);
        //    updateQueryBuilder.Append(",");
        //}
    }

    //updateQueryBuilder = new StringBuilder(updateQueryBuilder.subSequence(0, updateQueryBuilder.length() - 1));
    updateQueryBuilder.Append(" WHERE");
    updateQueryBuilder.Append(" ");

    if (columnValueMappingForCondition!=null) {
         for(var i=0;i<WhereKeys.Count;i++)
        {
            updateQueryBuilder.Append(WhereKeys[i]).Append("=").Append("@").Append(WhereKeys[i]);
                    if (i < WhereKeys.Count - 1)
                        updateQueryBuilder.Append(",");
        }
        
    }

    //updateQueryBuilder = new StringBuilder(updateQueryBuilder.subSequence(0, updateQueryBuilder.length() - 1));

    // Returning the generated UPDATE SQL Query as a String...
    return updateQueryBuilder.ToString();
}

        public DBConnect(string constr, bool excelcon)
        {
            istr_context = constr;
            if (excelcon == true)
            {
                ObledbC = new OleDbConnection(istr_context);
                //_Params=new 
            }
            else
            {
                isqlcon_connection = new SqlConnection(istr_context);
            }
        }
        //Method to check db connectivity is there
        public Boolean DBConnectivity()
        {
            try
            {
                Boolean ConnectionOpen = false;
                isqlcon_connection.Open();
                if( isqlcon_connection.State == ConnectionState.Open)
                {
                    ConnectionOpen = true;
                    isqlcon_connection.Close();
                }
                else
                {
                    ConnectionOpen = false;
                }

                return ConnectionOpen;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public DataSet ExecuteSp(string astr_spName, SqlParameter[] asqlpar_parameter)
        {
            SqlConnection lsqlcon_connection = new SqlConnection(iconnectionstring);
            
           // SqlCommand lsqlcmd_command;
            DataSet lds_dsResult=null;
            string id = string.Empty;
            try
            {
                lsqlcon_connection.Open();
                //isqltrans_Tr = lsqlcon_connection.BeginTransaction();
                //lsqlcmd_command = new SqlCommand();
                //lsqlcmd_command.CommandType = CommandType.Text;
                //lsqlcmd_command.Connection = lsqlcon_connection;
                //lsqlcmd_command.CommandTimeout = iint_Timeout;
                //lsqlcmd_command.Transaction = isqltrans_Tr;
                lds_dsResult = SqlHelper.ExecuteDataset(lsqlcon_connection, astr_spName, asqlpar_parameter);
               
                return lds_dsResult;

            }
            catch (Exception ex)
            {
                lsqlcon_connection.Close();
                return lds_dsResult;

            }
            finally
            {
                lsqlcon_connection.Close();
                lsqlcon_connection.Dispose();
                lsqlcon_connection.Close();
            }
           
            
        }

        //Method to return string value with out Parameter
        public string ExecuteReader(string SqlText)
        {
            SqlCommand lsqlcmd_command;
            string retrunValue = "";
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                lsqlcmd_command.CommandText = SqlText;
                retrunValue = Convert.ToString(lsqlcmd_command.ExecuteScalar());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (retrunValue);
        }

        //Method to return string value with Parameter
        public string ExecuteReader(string SqlText, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            string retrunValue = "";
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                lsqlcmd_command.CommandText = SqlText;
                BuildParameters(ref lsqlcmd_command, Parameters);
                retrunValue = Convert.ToString(lsqlcmd_command.ExecuteScalar());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;

            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (retrunValue);
        }

        //Method for Return Dataset Value
        public DataSet ExecuteDataSet(string SqlText)
        {
            SqlCommand lsqlcmd_command;
            SqlConnection con = new SqlConnection(iconnectionstring);
            DataSet retrunDataSet = new DataSet();
            try
            {
                con.Open();
                isqltrans_Tr = con.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = con;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(retrunDataSet);
                isqltrans_Tr.Commit();

            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
                
            }
            finally
            {
                con.Close();
            }
            return (retrunDataSet);
        }

        //Method for Return Dataset Value
        public DataSet ExecuteDataSetTableName(string SqlText, string tablename, DataSet objdataset)
        {
            SqlCommand lsqlcmd_command;
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(objdataset, tablename);
                isqltrans_Tr.Commit();

            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (objdataset);
        }


        //Method for Return Dataset Value
        public DataSet ExecuteDataSetTableName(string SqlText, string tablename, DataSet objdataset, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(objdataset, tablename);
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (objdataset);
        }

        //Method for Return Dataset Value with Parameter
        public DataSet ExecuteDataSet(string SqlText, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            DataSet retrunDataSet = new DataSet();
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(retrunDataSet);
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (retrunDataSet);
        }

        //Method for Insert,Update,Delete
        public int ExecuteNonQuery(string SqlText)
        {
            SqlCommand lsqlcmd_command;
            int i_AffectedRecords = 0;
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                i_AffectedRecords = Convert.ToInt32(lsqlcmd_command.ExecuteNonQuery());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
                
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (i_AffectedRecords);
        }

        //Method for Insert,Update,Delete
        public int ExecuteNonQuery(string SqlText, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            int i_AffectedRecords = 0;
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                i_AffectedRecords = Convert.ToInt32(lsqlcmd_command.ExecuteNonQuery());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (i_AffectedRecords);
        }

        //Method for Return scalar Value
        public string ExecuteScalar(string SqlQuery)
        {
            SqlCommand lsqlcmd_command;
            string retrunValue = "";
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlQuery;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                retrunValue = Convert.ToString(lsqlcmd_command.ExecuteScalar());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (retrunValue);
          
        }

        
       
        
        //Method for Return scalar Value
        public string ExecuteScalar(string SqlQuery, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            string retrunValue = "";
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlQuery;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                retrunValue = Convert.ToString(lsqlcmd_command.ExecuteScalar());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (retrunValue);
        }


        //Method for Return the RecordCount
        public Int64 ExecuteRecordCount(String SqlText)
        {
            SqlCommand lsqlcmd_command;
            Int64 RecordCount = 0;
            DataSet retrunDataSet = new DataSet();
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(retrunDataSet);
                isqltrans_Tr.Commit();
                RecordCount = retrunDataSet.Tables[0].Rows.Count;

            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;

            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (RecordCount);

        }

        //Method for Return the RecordCount
        public Int64 ExecuteRecordCount(string SqlText, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            Int64 RecordCount = 0;
            DataSet retrunDataSet = new DataSet();
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.Text;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SqlText;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(retrunDataSet);
                isqltrans_Tr.Commit();
                RecordCount = retrunDataSet.Tables[0].Rows.Count;
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (RecordCount);
        }

        public SqlDataReader ReturnDataReader(string QueryString)
        {
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                SqlCommand myCommand = new SqlCommand(QueryString, isqlcon_connection);
                SqlDataReader oReader;
                myCommand.Transaction = isqltrans_Tr;
                oReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
                isqltrans_Tr.Commit();
                return oReader;
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
        }

        //Method to return value in DataSet without Parameter
        public DataSet ExecuteDataSetSP(string SpName)
        {
            SqlCommand lsqlcmd_command;
            DataSet dsResult = new DataSet();
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.StoredProcedure;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SpName;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                SqlParameter[] param = new SqlParameter[ilst_Params.Count];
                ilst_Params.CopyTo(param, 0);
                lsqlcmd_command.Parameters.AddRange(param);
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(dsResult);
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();                
                throw SqlExc;                
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (dsResult);
        }
        
        //Method to return value in DataSet with Parameter
        public DataSet ExecuteDataSetSP(string SpName, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            DataSet dsResult = new DataSet();
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.StoredProcedure;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SpName;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                SqlDataAdapter Adapter = new SqlDataAdapter(lsqlcmd_command);
                Adapter.Fill(dsResult);
                isqltrans_Tr.Commit();

            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();

                throw SqlExc;                
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (dsResult);
        }
        
        //Method for Insert,Update,Delete
        public int ExecuteNonQuerySP(string SpName)
        {
            SqlCommand lsqlcmd_command;
            int i_AffectedRecords = 0;
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.StoredProcedure;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SpName;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                SqlParameter[] param = new SqlParameter[ilst_Params.Count];
                ilst_Params.CopyTo(param, 0);
                lsqlcmd_command.Parameters.AddRange(param);
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                i_AffectedRecords = Convert.ToInt32(lsqlcmd_command.ExecuteNonQuery());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }

            return (i_AffectedRecords);
        }

        //Method for Insert,Update,Delete
        public int ExecuteNonQuerySP(string SpName, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            int i_AffectedRecords = 0;
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.StoredProcedure;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SpName;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                i_AffectedRecords = Convert.ToInt32(lsqlcmd_command.ExecuteNonQuery());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (i_AffectedRecords);
        }

        //Method for Return scalar Value
        public string ExecuteScalarSP(string SpName)
        {
            SqlCommand lsqlcmd_command;
            string retrunValue = "";
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.StoredProcedure;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SpName;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                retrunValue = Convert.ToString(lsqlcmd_command.ExecuteScalar());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (retrunValue);
        }

        //Method for Return scalar Value
        public string ExecuteScalarSP(string SpName, InParameters[] Parameters)
        {
            SqlCommand lsqlcmd_command;
            string retrunValue = "";
            try
            {
                isqlcon_connection.Open();
                isqltrans_Tr = isqlcon_connection.BeginTransaction();
                lsqlcmd_command = new SqlCommand();
                lsqlcmd_command.CommandType = CommandType.StoredProcedure;
                lsqlcmd_command.Connection = isqlcon_connection;
                lsqlcmd_command.CommandText = SpName;
                lsqlcmd_command.CommandTimeout = iint_Timeout;
                lsqlcmd_command.Transaction = isqltrans_Tr;
                BuildParameters(ref lsqlcmd_command, Parameters);
                retrunValue = Convert.ToString(lsqlcmd_command.ExecuteScalar());
                isqltrans_Tr.Commit();
            }
            catch (Exception SqlExc)
            {
                isqltrans_Tr.Rollback();
                throw SqlExc;
            }
            finally
            {
                isqlcon_connection.Close();
            }
            return (retrunValue);
        }

        public DataSet OLEDBExecuteDataSet(string SqlText)
        {
            OleDbCommand oledbcmd;
            DataSet retrunDataSet = new DataSet();
            try
            {
                ObledbC.Open();
                oledbcmd  = new OleDbCommand();
                oledbcmd.CommandType = CommandType.Text;
                oledbcmd.Connection = ObledbC;
                oledbcmd.CommandText = SqlText;
                oledbcmd.CommandTimeout = iint_Timeout;
                OleDbDataAdapter oledbAd = new OleDbDataAdapter(oledbcmd);
                oledbAd.Fill(retrunDataSet);
            }
            catch (OleDbException oledbExc)
            {
                throw oledbExc;
            }
            finally
            {
                ObledbC.Close();
            }
            return (retrunDataSet);
        }
        public void AddParameter(string paramName, string paramValue, SqlDbType paramType)
        {
            SqlParameter _param = new SqlParameter();
            _param.ParameterName = paramName;
            _param.Value = paramValue;
            _param.SqlDbType = paramType;
            ilst_Params.Add(_param);
        }
        private void BuildParameters(ref SqlCommand lsqlcmd_command, InParameters[] InputParameters)
        {
            foreach (InParameters param in InputParameters)
            {
                SqlParameter tempParam = new SqlParameter(param.ParamName, param.SqlDataType);
                if (param.ParamValue == null || param.ParamValue == "")
                    tempParam.Value = DBNull.Value;
                else
                    tempParam.Value = param.ParamValue;

                lsqlcmd_command.Parameters.Add(tempParam);
            }
        }

    }
}
