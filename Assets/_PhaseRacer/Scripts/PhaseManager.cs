
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GamePhase
{
    Blue,
    Red
}

public class PhaseManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    private InputActionReference phaseShiftAction;

    [Header("Phase Settings")]
    [SerializeField]
    private GamePhase startingPhase = GamePhase.Blue;

    public GamePhase CurrentPhase { get; private set; }

    public event Action<GamePhase> OnPhaseChanged;

    private void Awake()
    {
        CurrentPhase = startingPhase;
    }

    private void OnEnable()
    {
        if (phaseShiftAction == null || phaseShiftAction.action == null)
        {
            Debug.LogError("PhaseManager: Phase Shift action is not assigned.");
            enabled = false;
            return;
        }

        phaseShiftAction.action.performed += OnPhaseShift;
        phaseShiftAction.action.Enable();
    }

    private void OnDisable()
    {
        if (phaseShiftAction == null || phaseShiftAction.action == null)
            return;

        phaseShiftAction.action.performed -= OnPhaseShift;
        phaseShiftAction.action.Disable();
    }

    private void OnPhaseShift(InputAction.CallbackContext context)
    {
        TogglePhase();
    }

    public void TogglePhase()
    {
        CurrentPhase = CurrentPhase == GamePhase.Blue
            ? GamePhase.Red
            : GamePhase.Blue;

        Debug.Log($"PHASE RACER: {CurrentPhase} phase activated.");

        OnPhaseChanged?.Invoke(CurrentPhase);
    }
}
