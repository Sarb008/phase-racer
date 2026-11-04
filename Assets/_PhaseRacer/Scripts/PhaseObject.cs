
using UnityEngine;

public class PhaseObject : MonoBehaviour
{
    [Header("Phase Settings")]
    [SerializeField] private GamePhase activePhase = GamePhase.Blue;

    [Header("References")]
    [SerializeField] private PhaseManager phaseManager;

    private Renderer[] renderers;
    private Collider[] colliders;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);
    }

    private void OnEnable()
    {
        if (phaseManager == null)
        {
            Debug.LogError(
                $"PhaseObject: PhaseManager not assigned on {name}.");
            return;
        }

        phaseManager.OnPhaseChanged += HandlePhaseChanged;

        // Sync immediately with the current phase.
        UpdatePhaseState(phaseManager.CurrentPhase);
    }

    private void OnDisable()
    {
        if (phaseManager != null)
            phaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void HandlePhaseChanged(GamePhase newPhase)
    {
        UpdatePhaseState(newPhase);
    }

    private void UpdatePhaseState(GamePhase currentPhase)
    {
        bool isActive = currentPhase == activePhase;

        foreach (Renderer renderer in renderers)
            renderer.enabled = isActive;

        foreach (Collider collider in colliders)
            collider.enabled = isActive;
    }
}
