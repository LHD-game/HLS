using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void AddSingleSchedule()
        {
            CalendarSchedulerData d = new CalendarSchedulerData("Book Club", Color.red, 2023, 7, 2, false, 10, 15, "");
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
                               2023,
                               7,
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