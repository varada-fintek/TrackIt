using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TrackIT.Security
{
    public class UserBO : TrackITAbstractDAL
    {

        public Guid? UsersID { get; set; }
        public string UserName { get; set; }
        public string LoginUserName { get; set; }
        public byte[] Password { get; set; }        
        public Guid? RoleID { get; set; }
        public DataSet dsUser { get; set; }
        public Boolean IsSuperUser { get; set; }
        public Boolean IsAdminRole { get; set; }
        public string ChildScreenURL { get; set; }
        public string UserType { get; set; }
        public Guid? FirmID { get; set; }
        public string FirmCode { get; set; }

        public int IsFirstLogin { get; set; }

        public Guid? UsersAccessID { get; set; }
        public int ModuleID { get; set; }
        public int ScreenID { get; set; }
        //public string Image_Path { get; set; }
        public int Add { get; set; }
        public int Edit { get; set; }
        public int View { get; set; }
        public int Delete { get; set; }
        public string XMLData { get; set; }

        public string LoginSessionID { get; set; }
        public string LoginType { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogOffTime { get; set; }
        
        public int LoginID { get; set; }

        public Guid? StaffID { get; set; }
        public string StaffName { get; set; }

        public string DisplayName { get; set; }
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string User_Photo_Path { get; set; }

        public string DepartmentCode { get; set; }

        public bool LoginAccess { get; set; }

        public UserBO(string sConn)
        {
            ConnectionString = sConn;
        }
    }
}
