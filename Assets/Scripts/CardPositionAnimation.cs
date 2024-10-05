using UnityEngine;

public class CardPositionAnimation : MonoBehaviour
{
    private float startedAt;
    public float animationDuration;
    private bool init = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    public void GoTo(Vector3 position, Quaternion rotation)
    {
        init = true;
        startedAt = Time.time;
        startPosition = transform.position;
        startRotation = transform.rotation;
        targetPosition = position;
        targetRotation = rotation;
    }

    private void Update()
    {
        if (!init)
        {
            return;
        }
        var elapsed = Time.time - startedAt;
        var framePosition = Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp01(elapsed / animationDuration));
        var frameRotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.Clamp01(elapsed / animationDuration));
        transform.SetPositionAndRotation(framePosition, frameRotation);
        // transform.SetLocalPositionAndRotation(framePosition, frameRotation);
    }
}
