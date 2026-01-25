using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float Sensitivity = 2f;
    public Transform PlayerBody;

    float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X") * Sensitivity;
        float my = Input.GetAxis("Mouse Y") * Sensitivity;

        pitch -= my;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        if (PlayerBody) PlayerBody.Rotate(Vector3.up * mx);
    }
}
