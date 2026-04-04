using UnityEngine;
using UnityEngine.InputSystem;

public class Harpoon : MonoBehaviour
{
    public float maxDistance = 10f;
    public float speed = 20f;
    public float timeToFullCharge = 2f;
    public float chargeSpeed = 2f;

    public Transform lineStart; // drag an empty GameObject here
    private LineRenderer _line;
    private Vector3 _homePos;
    private bool _shooting;
    private float _chargeTime;
    private float _targetDist;
    private bool _returning;

    void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _homePos = transform.localPosition;
        _line.enabled = false;
    }

    void Update()
    {
        bool held = Mouse.current.leftButton.isPressed;

        if (held && !_shooting)
            _chargeTime += Mathf.Lerp(0, timeToFullCharge, Time.deltaTime * chargeSpeed);

        if (!held && _chargeTime > 0 && !_shooting)
        {
            _targetDist = Mathf.Clamp01(_chargeTime / timeToFullCharge) * maxDistance;
            _chargeTime = 0f;
            _shooting = true;
            _returning = false;
        }

        if (_shooting)
        {
            float goalZ = _returning ? _homePos.z : _homePos.z + _targetDist;
            Vector3 goal = new Vector3(_homePos.x, _homePos.y, goalZ);
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                goal,
                speed * Time.deltaTime
            );

            if (Vector3.Distance(transform.localPosition, goal) < 0.01f)
            {
                if (!_returning)
                    _returning = true;
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
        _line.enabled = _shooting;
        if (_shooting)
        {
            _line.SetPosition(0, lineStart.position); // rope starts at player origin
            _line.SetPosition(1, transform.position); // rope ends at harpoon tip
        }
    }
}
