using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maything.UI.CalendarSchedulerUI
{
    public class CalendarSchedulerHeaderWeek : MonoBehaviour
    {
        // Start is called before the first frame update
        public CalendarSchedulerUI UI;
        public Text[] textWeeks;

        RectTransform ownerTransform;
        void Start()
        {
            ownerTransform = GetComponent<RectTransform>();
            UpdateTheme();
            UpdateText();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void UpdateTheme()
        {
            if (UI == null) return;
            if (UI.language == null) return;
            if (UI.theme == null) return;

            RectTransform UITransform = UI.gameObject.GetComponent<RectTransform>();

            ownerTransform.sizeDelta = new Vector2(0, UI.theme.weekHeight);
            ownerTransform.localPosition = new Vector3(UITransform.rect.width / -2f, UITransform.rect.height / 2f - UI.theme.headerHeight, 0);

            GetComponent<Image>().color = UI.theme.weekBackground;

            float singleSize = ownerTransform.rect.width / (float)textWeeks.Length;

            for (int i = 0; i < textWeeks.Length; i++)
            {
                textWeeks[i].rectTransform.sizeDelta = new Vector2(singleSize, 0);
                textWeeks[i].rectTransform.localPosition = new Vector3(i * singleSize, 0, 0);
                textWeeks[i].color = UI.theme.weekTextColor;
                textWeeks[i].fontSize = UI.theme.weekTextSize;
            }

        }

        void UpdateText()
        {
            if (UI == null) return;
            if (UI.language == null) return;
            if (UI.theme == null) return;

            if (UI.sundayIsFirst)
            {
                textWeeks[0].text = UI.language.Sunday;
                textWeeks[1].text = UI.language.Monday;
                textWeeks[2].text = UI.language.Tuesday;
                textWeeks[3].text = UI.language.Wednesday;
                textWeeks[4].text = UI.language.Thursday;
                textWeeks[5].text = UI.language.Friday;
                textWeeks[6].text = UI.language.Saturday;
            }
            else
            {
                textWeeks[0].text = UI.language.Monday;
                textWeeks[1].text = UI.language.Tuesday;
                textWeeks[2].text = UI.language.Wednesday;
                textWeeks[3].text = UI.language.Thursday;
                textWeeks[4].text = UI.language.Friday;
                textWeeks[5].text = UI.language.Saturday;
                textWeeks[6].text = UI.language.Sunday;
            }
        }
    }
}