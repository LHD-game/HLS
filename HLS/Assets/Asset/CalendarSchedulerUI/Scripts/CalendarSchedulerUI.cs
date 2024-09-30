using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Maything.UI.CalendarSchedulerUI
{

    public class CalendarSchedulerUI : MonoBehaviour
    {
        public CalendarSchedulerTheme theme;
        public CalendarSchedulerLanguage language;

        public int year = 2023;
        public int month = 10;
        public int day = 1;
        public bool sundayIsFirst = true;
        public bool isIncludeAfterAndBefore = true;

        [Header("Control")]
        public RectTransform border;
        public CalendarSchedulerHeaderMonth monthControl;
        public CalendarSchedulerDays daysControl;
        public GameObject dayCellTemplate;
        public GameObject scheduleItemTemplate;

        [HideInInspector]
        public DateTime currentDateTime;


        public List<CalendarSchedulerData> data;


        [Serializable]
        public class CalendarSchedulerChangeEvent : UnityEvent<DateTime>
        {

        }

        [Serializable]
        public class CalendarSchedulerItemEvent : UnityEvent<CalendarSchedulerItem>
        {

        }



        [Space]
        [SerializeField]
        private CalendarSchedulerChangeEvent m_OnDateTimeChanged = new CalendarSchedulerChangeEvent();

        public CalendarSchedulerChangeEvent onDateTimeChanged
        {
            get { return m_OnDateTimeChanged; }
            set { m_OnDateTimeChanged = value; }
        }


        [Space]
        [SerializeField]
        private CalendarSchedulerItemEvent m_OnScheduleChanged = new CalendarSchedulerItemEvent();

        public CalendarSchedulerItemEvent onScheduleChanged
        {
            get { return m_OnScheduleChanged; }
            set { m_OnScheduleChanged = value; }
        }


        // Start is called before the first frame update
        void Start()
        {
            // 오늘 날짜를 얻기 위해 추가
            year = DateTime.Today.Year;
            month = DateTime.Today.Month;
            day = DateTime.Today.Day;
            Initialization(true);
            //Initialization(false);
        }

        public void Initialization(bool isFirst)
        {
            currentDateTime = new DateTime(year, month, day);
            UpdateTheme();

            if (isFirst)
            {
                // 월과 일을 맟구기 위한 작업
                // 추가
                monthControl.UpdateTheme();
                monthControl.UpdateMonthText();
                //기존
                daysControl.Initialization();
            }
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        void UpdateTheme()
        {
            if (theme.isBorder == false)
                border.gameObject.SetActive(false);
            else
            {
                for (int i = 0; i < border.childCount; i++)
                {
                    Image image = border.GetChild(i).GetComponent<Image>();
                    if (image != null)
                        image.color = theme.borderColor;
                }
            }

        }

        public void NextMonth()
        {
            month += 1;
            if (month > 12)
            {
                year += 1;
                month = 1;

            }

            day = -1;
            if (currentDateTime != null && currentDateTime.Year == year && currentDateTime.Month == month)
                day = currentDateTime.Day;

            monthControl.UpdateMonthText();
            daysControl.UpdateDays(year, month, day, sundayIsFirst);
        }

        public void PrevMonth()
        {
            month -= 1;
            if (month < 1)
            {
                year -= 1;
                month = 12;
            }

            day = -1;
            if (currentDateTime != null && currentDateTime.Year == year && currentDateTime.Month == month)
                day = currentDateTime.Day;

            monthControl.UpdateMonthText();
            daysControl.UpdateDays(year, month, day, sundayIsFirst);
        }

        public void NextYear()
        {
            year++;
            if (currentDateTime != null && currentDateTime.Year == year && currentDateTime.Month == month)
                day = currentDateTime.Day;

            monthControl.UpdateMonthText();
            daysControl.UpdateDays(year, month, day, sundayIsFirst);

        }

        public void PrevYear()
        {
            year--;
            if (currentDateTime != null && currentDateTime.Year == year && currentDateTime.Month == month)
                day = currentDateTime.Day;

            monthControl.UpdateMonthText();
            daysControl.UpdateDays(year, month, day, sundayIsFirst);

        }

        public void SetDay(int year, int month, int day)
        {
            this.year = year;
            this.month = month;
            this.day = day;
            currentDateTime = new DateTime(year, month, day);

            monthControl.UpdateMonthText();
            daysControl.UpdateDays(year, month, day, sundayIsFirst);
        }

        public void AddItem(CalendarSchedulerData newData)
        {
            data.Add(newData);
            daysControl.UpdateDays(year, month, day, sundayIsFirst);
        }

        public void AddItems(List<CalendarSchedulerData> newDatas)
        {
            data.AddRange(newDatas);

            daysControl.UpdateDays(year, month, day, sundayIsFirst);
        }

        public void ClearItems()
        {
            daysControl.ClearItems();
            data.Clear();
        }



    }
}