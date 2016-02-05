using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DBHelper
{
    public class InParameters
    {
        private string _Svalue;
        private string _SParamName;
        private SqlDbType _SSqlDbType;

        public InParameters()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string ParamValue
        {
            get
            {
                return _Svalue;
            }
            set
            {
                _Svalue = value;
            }
        }
        public string ParamName
        {
            get
            {
                return _SParamName;
            }
            set
            {
                _SParamName = value;
            }
        }
        public SqlDbType SqlDataType
        {
            get
            {
                return _SSqlDbType;
            }
            set
            {
                _SSqlDbType = value;
            }
        }
    }
}
