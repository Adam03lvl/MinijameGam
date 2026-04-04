using UnityEngine;
using UnityEngine.InputSystem;

public class Harpoon : MonoBehaviour
{
    public float maxDistance = 10f;
    public float speed = 20f;
    public float timeToFullCharge = 2f;
    public Transform ropeSource;

    private LineRenderer _line;
    private Vector3 _homePos;
    private bool _shooting;
    private float _chargeTime; // Track raw time
    private float _targetDist;
    private bool _returning;

    void Awake()
    {
        _line = GetComponent<LineRenderer>();
        // Store the starting local position (relative to the player/camera)
        _homePos = transform.localPosition;

        _line.enabled = false;
    }

    void Update()
    {
        // Using Mouse.current is fine, but ensure the Input System package is active
        bool held = Mouse.current.leftButton.isPressed;

        // charging with DeltaTime
        if (held && !_shooting)
        {
            _chargeTime += Time.deltaTime;
        }

        // Release & fire
        if (!held && _chargeTime > 0 && !_shooting)
        {
            float chargePercent = Mathf.Clamp01(_chargeTime / timeToFullCharge);
            _targetDist = chargePercent * maxDistance;

            _chargeTime = 0f;
            _shooting = true;
            _returning = false;
        }

        // harpoon movement logic
        if (_shooting)
        {
            // Calculate where we want to be relative to the parent
            float targetZ = _returning ? _homePos.z : _homePos.z + _targetDist;
            Vector3 localGoal = new Vector3(_homePos.x, _homePos.y, targetZ);

            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                localGoal,
                speed * Time.deltaTime
            );

            // Check if reached goal
            if (Vector3.Distance(transform.localPosition, localGoal) < 0.01f)
            {
                if (!_returning)
                {
                    _returning = true;
                }
                else
                {
                    transform.localPosition = _homePos;
                    _shooting = false;
                }
            }
        }
    }

    void LateUpdate()
    {
        if (_shooting)
        {
            _line.enabled = true;

            // 0 is the gun/hand, 1 is the harpoon
            _line.SetPosition(0, ropeSource.position);
            _line.SetPosition(1, transform.position);
        }
        else
        {
            _line.enabled = false;
        }
    }
}
