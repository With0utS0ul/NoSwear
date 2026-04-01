using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 100f;

    [Header("Clamping")]
    public float verticalClampMin = -70f;
    public float verticalClampMax = 70f;

    [Header("References")]
    public Transform cameraPivot;

    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraPivot == null)
        {
            Debug.LogError("CameraPivot не назначен в инспекторе!");
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, verticalClampMin, verticalClampMax);

        cameraPivot.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}