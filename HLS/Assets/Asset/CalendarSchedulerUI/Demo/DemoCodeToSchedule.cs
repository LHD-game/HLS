using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Maything.UI.CalendarSchedulerUI
{
    public class DemoCodeToSchedule : MonoBehaviour
    {
        public CalendarSchedulerUI calendarSchedulerUI;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        // 캘린더에 점수 표시 예시
        public void addTestScore()
        {
            List<CalendarSchedulerData> dataList = new List<CalendarSchedulerData>();
            for (int i = 1; i < 31; i++)
            {
                CalendarSchedulerData d = new CalendarSchedulerData(
                    Random.Range(11, 144).ToString(),
                    Color.white,
                    2024,
                    9,
                    i,
                    true, 0, 0, "");
                dataList.Add(d);

            }
            calendarSchedulerUI.AddItems(dataList);
        }
        
        // 스케쥴 내용 디자인
        public void addSingleTestScore(DateTime date, int score)
        {
            CalendarSchedulerData d = new CalendarSchedulerData(
                "<color=#32438B><size=12.5%>"+score+"</size></color>",
                Color.white,
                date.Year,
                date.Month,
                date.Day,
                true, 0, 0, "");
            calendarSchedulerUI.AddItem(d);
        }
        
        public void addTestScore(List<Dictionary<string, object>> SD_)
        {
            List<CalendarSchedulerData> dataList = new List<CalendarSchedulerData>();
            for (int i = 1; i < SD_.Count+1; i++)
            {
                DateTime Date = Convert.ToDateTime(SD_[SD_.Count - i]["date"]);
                int TotalData = Convert.ToInt32(SD_[SD_.Count - i]["total"]);
                CalendarSchedulerData d = new CalendarSchedulerData(
                    Random.Range(11, 144).ToString(),
                    Color.white,
                    2024,
                    9,
                    i,
                    true, 0, 0, "");
                dataList.Add(d);
            }
            calendarSchedulerUI.AddItems(dataList);
        }

        public void AddSingleSchedule()
        {
            CalendarSchedulerData d = new CalendarSchedulerData("Book Club", Color.red, DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, false, 10, 15, "");
            calendarSchedulerUI.AddItem(d);
        }

        public void RandomSchedules()
        {
            List<CalendarSchedulerData> dataList = new List<CalendarSchedulerData>();
            string[] testName = new string[] { "Dance Club", "HSH Arena", "Weekly Summit", "Galaxy Match", "GER Arena", "Park", "Library" };
            Color[] testColors = new Color[] { Color.red, Color.blue, Color.black, Color.yellow, Color.cyan, Color.grey };

            for (int i = 1; i < 31; i++)
            {
                for (int j = 0; j < Random.Range(1, 5); j++)
                {
                    CalendarSchedulerData d = new CalendarSchedulerData(
                               testName[Random.Range(0, testName.Length)],
                               testColors[Random.Range(0, testColors.Length)],
                               2024,
                               9,
                               i,
                               false, Random.Range(9, 23), Random.Range(1, 59), "");
                    dataList.Add(d);
                }

            }

            calendarSchedulerUI.AddItems(dataList);
        }

        public void ClearSchedules()
        {
            calendarSchedulerUI.ClearItems();
        }
    }
}