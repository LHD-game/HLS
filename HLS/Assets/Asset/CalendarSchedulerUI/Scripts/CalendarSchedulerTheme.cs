using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maything.UI.CalendarSchedulerUI
{

    [CreateAssetMenu(menuName = "Calendar Scheduler UI/Create New Theme")]
    public class CalendarSchedulerTheme : ScriptableObject
    {
        public enum enumHeaderArrowAlign
        {
            AlignBothSides,
            AlignMiddle,
        }

        public bool isBorder = true;
        public Color borderColor = Color.grey;

        [Header("Header")]
        public float headerHeight = 55f;
        public int headerTextSize = 18;
        public float headerTextWidth = 200f;
        public Color headerBackground = Color.white;
        public Color headerTextColor = Color.black;
        public enumHeaderArrowAlign headerArrowAlign = enumHeaderArrowAlign.AlignMiddle;

        public Sprite headerPrevMonthIcon;
        public Sprite headerNextMonthIcon;

        public Sprite headerPrevYearIcon;
        public Sprite headerNextYearIcon;

        [Header("Week")]
        public float weekHeight = 35f;
        public int weekTextSize = 14;
        public Color weekBackground = Color.white;
        public Color weekTextColor = Color.black;

        [Header("Day")]
        public int dayTextSize = 14;
        public TextAnchor dayTextAnchor = TextAnchor.UpperLeft;
        public Color dayNormalTextColor = Color.black;
        public Color dayNormalBackground = Color.white;

        public Color dayDisableTextColor = Color.gray;
        public Color dayDisableBackground = Color.white;

        public Color daySelectedTextColor = Color.black;
        public Color daySelectedBackground = Color.blue;

        public Color dayToDayTextColor = Color.black;
        public Color dayToDayBackground = Color.white;

        public bool isDayBorder = true;
        public Color dayBorderColor = Color.gray;

        [Header("Item")]
        public float itemTopOffset = 25f;
        public Color itemNormalTextColor = Color.black;
        public Color itemSelectedBackground = Color.grey;
        public Color itemSelectedTextColor = Color.black;
    }
}

