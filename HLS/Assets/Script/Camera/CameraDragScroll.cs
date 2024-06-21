using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2.0f; // �巡�� �ӵ� ����

    private Vector3 dragOrigin; // �巡�� ���� ��ġ

    void Update()
    {
        // ���콺 ���� ��ư�� ������ �� �巡�� ���� ��ġ ����
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        // ���콺 ���� ��ư�� ������ �ִ� ���� ȭ���� �巡��
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(difference.x * dragSpeed, difference.y * dragSpeed, 0);

            Camera.main.transform.Translate(-move, Space.World); // ī�޶� �̵�
            dragOrigin = Input.mousePosition;
        }
    }
}
