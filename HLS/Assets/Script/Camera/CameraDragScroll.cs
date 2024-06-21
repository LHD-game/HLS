using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2.0f; // 드래그 속도 조절

    private Vector3 dragOrigin; // 드래그 시작 위치

    void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때 드래그 시작 위치 저장
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        // 마우스 왼쪽 버튼을 누르고 있는 동안 화면을 드래그
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(difference.x * dragSpeed, difference.y * dragSpeed, 0);

            Camera.main.transform.Translate(-move, Space.World); // 카메라 이동
            dragOrigin = Input.mousePosition;
        }
    }
}
