using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maything.UI.CalendarSchedulerUI
{

    [Serializable]
    public class CalendarSchedulerData : IComparable<CalendarSchedulerData>
    {
        public string title;
        public Color color = Color.red;
        public CalendarSchedulerDateTime startDateTime;
        public string notes;

        public CalendarSchedulerData()
        {

        }

        public CalendarSchedulerData(string title, Color color, CalendarSchedulerDateTime startDateTime, string notes)
        {
            this.title = title;
            this.color = color;
            this.startDateTime = startDateTime;
            this.notes = notes;
        }

        public CalendarSchedulerData(string title, Color color, int startYear, int startMonth, int startDay, bool isFullDay, int startHour, int startMinute, string notes)
        {
            this.title = title;
            this.color = color;
            CalendarSchedulerDateTime dt = new CalendarSchedulerDateTime();
            dt.year = startYear;
            dt.month = startMonth;
            dt.day = startDay;
            dt.hour = startHour;
            dt.minute = startMinute;
            dt.isFullDay = isFullDay;

            this.startDateTime = dt;
            this.notes = notes;
        }

        public int CompareTo(CalendarSchedulerData other)
        {
            DateTime d1 = startDateTime.ToDateTime();
            DateTime d2 = other.startDateTime.ToDateTime();

            return d1.CompareTo(d2);
        }

        public override string ToString()
        {
            if (title != "")
                return title;
            else
                return base.ToString();
        }
    }

    [Serializable]
    public class CalendarSchedulerDateTime
    {
        public int year;
        public int month;
        public int day;
        public bool isFullDay = true;
        public int hour;
        public int minute;

        public DateTime ToDateTime()
        {
            if (isFullDay)
                return new DateTime(year, month, day, 0, 0, 0);
            else
                return new DateTime(year, month, day, hour, minute, 0);
        }

        public override string ToString()
        {
            return year.ToString() + "-" + month.ToString() + "-" + day.ToString() + " " + hour.ToString() + ":" + minute.ToString();
        }
    }
}

