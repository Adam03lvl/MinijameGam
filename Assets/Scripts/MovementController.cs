using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 0.2f;

    public float cameraDistance = 5f; // how far behind the player
    public float cameraHeight = 1.6f; // what point on the player to orbit around

    private CharacterController _cc;
    private float _yaw;
    private float _pitch = 20f; // start looking slightly down

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        _yaw = transform.eulerAngles.y;
    }

    void Update()
    {
        // Mouse look
        Vector2 mouse = Mouse.current.delta.ReadValue();
        _yaw += mouse.x * mouseSensitivity;
        _pitch -= mouse.y * mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, -8f, 60f);

        // WASD — body always faces the yaw direction
        transform.rotation = Quaternion.Euler(0f, _yaw, 0f);

        Vector2 input = new Vector2(
            Keyboard.current.dKey.isPressed ? 1
                : Keyboard.current.aKey.isPressed ? -1
                : 0,
            Keyboard.current.wKey.isPressed ? 1
                : Keyboard.current.sKey.isPressed ? -1
                : 0
        );

        Vector3 move = (transform.right * input.x + transform.forward * input.y) * moveSpeed;
        move.y = -2f;
        _cc.Move(move * Time.deltaTime);

        // Orbit camera around a point at cameraHeight on the player
        Vector3 pivot = transform.position + Vector3.up * cameraHeight;
        Quaternion orbitRot = Quaternion.Euler(_pitch, _yaw, 0f);
        Camera.main.transform.position = pivot + orbitRot * new Vector3(0f, 0f, -cameraDistance);
        Camera.main.transform.LookAt(pivot);
    }
}
