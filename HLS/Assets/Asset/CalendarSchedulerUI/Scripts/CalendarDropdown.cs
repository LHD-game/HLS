using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maything.UI.CalendarSchedulerUI
{

    public class CalendarDropdown : MonoBehaviour, IPointerClickHandler
    {
        public int year;
        public int month;
        public int day;

        public CalendarSchedulerTheme theme;
        public CalendarSchedulerLanguage language;

        public Text textBox;
        public GameObject calendarUI;
        public RectTransform content;

        bool isCalenderDisplay = false;

        CalendarSchedulerUI calendarSchedulerUI;
        // Start is called before the first frame update
        void Start()
        {
            RectTransform ownerTransform = GetComponent<RectTransform>();

            GameObject go = Instantiate(calendarUI, content);
            RectTransform rect = go.GetComponent<RectTransform>();

            //rect.pivot = new Vector2(0, 1);
            //rect.position = gameObject.transform.position;
            rect.localPosition = new Vector3(rect.rect.width / 2f - ownerTransform.rect.width / 2f, rect.rect.height / -2f);
            //rect.localPosition = Vector3.zero;

            calendarSchedulerUI = go.GetComponent<CalendarSchedulerUI>();
            if (theme != null)
                calendarSchedulerUI.theme = theme;
            if (language != null)
                calendarSchedulerUI.language = language;

            calendarSchedulerUI.Initialization(true);
            calendarSchedulerUI.onDateTimeChanged.AddListener(calendarOnChange);

            if (year == 0 && month == 0 && day == 0)
            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
                day = DateTime.Now.Day;
            }
            textBox.text = year.ToString() + "-" + month.ToString() + "-" + day.ToString();
            //calendarSchedulerUI.SetDay(year,month, day);
            //calendarSchedulerUI.onDateTimeChanged

            go.SetActive(false);
        }

        void calendarOnChange(DateTime dateTime)
        {
            isCalenderDisplay = false;
            calendarSchedulerUI.gameObject.SetActive(isCalenderDisplay);

            year = dateTime.Year;
            month = dateTime.Month;
            day = dateTime.Day;

            textBox.text = year.ToString() + "-" + month.ToString() + "-" + day.ToString();

        }

        // Update is called once per frame
        void Update()
        {

        }

        void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            isCalenderDisplay = !isCalenderDisplay;

            if (calendarSchedulerUI == null) return;

            if (isCalenderDisplay)
                calendarSchedulerUI.SetDay(year, month, day);

            calendarSchedulerUI.gameObject.SetActive(isCalenderDisplay);
        }
    }
}