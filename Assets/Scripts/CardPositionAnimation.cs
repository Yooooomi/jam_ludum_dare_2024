using UnityEngine;

public class CardPositionAnimation : MonoBehaviour
{
    private float startedAt;
    public float animationDuration;
    private bool init = false;
    private float thisAnimationDuration;
    private Vector3 startPosition;
    private float startScale;
    private Vector3 targetPosition;
    private float targetScale;
    private Quaternion startRotation;
    private Quaternion targetRotation;

    public void GoTo(Vector3 position, Quaternion rotation, float inSeconds = -1f, float scale = 1.0f)
    {
        float finalInSeconds = inSeconds == -1f ? animationDuration : inSeconds;
        thisAnimationDuration = finalInSeconds;
        init = true;
        startedAt = Time.time;
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale.x;
        targetPosition = position;
        targetRotation = rotation;
        targetScale = scale;
    }

    private void Update()
    {
        if (!init)
        {
            return;
        }
        var elapsed = Time.time - startedAt;
        var framePosition = Vector3.Lerp(startPosition, targetPosition, Mathf.Clamp01(elapsed / thisAnimationDuration));
        var frameRotation = Quaternion.Lerp(startRotation, targetRotation, Mathf.Clamp01(elapsed / thisAnimationDuration));
        var frameScale = Mathf.Lerp(startScale, targetScale, Mathf.Clamp01(elapsed / thisAnimationDuration));
        transform.SetPositionAndRotation(framePosition, frameRotation);
        transform.localScale = new Vector3(frameScale, frameScale, frameScale);
    }
}
