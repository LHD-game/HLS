using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maything.UI.CalendarSchedulerUI
{

    public class CalendarSchedulerDay : MonoBehaviour, IPointerClickHandler
    {
        public Image cellBackground;
        public Text cellText;
        public DateTime cellDateTime;
        public CalendarSchedulerDays days;
        public bool isEnabled = false;

        [HideInInspector]
        public int itemCount = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (!isEnabled) return;

            days.ChangeDay(this);
        }
    }
}