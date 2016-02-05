using System;
using System.ComponentModel;

using TrackIT.Common;

namespace TrackIT.WebApp.CtrlAEnum
{
    public enum Gender
    {
        [Description("Male")]
        Male,
        [Description("Female")]
        Female,
    }

    public enum WorkflowPeriod
    {
        [Description("DAYS")]
        Days,
        [Description("HOURS")]
        Hours,
    }

    public enum TaskStatus
    {
        [Description("COMPLETED")]
        Completed,
        [Description("PENDING")]
        Pending,
    }


    public enum MessageType
    {
        SMS = 0,
        EMAIL,
    }

    public enum YesNoType
    {
        No  = 0,
        Yes = 1,       
    }

    public enum ReportTemplate
    {
        [Description("Application Access Report")]
        ApplicationAccessReport,
        [Description("Application SLA Report")]
        ApplicationSLAReport
    }

    public enum RequestStatus
    {
        [Description("Requested")]
        Requested,
        [Description("Created")]
        Created,
        [Description("Approved")]
        Approved,
        [Description("Reviewed")]
        Reviewed,
        [Description("Completed")]
        Completed,
        [Description("Cancelled")]
        Cancelled
    }

    public enum ApplicationDefault
    {
        [Description("Application")]
        Application,
        [Description("Email")]
        Email,
        [Description("Windows ID")]
        WindowsID
    }

    public enum RequestAccessType
    {
        [Description("Add")]
        Add,
        [Description("Edit")]
        Edit,
        [Description("Delete")]
        Delete,
    }

    public enum SLAType
    {
        [Description("Within SLA Date")]
        Within_SLA_Date,
        [Description("Exceeded SLA Date")]
        Exceeded_SLA_Date,
    }

}
