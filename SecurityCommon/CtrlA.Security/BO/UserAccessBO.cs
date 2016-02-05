using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TrackIT.Security
{
    public class UserAccessBO : TrackITAbstractDAL
    {
        public Guid? UsersAccessID { get; set; }
        public Guid? UsersID { get; set; }
        public Guid? RoleID { get; set; }
        public int ModuleID { get; set; }
        public int ScreenID { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int View { get; set; }
        public int Delete { get; set; }
        public DataSet dsUserAccess { get; set; }
        public string XMLData { get; set; }
        public string FilePath { get; set; }
        public string ChildScreenUrl{ get; set; }

        public UserAccessBO(string sConn)
        {
            ConnectionString = sConn;
        }
    }
}
