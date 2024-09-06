using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maything.UI.CalendarSchedulerUI
{

    public class CalendarSchedulerItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        CalendarSchedulerData data;
        public CalendarSchedulerDays days;
        public CalendarSchedulerDay day;

        public Image background;
        public Image point;
        public Text textTime;
        public Text textTitle;

        [HideInInspector]
        public bool isSelected = false;

        [HideInInspector]
        public Color normalColor = new Color(0, 0, 0, 0);

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetData(CalendarSchedulerData data)
        {
            this.data = data;
            if (data.startDateTime.isFullDay)
                textTime.text = "All day";
            else
                textTime.text = data.startDateTime.hour.ToString("00") + ":" + data.startDateTime.minute.ToString("00");

            if (data.title == "")
                textTitle.text = "No title";
            else
                textTitle.text = data.title;

            point.color = data.color;
            textTitle.color = days.UI.theme.itemNormalTextColor;
            textTime.color = days.UI.theme.itemNormalTextColor;
        }

        void IPointerEnterHandler.OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
            background.color = days.UI.theme.itemSelectedBackground;
            textTitle.color = days.UI.theme.itemSelectedTextColor;
            textTime.color = days.UI.theme.itemSelectedTextColor;
        }

        void IPointerExitHandler.OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (isSelected == false)
            {
                background.color = normalColor;
                textTitle.color = days.UI.theme.itemNormalTextColor;
                textTime.color = days.UI.theme.itemNormalTextColor;
            }
        }

        void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            days.ChangeItem(this);
        }
    }
}