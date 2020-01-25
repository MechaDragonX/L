using System;
using System.Collections.Generic;
using System.Text;

namespace L
{
    public static class DateTimeParser
    {
        /// <summary>
        /// Parses a DateTime object from a month's name and the year
        /// </summary>
        /// <param name="year"></param>
        /// <param name="monthName">The name of the month</param>
        /// <returns></returns>
        public static DateTime Parse(int year, string monthName)
        {
            int month = 0;
            switch(monthName.ToLower())
            {
                case  "january":
                case "jan":
                {
                    month = 1;
                    break;
                }
                case "february":
                case "feb":
                {
                    month = 2;
                    break;
                }
                case "march":
                case "mar":
                {
                    month = 3;
                    break;
                }
                case "april":
                case "apr":
                {
                    month = 4;
                    break;
                }
                case "may":
                {
                    month = 5;
                    break;
                }
                case "june":
                case "jun":
                {
                    month = 6;
                    break;
                }
                case "july":
                case "jul":
                {
                    month = 7;
                    break;
                }
                case "august":
                case "aug":
                {
                    month = 8;
                    break;
                }
                case "september":
                case "sep":
                case "sept":
                {
                    month = 9;
                    break;
                }
                case "october":
                case "oct":
                {
                    month = 10;
                    break;
                }
                case "november":
                case "nov":
                {
                    month = 11;
                    break;
                }
                case "december":
                case "dec":
                {
                    month = 12;
                    break;
                }
                default:
                    month = -1;
                    break;
            }
            return new DateTime(year, month, 1);
        }
    }
}
