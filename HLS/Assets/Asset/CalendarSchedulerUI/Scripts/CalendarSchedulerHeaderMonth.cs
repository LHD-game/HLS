using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maything.UI.CalendarSchedulerUI
{

    public class CalendarSchedulerHeaderMonth : MonoBehaviour
    {
        public CalendarSchedulerUI UI;
        public Image prevMonthImage;
        public Image nextMonthImage;

        public Image prevYearImage;
        public Image nextYearImage;

        public Text monthText;

        RectTransform ownerTransform;

        // Start is called before the first frame update
        void Start()
        {
            ownerTransform = GetComponent<RectTransform>();

            UpdateTheme();
            UpdateMonthText();
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void UpdateTheme()
        {
            if (UI == null) return;
            if (UI.theme == null) return;
            if (UI.language == null) return;

            GetComponent<Image>().color = UI.theme.headerBackground;

            ownerTransform.sizeDelta = new Vector2(0, UI.theme.headerHeight);

            monthText.rectTransform.sizeDelta = new Vector2(UI.theme.headerTextWidth, 0);
            monthText.fontSize = UI.theme.headerTextSize;

            prevMonthImage.sprite = UI.theme.headerPrevMonthIcon;
            nextMonthImage.sprite = UI.theme.headerNextMonthIcon;

            prevYearImage.sprite = UI.theme.headerPrevYearIcon;
            nextYearImage.sprite = UI.theme.headerNextYearIcon;

            prevMonthImage.color = UI.theme.headerTextColor;
            nextMonthImage.color = UI.theme.headerTextColor;

            prevYearImage.color = UI.theme.headerTextColor;
            nextYearImage.color = UI.theme.headerTextColor;



            float arrowY = ownerTransform.rect.height / -2f + prevMonthImage.rectTransform.rect.height / 2f;
            switch (UI.theme.headerArrowAlign)
            {
                case CalendarSchedulerTheme.enumHeaderArrowAlign.AlignBothSides:
                    prevYearImage.rectTransform.localPosition = new Vector3(10, arrowY);
                    nextYearImage.rectTransform.localPosition = new Vector3(ownerTransform.rect.width - nextMonthImage.rectTransform.rect.width - 10, arrowY);

                    prevMonthImage.rectTransform.localPosition = new Vector3(20 + prevYearImage.rectTransform.rect.width, arrowY);
                    nextMonthImage.rectTransform.localPosition = new Vector3(ownerTransform.rect.width - nextMonthImage.rectTransform.rect.width * 2 - 20, arrowY);


                    break;
                case CalendarSchedulerTheme.enumHeaderArrowAlign.AlignMiddle:
                    prevMonthImage.rectTransform.localPosition = new Vector3(ownerTransform.rect.width / 2f - UI.theme.headerTextWidth / 2f - prevMonthImage.rectTransform.rect.width, arrowY);
                    nextMonthImage.rectTransform.localPosition = new Vector3(ownerTransform.rect.width / 2f + UI.theme.headerTextWidth / 2f, arrowY);

                    prevYearImage.rectTransform.localPosition = new Vector3(ownerTransform.rect.width / 2f - UI.theme.headerTextWidth / 2f - prevMonthImage.rectTransform.rect.width * 2 - 10, arrowY);
                    nextYearImage.rectTransform.localPosition = new Vector3(ownerTransform.rect.width / 2f + UI.theme.headerTextWidth / 2f + nextYearImage.rectTransform.rect.width + 10, arrowY);

                    break;
            }

        }

        public void UpdateMonthText()
        {
            if (UI == null) return;
            switch (UI.month)
            {
                case 1:
                    monthText.text = UI.language.January;
                    break;
                case 2:
                    monthText.text = UI.language.February;
                    break;
                case 3:
                    monthText.text = UI.language.March;
                    break;
                case 4:
                    monthText.text = UI.language.April;
                    break;
                case 5:
                    monthText.text = UI.language.March;
                    break;
                case 6:
                    monthText.text = UI.language.June;
                    break;
                case 7:
                    monthText.text = UI.language.July;
                    break;
                case 8:
                    monthText.text = UI.language.August;
                    break;
                case 9:
                    monthText.text = UI.language.September;
                    break;
                case 10:
                    monthText.text = UI.language.October;
                    break;
                case 11:
                    monthText.text = UI.language.November;
                    break;
                case 12:
                    monthText.text = UI.language.December;
                    break;
            }

            monthText.color = UI.theme.headerTextColor;

            if (UI.language.isYearBeforeMonth)
                monthText.text = /*UI.year.ToString() + UI.language.YearName + " " +*/ monthText.text;
            else
                monthText.text = monthText.text; /*+ " " + UI.year.ToString() + UI.language.YearName*/;
        }
    }
}