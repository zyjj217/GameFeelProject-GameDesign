using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float shakeDuration = 0.2f;  // How long the shake lasts
    public float shakeMagnitude = 0.3f; // How intense the shake is

    private Vector3 originalPosition;
    private float shakeTimeRemaining = 0f;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            // Randomize camera position slightly for shake effect
            transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            // Reset camera to original position when shake ends
            transform.localPosition = originalPosition;
        }
    }

    // Call this method to trigger a screen shake
    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        shakeTimeRemaining = duration;
    }
}
