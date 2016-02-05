using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace ProjMngTrack.WebApp
{
    #region Setup
    /// <summary>
    /// 
    /// </summary>
    public static class Predefined_FeesType
    {
        public const string COURSE_FEES = "Course Fees";
        public const string REGISTRATION_FEES = "Registration Fees";
        public const string LATE_CHARGE = "Late Charge";
    }
    #endregion

    #region Reports
    /// <summary>
    /// 
    /// </summary>
    public static class Predefined_Reports
    {
        public const string StaffListReportID = "738D1A17-B249-446E-A61E-AC27C9CA6375";
        public const string PositionReportID = "574FFFFA-2F7D-4B40-8F95-03A7CC315F45";
        public const string StatutoryReportID = "0662BF79-E9F7-4D92-804D-022A4C8F2EAE";
        public const string ConfirmedReportID = "C95C6F73-09CA-4C18-8699-05FE65BA6C81";
        public const string InsuranceReportID = "95E93D87-0344-4D16-A2E2-0D00512B78DB";
        public const string NewlyJoinedReportID = "85CA6574-AAA3-4F38-A9BE-0FF5DECBF7CB";
        public const string ResignedReportID = "5A517C45-AC0B-4A45-A513-19F9770C2F22";
        public const string RemunerationReportID = "4F04DC81-EEED-40AC-9F0E-1A6F05D2EF10";
        public const string StaffDetailsReportID = "7232495A-5EF1-4721-8B6C-1A75A3FEBF50";
        public const string SalaryReportID = "250ECAAD-EBA8-427C-8C88-1B0762CEAD7F";
        public const string LeaveBalanceReportID = "98A93145-CA6A-4065-B558-24314065E805";
        public const string RevisionReportID = "5B5CA779-33D3-495D-8642-2A830E0A485F";
        public const string AllowanceReportID = "EA801B9F-B7A9-4BB8-9BE0-A618F287AF1D";
    }
    #endregion



}