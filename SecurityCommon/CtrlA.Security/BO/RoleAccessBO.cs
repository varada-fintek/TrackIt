using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrackIT.Security
{
    public class RoleAccessBO : TrackITAbstractDAL
    {
        public Guid? RoleAccessID { get; set; }
        public Guid? RoleID { get; set; }
        public Guid? ParentRoleID { get; set; }

        public int ModuleID { get; set; }
        public int ScreenID { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int View { get; set; }
        public int Delete { get; set; }

        public RoleAccessBO(string sConn)
        {
            ConnectionString = sConn;
        }
    }
}
