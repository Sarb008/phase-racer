
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Forward Movement")]
    [SerializeField, Min(0f)]
    private float forwardSpeed = 12f;

    [Header("Lane Movement")]
    [SerializeField, Min(0.1f)]
    private float laneSpacing = 5f;

    [SerializeField, Min(0.1f)]
    private float laneChangeSpeed = 12f;

    [Header("Input Actions")]
    [SerializeField]
    private InputActionReference laneLeftAction;

    [SerializeField]
    private InputActionReference laneRightAction;

    // 0 = left, 1 = center, 2 = right.
    private int currentLane = 1;

    private float centerX;

    private void Awake()
    {
        centerX = transform.position.x;
    }

    private void OnEnable()
    {
        if (laneLeftAction != null)
        {
            laneLeftAction.action.performed += OnLaneLeft;
            laneLeftAction.action.Enable();
        }

        if (laneRightAction != null)
        {
            laneRightAction.action.performed += OnLaneRight;
            laneRightAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (laneLeftAction != null)
        {
            laneLeftAction.action.performed -= OnLaneLeft;
            laneLeftAction.action.Disable();
        }

        if (laneRightAction != null)
        {
            laneRightAction.action.performed -= OnLaneRight;
            laneRightAction.action.Disable();
        }
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;

        // Move forward automatically.
        Vector3 position = transform.position;
        position.z += forwardSpeed * deltaTime;

        // Convert the selected lane into a world-space X coordinate.
        float targetX = centerX + (currentLane - 1) * laneSpacing;

        // Move smoothly toward the selected lane.
        position.x = Mathf.MoveTowards(
            position.x,
            targetX,
            laneChangeSpeed * deltaTime
        );

        transform.position = position;
    }

    private void OnLaneLeft(InputAction.CallbackContext context)
    {
        currentLane = Mathf.Max(0, currentLane - 1);
    }

    private void OnLaneRight(InputAction.CallbackContext context)
    {
        currentLane = Mathf.Min(2, currentLane + 1);
    }
}
