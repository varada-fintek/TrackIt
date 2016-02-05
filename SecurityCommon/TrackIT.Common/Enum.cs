using System;

namespace TrackIT.Common
{

    /// <summary>
    /// Constants used throughout the application
    /// </summary>

    public enum Constants
    {
        ErrorValue = -999,
        DummyID = 0,
    }

    /// <summary>
    /// Status used in different SQL or other complex logic operations
    /// </summary>

    public enum Status
    {
        Failed = 0,
        Succeeded,
        Warning,
        Exists,
    }

    public enum RecordState
    {
        All = 0,
        Active = 1,
        Deleted = 2,
    }

    public enum Month
    {
        None = 0,
        January = 01,
        February = 02,
        March = 03,
        April = 04,
        May = 05,
        June = 06,
        July = 07,
        August = 08,
        September = 09,
        October = 10,
        November = 11,
        December = 12
    }

}