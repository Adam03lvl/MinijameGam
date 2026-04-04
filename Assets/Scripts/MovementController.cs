using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.2f;
    public float cameraDistance = 5f;
    public Transform cameraPivot; // drag your pivot child object here

    private float _yaw;
    private float _pitch = 20f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        Vector2 mouse = Mouse.current.delta.ReadValue();
        _yaw += mouse.x * mouseSensitivity;
        _pitch -= mouse.y * mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, 7f, 60f);

        Vector2 input = new Vector2(
            (Keyboard.current.dKey.isPressed ? 1 : 0) - (Keyboard.current.aKey.isPressed ? 1 : 0),
            (Keyboard.current.wKey.isPressed ? 1 : 0) - (Keyboard.current.sKey.isPressed ? 1 : 0)
        );

        Quaternion camYaw = Quaternion.Euler(0f, _yaw, 0f);
        Vector3 moveDir = (camYaw * new Vector3(input.x, 0f, input.y)).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Camera.main.transform.position =
            rotation * new Vector3(0f, 0f, -cameraDistance) + cameraPivot.position;
        Camera.main.transform.rotation = rotation;
    }
}
