using UnityEngine;

public class TouchCameraScroll : MonoBehaviour //����� ����� ��ġ ������ ��ũ���� �������ִ� �ڵ�

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
