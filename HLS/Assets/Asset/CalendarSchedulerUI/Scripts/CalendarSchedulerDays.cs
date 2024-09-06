using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maything.UI.CalendarSchedulerUI
{

    public class CalendarSchedulerDays : MonoBehaviour
    {
        const int TOTALCELLS = 42;

        public CalendarSchedulerUI UI;
        public RectTransform borderTransform;
        public RectTransform contentTransform;

        RectTransform ownerTransform;

        CalendarSchedulerDay[,] cellDays = new CalendarSchedulerDay[6, 7];

        CalendarSchedulerDay selectedDay = null;

        List<CalendarSchedulerItem> items = new List<CalendarSchedulerItem>();
        CalendarSchedulerItem selectedItem = null;

        // Start is called before the first frame update
        void Start()
        {
            Initialization();
        }

        public void Initialization()
        {
            ownerTransform = GetComponent<RectTransform>();
            UpdateTheme();
            UpdateBorders();
            AddCells();
            UpdateDays(UI.year, UI.month, UI.day, UI.sundayIsFirst);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateTheme()
        {
            RectTransform UITransform = UI.gameObject.GetComponent<RectTransform>();
            ownerTransform.anchorMin = new Vector2(0, 1);
            ownerTransform.anchorMax = new Vector2(1, 1);

            ownerTransform.sizeDelta = new Vector2(0, UITransform.rect.height - UI.theme.weekHeight - UI.theme.headerHeight);
            ownerTransform.localPosition = new Vector3(UITransform.rect.width / -2f, UITransform.rect.height / 2f - UI.theme.headerHeight - UI.theme.weekHeight, 0);
        }

        public void UpdateBorders()
        {
            if (UI.theme.isDayBorder == false) return;

            float singleWidth = ownerTransform.rect.width / 7f;
            float singleHeight = ownerTransform.rect.height / 6f;

            for (int i = 1; i < 7; i++)
            {
                GameObject go = new GameObject("Column" + i.ToString());
                go.transform.parent = borderTransform;
                RectTransform goRect = go.AddComponent<RectTransform>();
                goRect.anchorMin = new Vector2(0, 1);
                goRect.anchorMax = new Vector2(0, 1);
                goRect.pivot = new Vector2(0, 1);

                goRect.sizeDelta = new Vector2(1, ownerTransform.rect.height);
                goRect.localPosition = new Vector3(i * singleWidth - ownerTransform.rect.width / 2f, ownerTransform.rect.height / 2f, 0);

                Image img = go.AddComponent<Image>();
                img.color = UI.theme.dayBorderColor;
            }

            for (int i = 1; i < 7; i++)
            {
                GameObject go = new GameObject("Row" + i.ToString());
                go.transform.parent = borderTransform;
                RectTransform goRect = go.AddComponent<RectTransform>();
                goRect.anchorMin = new Vector2(0, 1);
                goRect.anchorMax = new Vector2(0, 1);
                goRect.pivot = new Vector2(0, 1);

                goRect.sizeDelta = new Vector2(ownerTransform.rect.width, 1);
                goRect.localPosition = new Vector3(ownerTransform.rect.width / -2f, i * singleHeight - ownerTransform.rect.height / 2f, 0);

                Image img = go.AddComponent<Image>();
                img.color = UI.theme.dayBorderColor;

            }

        }
        public void AddCells()
        {
            float singleWidth = ownerTransform.rect.width / 7f;
            float singleHeight = ownerTransform.rect.height / 6f;
            int idx = 0;


            for (int y = 6; y > 0; y--)
            {
                for (int x = 0; x < 7; x++)
                {
                    idx += 1;

                    GameObject go = Instantiate(UI.dayCellTemplate, contentTransform);
                    go.name = "Cell" + idx.ToString();
                    RectTransform goRect = go.GetComponent<RectTransform>();
                    goRect.anchorMin = new Vector2(0, 1);
                    goRect.anchorMax = new Vector2(0, 1);
                    goRect.pivot = new Vector2(0, 1);
                    goRect.sizeDelta = new Vector2(singleWidth, singleHeight);
                    goRect.localPosition = new Vector3(singleWidth * x - ownerTransform.rect.width / 2f, singleHeight * y - ownerTransform.rect.height / 2f, 0);


                    //GameObject textGo = Instantiate(UI.dayCellTemplate, goRect);
                    //textGo.name = "Day" + idx.ToString();
                    //RectTransform textRect = textGo.GetComponent<RectTransform>();
                    //textRect.anchorMin = Vector2.zero;
                    //textRect.anchorMax = Vector2.one;
                    //textRect.sizeDelta = new Vector2(UI.theme.dayTextSpace * -2f, UI.theme.dayTextSpace * -2f);
                    //textRect.localPosition = new Vector3(goRect.rect.width / 2f, goRect.rect.height / -2f);

                    CalendarSchedulerDay day = go.GetComponent<CalendarSchedulerDay>();
                    if (day != null)
                    {
                        day.cellBackground.color = UI.theme.dayNormalBackground;
                        day.cellText.text = idx.ToString();
                        day.cellText.color = UI.theme.dayNormalTextColor;
                        day.cellText.alignment = UI.theme.dayTextAnchor;
                        day.days = this;
                    }
                    cellDays[6 - y, x] = day;
                }
            }


        }

        public void UpdateDays(int year, int month, int day, bool sundayIsFirst)
        {
            ClearItems();
            UI.data.Sort();

            int days = DateTime.DaysInMonth(year, month);
            DateTime firstDay = new DateTime(year, month, 1);
            int fstCol = (int)firstDay.DayOfWeek;

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    cellDays[y, x].isEnabled = false;
                    cellDays[y, x].cellText.text = "";
                }
            }

            for (int d = 1; d <= days; d++)
            {
                DateTime date = new DateTime(year, month, d);
                int column, row;
                int offset = -1;
                if (sundayIsFirst == false) offset = -2;

                //row = (d + fstCol - 1) / 7;
                //column = (d + fstCol - 1) % 7;
                row = (d + fstCol + offset) / 7;
                column = (d + fstCol + offset) % 7;
                cellDays[row, column].cellText.text = d.ToString();

                if (day == d)
                {
                    cellDays[row, column].cellText.color = UI.theme.daySelectedTextColor;
                    cellDays[row, column].cellBackground.color = UI.theme.daySelectedBackground;
                    selectedDay = cellDays[row, column];
                }
                else
                {
                    if (date.Year == DateTime.Now.Year && date.Month == DateTime.Now.Month && date.Day == DateTime.Now.Day)
                    {
                        cellDays[row, column].cellText.color = UI.theme.dayToDayTextColor;
                        cellDays[row, column].cellBackground.color = UI.theme.dayToDayBackground;
                    }
                    else
                    {
                        cellDays[row, column].cellText.color = UI.theme.dayNormalTextColor;
                        cellDays[row, column].cellBackground.color = UI.theme.dayNormalBackground;
                    }
                }
                AddItems(cellDays[row, column], date);
                cellDays[row, column].cellDateTime = date;
                cellDays[row, column].isEnabled = true;

                if (cellDays[row, column].itemCount == 1)
                {
                    cellDays[row, column].cellText.text += " ( 1 " + UI.language.singleEvent + ")";
                }
                else if (cellDays[row, column].itemCount > 1)
                {
                    cellDays[row, column].cellText.text += " ( " + cellDays[row, column].itemCount.ToString() + " " + UI.language.multiEvent + ")";
                }
            }

            if (UI.isIncludeAfterAndBefore)
                ListDaysOfAllMonths(year, month, sundayIsFirst);
        }

        void ListDaysOfAllMonths(int year, int month, bool sundayIsFirst)
        {
            DateTime firstDay = new DateTime(year, month, 1);
            int offset = -1;
            if (sundayIsFirst == false) offset = -2;

            int fstCol = (int)firstDay.DayOfWeek;
            int newMonth = month;

            // adjust for year
            if (month == 1)
            {
                year--;
                newMonth = 12;
            }
            else
            {
                newMonth--;
            }
            int days = DateTime.DaysInMonth(year, newMonth);

            // previous days
            for (int d = fstCol + offset; d >= 0; d--)
            {
                DateTime date = new DateTime(year, newMonth, days);
                cellDays[0, d].cellText.text = days.ToString();
                cellDays[0, d].cellDateTime = date;
                cellDays[0, d].cellText.color = UI.theme.dayDisableTextColor;
                cellDays[0, d].cellBackground.color = UI.theme.dayDisableBackground;
                days--;
            }


            // future days
            newMonth = month;
            if (month == 12)
            {
                year++;
                newMonth = 1;
            }
            else
            {
                newMonth++;
            }

            days = DateTime.DaysInMonth(year, month);
            int day = 1;
            int endDay = 42;
            if (!sundayIsFirst) endDay = 43;

            for (int d = fstCol + days + 1; d <= endDay; d++)
            {
                int c = (d + offset) % 7;
                int r = (d + offset) / 7;
                DateTime date = new DateTime(year, newMonth, day);
                cellDays[r, c].cellText.text = day.ToString();
                cellDays[r, c].cellDateTime = date;
                cellDays[r, c].cellText.color = UI.theme.dayDisableTextColor;
                cellDays[r, c].cellBackground.color = UI.theme.dayDisableBackground;
                day++;
            }
        }

        public void ClearItems()
        {
            foreach (CalendarSchedulerItem item in items)
            {
                item.day.cellText.text = item.day.cellDateTime.Day.ToString();
                item.day.itemCount = 0;
                Destroy(item.gameObject);
            }
            items.Clear();
        }

        public void AddItems(CalendarSchedulerDay day, DateTime date)
        {
            foreach (CalendarSchedulerData d in UI.data)
            {
                if (d.startDateTime.year == date.Year && d.startDateTime.month == date.Month && d.startDateTime.day == date.Day)
                {
                    RectTransform rect = day.gameObject.GetComponent<RectTransform>();
                    //Ôö¼ÓItem£¡
                    GameObject go = Instantiate(UI.scheduleItemTemplate, rect);
                    RectTransform goRect = go.GetComponent<RectTransform>();
                    goRect.sizeDelta = new Vector2(rect.rect.width - 10, 25f);
                    goRect.localPosition = new Vector3(5, -25f * day.itemCount - UI.theme.itemTopOffset);

                    CalendarSchedulerItem item = go.GetComponent<CalendarSchedulerItem>();
                    item.days = this;
                    item.day = day;
                    item.SetData(d);

                    items.Add(item);

                    day.itemCount++;
                }
            }
        }

        public void ChangeDay(CalendarSchedulerDay day)
        {
            if (selectedDay != null)
            {
                if (selectedDay.cellDateTime.Year == DateTime.Now.Year && selectedDay.cellDateTime.Month == DateTime.Now.Month && selectedDay.cellDateTime.Day == DateTime.Now.Day)
                {
                    selectedDay.cellText.color = UI.theme.dayToDayTextColor;
                    selectedDay.cellBackground.color = UI.theme.dayToDayBackground;
                }
                else
                {
                    selectedDay.cellText.color = UI.theme.dayNormalTextColor;
                    selectedDay.cellBackground.color = UI.theme.dayNormalBackground;
                }
            }

            selectedDay = day;
            selectedDay.cellText.color = UI.theme.daySelectedTextColor;
            selectedDay.cellBackground.color = UI.theme.daySelectedBackground;

            UI.currentDateTime = day.cellDateTime;
            UI.onDateTimeChanged.Invoke(day.cellDateTime);
        }

        public void ChangeItem(CalendarSchedulerItem item)
        {
            if (selectedItem != null)
            {
                selectedItem.background.color = selectedItem.normalColor;
                selectedItem.isSelected = false;
            }

            selectedItem = item;
            selectedItem.isSelected = true;
            selectedItem.background.color = UI.theme.itemSelectedBackground;

            UI.onScheduleChanged.Invoke(item);
        }
    }
}