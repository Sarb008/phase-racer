
using UnityEngine;

public class PhaseTrackLighting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PhaseManager phaseManager;
    [SerializeField] private Renderer[] glowRenderers;

    [Header("Phase Colors")]
    [SerializeField, ColorUsage(true, true)]
    private Color blueEmission = new Color(0f, 2f, 5f, 1f);

    [SerializeField, ColorUsage(true, true)]
    private Color redEmission = new Color(5f, 0.1f, 0.25f, 1f);

    private static readonly int EmissionColorId =
        Shader.PropertyToID("_EmissionColor");

    private MaterialPropertyBlock propertyBlock;

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        if (phaseManager == null)
        {
            Debug.LogError("PhaseTrackLighting: PhaseManager missing.");
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
        Color emission = phase == GamePhase.Blue
            ? blueEmission
            : redEmission;

        foreach (Renderer glowRenderer in glowRenderers)
        {
            if (glowRenderer == null)
                continue;

            // Preserve any existing property overrides.
            glowRenderer.GetPropertyBlock(propertyBlock);

            propertyBlock.SetColor(EmissionColorId, emission);

            glowRenderer.SetPropertyBlock(propertyBlock);

            propertyBlock.Clear();
        }
    }
}
