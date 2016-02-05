using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections;
using System.Data;

namespace TrackIT.Common
{
    public static class DateFunctions
    {     
        #region Public Methods

        public static DateTime ToDate(string dateString, string dateFormat)
        {
            DateTimeFormatInfo format = new DateTimeFormatInfo();
            format.ShortDatePattern = dateFormat;
            try
            {
                return DateTime.Parse(dateString, format);
            }
            catch (Exception ex)
            {
                throw new FormatException("Invalid date or format string.", ex);
            }

        }
        
        public static DateTime ParseDateTime(string text)
        {

            DateTime dateTime = DateTime.MinValue;
            IFormatProvider CultureInfo = new System.Globalization.CultureInfo("en-US", true);

            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    string st = DateTime.MinValue.ToString("MM/dd/yyyy");
                    dateTime = Convert.ToDateTime(st);
                }
                else
                {
                    dateTime = DateTime.ParseExact(text.Trim(), "MM/dd/yyyy hh:mm:ss tt", CultureInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dateTime;
        }

        public static DateTime ParseDateExact(string sDate)
        {
            //string lang = "en-US";
            //System.Globalization.CultureInfo cInfo = new System.Globalization.CultureInfo(lang);

            DateTime retDatetime = DateTime.MinValue;
            string[] expectedFormats = { "dd/MM/yyyy", "MM/dd/yyyy" };

            try
            {
                retDatetime = DateTime.ParseExact(sDate, expectedFormats, null, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDatetime;
        }

        public static DateTime ParseDate(string text)
        {
            DateTime dateTime = DateTime.MinValue;
            IFormatProvider CultureInfo = new System.Globalization.CultureInfo("en-US", true);
            try
            {
                if (string.IsNullOrEmpty(text))
                {
                    string st = DateTime.MinValue.ToString("MM/dd/yyyy");
                    dateTime = Convert.ToDateTime(st);
                }
                else
                {
                    dateTime = DateTime.ParseExact((text.Trim()), "MM/dd/yyyy", CultureInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dateTime;
        }
        
        public static bool CheckWithCurrentDate(string sDate)
        {
            bool boolFlag = false;

            try
            {
                if (!string.IsNullOrEmpty(sDate))
                {
                    string[] sCheckDate = Convert.ToDateTime(sDate).ToString("MM/yyyy").Split('/');
                    string[] sCurrentDate = DateTime.Now.ToString("MM/yyyy").Split('/');

                    if (Convert.ToInt32(sCheckDate[1]) > Convert.ToInt32(sCurrentDate[1]))
                        boolFlag = false;
                    else if (Convert.ToInt32(sCheckDate[1]) == Convert.ToInt32(sCurrentDate[1]))
                    {
                        if (Convert.ToInt32(sCheckDate[0]) > Convert.ToInt32(sCurrentDate[0]))
                            boolFlag = false;
                        else
                            boolFlag = true;
                    }
                    else
                        boolFlag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return boolFlag;
        }

        public static string ParseDate(string sCurrentDate, string sDateFormat)
        {
            string sDate = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(sCurrentDate))
                {
                    DateTime dt = Convert.ToDateTime(sCurrentDate);
                    sDate = dt.ToString(sDateFormat);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sDate;
        }

        public static string FormatDate(DateTime dt)
        {
            string sOutput = string.Empty;
            try
            {
                DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day);
                string strMonth = dt.ToString("MMMM");
                sOutput = dt1.DayOfWeek.ToString() + ", " + strMonth + " " + dt1.Day.ToString() + ", " + dt1.Year.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sOutput;
        }

        public static string GetStartandEndDateLabels(DateTime dt)
        {
            string sOutput = string.Empty;
            try
            {
                DayOfWeek day = dt.DayOfWeek;
                int days = day - DayOfWeek.Sunday;
                DateTime start = dt.AddDays(-days);
                DateTime dtMonday = start.AddDays(1);
                DateTime dtTuesday = start.AddDays(2);
                DateTime dtWednesday = start.AddDays(3);
                DateTime dtThursday = start.AddDays(4);
                DateTime dtFriday = start.AddDays(5);
                DateTime end = start.AddDays(6);
                sOutput = start.Month + "/" + start.Day + "|" + dtMonday.Month + "/" + dtMonday.Day + "|" + dtTuesday.Month + "/" + dtTuesday.Day + "|" + dtWednesday.Month + "/" + dtWednesday.Day + "|" + dtThursday.Month + "/" + dtThursday.Day + "|" + dtFriday.Month + "/" + dtFriday.Day + "|" + end.Month + "/" + end.Day;                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sOutput;
        }

        public static string GetSundayforDate(DateTime dt)
        {
            string sOutput = string.Empty;
            try
            {
                DayOfWeek day = dt.DayOfWeek;
                int days = day - DayOfWeek.Sunday;
                DateTime start = dt.AddDays(-days);
                sOutput = start.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sOutput;
        }

        public static string GetDay(DateTime dt)
        {
            string strDay = string.Empty;
            try
            {
                strDay = dt.DayOfWeek.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strDay;
        }

        public static string getDate(DateTime dt)
        {
            string retDate = string.Empty;
            try
            {
                retDate = dt.Month + "/" + dt.Day + "/" + dt.Year;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retDate;
        }

        public static string GetDateFormatByMonth(string sDate)
        {
            string sOutput = string.Empty;
            try
            {
                DateTime dt = Convert.ToDateTime(sDate);
                sOutput = dt.ToString("MMMM");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sOutput;
        }

        public static string CalculateYearMonthDays(DateTime startDate, DateTime endDate)
        {
            int years = endDate.Year - startDate.Year;
            int months = 0;
            int days = 0;

            try
            {

                // Check if the last year, was a full year.
                if (endDate < startDate.AddYears(years) && years != 0)
                {
                    years--;
                }

                // Calculate the number of months.
                startDate = startDate.AddYears(years);

                if (startDate.Year == endDate.Year)
                {
                    months = endDate.Month - startDate.Month;
                }
                else
                {
                    months = (12 - startDate.Month) + endDate.Month;
                }

                // Check if last month was a complete month.
                if (endDate < startDate.AddMonths(months) && months != 0)
                {
                    months--;
                }

                // Calculate the number of days.
                startDate = startDate.AddMonths(months);

                //days = (endDate - startDate).Days;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (years + " Years " + months + " Months " + days + " days");
        }

        public static string CalculateYearMonth(DateTime startDate, DateTime endDate)
        {
            int years = endDate.Year - startDate.Year;
            int months = 0;
            //int days = 0;

            try
            {
                // Check if the last year, was a full year.
                if (endDate < startDate.AddYears(years) && years != 0)
                {
                    years--;
                }

                // Calculate the number of months.
                startDate = startDate.AddYears(years);

                if (startDate.Year == endDate.Year)
                {
                    months = endDate.Month - startDate.Month;
                }
                else
                {
                    months = (12 - startDate.Month) + endDate.Month;
                }

                // Check if last month was a complete month.
                if (endDate < startDate.AddMonths(months) && months != 0)
                {
                    months--;
                }

                // Calculate the number of days.
                startDate = startDate.AddMonths(months);

                //days = (endDate - startDate).Days;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return years + " Years " + months + " Months"; //, days);
        }

        public static string GetDateFormatByYearMonthDayForReports(string sDate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(sDate);
                return dt.Year + "," + dt.Month + "," + dt.Day;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetDateFormatByMonthDayYear(string sDate)
        {
            try
            {
                string[] date = null;

                if (!string.IsNullOrEmpty(sDate))
                {
                    date = sDate.Split('/');
                    sDate = date[1] + "/" + date[0] + "/" + date[2];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sDate;
        }

        public static string GetDateFormatByMonthYearForReports(string sDate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(sDate);
                return dt.ToString("MMMM-yyyy"); //dt.Month.ToString("MMMM") + "-" + dt.Year;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetDateFormatByMonthYearForIntake(string sDate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(sDate);
                return dt.ToString("MMM - yyyy").ToUpper();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetMonthByDate(string sDate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(sDate);
                return dt.Month.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetYearByDate(string sDate)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(sDate);
                return dt.Year.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Formatting date
        public static string getFormattedDate(string strDate)
        {
            string strDateReturn = string.Empty;
            try
            {
                string strDateCulture = CultureInfo.CurrentCulture.Name;
                switch (strDateCulture)
                {
                    case "en-GB":
                        strDateReturn = strDate;
                        break;
                    case "en-US":
                        strDateReturn = setDate(strDate);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strDateReturn;
        }

        public static string setDate(string dtDate)
        {
            string actualDate = string.Empty;
            try
            {
                string[] strArray = new string[3];
                strArray = dtDate.Split('/');

                if (strArray[1].Length == 1)
                {
                    strArray[1] = "0" + strArray[1];
                }
                if (strArray[0].Length == 1)
                {
                    strArray[0] = "0" + strArray[0];
                }
                actualDate = strArray[1] + "/" + strArray[0] + "/" + strArray[2];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return actualDate;

        }
        #endregion

        #endregion

    }
}
