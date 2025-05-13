using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float maxLookX = 90f;
    public float minLookX = -90f;
    public float scrollSpeed = 20f; // vitesse de zoom (scroll)

    private float rotX; // Rotation verticale (pitch)

    void Update()
    {
        Move();
        Look();
        Zoom();
    }

    void Move()
    {
        // Déplacement ZQSD (AZERTY)
        float x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float z = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;

        Vector3 dir = transform.right * x + transform.forward * z;
        transform.position += dir.normalized * moveSpeed * Time.deltaTime;
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotX -= mouseY;
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        transform.localRotation = Quaternion.Euler(rotX, 0f, 0f);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            transform.parent.position += transform.parent.forward * scroll * scrollSpeed;
        }
    }
}
