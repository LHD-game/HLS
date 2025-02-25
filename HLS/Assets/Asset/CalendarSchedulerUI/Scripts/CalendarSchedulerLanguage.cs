using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maything.UI.CalendarSchedulerUI
{

    [CreateAssetMenu(menuName = "Calendar Scheduler UI/Create New Language")]
    public class CalendarSchedulerLanguage : ScriptableObject
    {
        [Header("Year")]
        public string YearName = "";
        public bool isYearBeforeMonth = false;

        [Header("Month")]
        public string January = "January";
        public string February = "February";
        public string March = "March";
        public string April = "April";
        public string May = "May";
        public string June = "June";
        public string July = "July";
        public string August = "August";
        public string September = "September";
        public string October = "October";
        public string November = "November";
        public string December = "December";

        [Header("Week")]
        public string Monday = "Mon";
        public string Tuesday = "Tue";
        public string Wednesday = "Wed";
        public string Thursday = "Thu";
        public string Friday = "Fri";
        public string Saturday = "Sat";
        public string Sunday = "Sun";

        [Header("Event")]
        public string singleEvent = "event";
        public string multiEvent = "events";

    }
}