using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TrackIT.Security
{
    public class RoleBO : TrackITAbstractDAL
    {
        public Guid? RoleID { get; set; }
        public Guid? BUID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string BUName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsPredefined { get; set; }
        public string RoleType { get; set; }
        public string RoleTypeDesc { get; set; }

        public DataSet dsRole { get; set; }

        public Guid? RoleAccessID { get; set; }
        public int ModuleID { get; set; }
        public int ScreenID { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int View { get; set; }
        public int Delete { get; set; }
        public string XMLData { get; set; }

        public RoleBO(string sConn)
        {
            ConnectionString = sConn;
        }
    }
}
