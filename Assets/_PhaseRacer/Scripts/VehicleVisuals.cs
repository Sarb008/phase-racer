
using UnityEngine;

public class VehicleVisuals : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform visual;

    [Header("Banking")]
    [SerializeField, Range(0f, 35f)]
    private float maxBankAngle = 15f;

    [SerializeField, Min(0.1f)]
    private float bankSensitivity = 2f;

    [SerializeField, Min(0.1f)]
    private float bankSmoothness = 10f;

    [Header("Hover")]
    [SerializeField, Min(0f)]
    private float hoverAmplitude = 0.12f;

    [SerializeField, Min(0.1f)]
    private float hoverFrequency = 2f;

    private float previousX;
    private float currentBank;

    private Vector3 initialVisualPosition;

    private void Start()
    {
        if (player == null || visual == null)
        {
            Debug.LogError("VehicleVisuals: Assign Player and Visual.");
            enabled = false;
            return;
        }

        previousX = player.position.x;
        initialVisualPosition = visual.localPosition;
    }

    private void LateUpdate()
    {
        // Calculate lateral movement.
        float lateralVelocity = Time.deltaTime > 0f
            ? (player.position.x - previousX) / Time.deltaTime
            : 0f;

        previousX = player.position.x;

        // Calculate banking.
        float targetBank = Mathf.Clamp(
            -lateralVelocity * bankSensitivity,
            -maxBankAngle,
            maxBankAngle
        );

        float t = 1f - Mathf.Exp(-bankSmoothness * Time.deltaTime);

        currentBank = Mathf.Lerp(currentBank, targetBank, t);

        visual.localRotation = Quaternion.Euler(0f, 0f, currentBank);

        // Calculate hover bobbing.
        float hoverOffset = Mathf.Sin(
            Time.time * hoverFrequency * Mathf.PI * 2f
        ) * hoverAmplitude;

        visual.localPosition = initialVisualPosition
            + Vector3.up * hoverOffset;
    }
}
