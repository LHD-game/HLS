using UnityEngine;

public class TouchCameraScroll : MonoBehaviour //모바일 기기의 터치 감지로 스크롤을 지원해주는 코드

{
    public float scrollSpeed = 0.1f;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 deltaPosition = touch.deltaPosition;
                transform.Translate(-deltaPosition.x * scrollSpeed * Time.deltaTime, -deltaPosition.y * scrollSpeed * Time.deltaTime, 0);
            }
        }
    }
}
