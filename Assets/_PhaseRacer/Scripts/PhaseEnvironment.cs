
using UnityEngine;

public class PhaseEnvironment : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhaseManager phaseManager;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Light directionalLight;

    [Header("BLUE - Order")]
    [SerializeField]
    private Color blueBackground =
        new Color(0.025f, 0.065f, 0.13f);

    [SerializeField]
    private Color blueLight =
        new Color(0.65f, 0.85f, 1f);

    [Header("RED - Chaos")]
    [SerializeField]
    private Color redBackground =
        new Color(0.13f, 0.015f, 0.035f);

    [SerializeField]
    private Color redLight =
        new Color(1f, 0.35f, 0.4f);

    private void OnEnable()
    {
        if (phaseManager == null)
        {
            Debug.LogError("PhaseEnvironment: PhaseManager missing.");
            enabled = false;
            return;
        }

        phaseManager.OnPhaseChanged += ApplyPhase;

        ApplyPhase(phaseManager.CurrentPhase);
    }

    private void OnDisable()
    {
        if (phaseManager != null)
            phaseManager.OnPhaseChanged -= ApplyPhase;
    }

    private void ApplyPhase(GamePhase phase)
    {
        bool isBlue = phase == GamePhase.Blue;

        if (mainCamera != null)
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor =
                isBlue ? blueBackground : redBackground;
        }

        if (directionalLight != null)
        {
            directionalLight.color =
                isBlue ? blueLight : redLight;
        }
    }
}
