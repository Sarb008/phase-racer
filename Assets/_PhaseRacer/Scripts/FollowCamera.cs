
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -11f);
    [SerializeField, Min(0f)] private float followSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Desired position relative to the player's logical position.
        Vector3 desiredPosition = target.position + offset;

        // Smoothly follow the player without changing camera rotation.
        float t = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            t
        );
    }
}
