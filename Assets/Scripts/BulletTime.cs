using System.Collections;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    public Camera cam;            
    public float slowFactor = 0.3f; // Time scale during bullet time
    public float zoomSize = 3.5f;   // Camera zoom level in bullet time
    public float normalZoom = 5f;   // Normal camera size
    public float zoomDuration = 0.5f; // Zoom transition duration

    private bool isBulletTimeActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (isBulletTimeActive)
                DeactivateBulletTime();
            else
                ActivateBulletTime();
        }
    }

    public void toggleBulletTime()
    {
        if (isBulletTimeActive)
        {
            DeactivateBulletTime();
        }
        else
        {
            ActivateBulletTime();
        } 
    }
    void ActivateBulletTime()
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        StartCoroutine(ZoomEffect(zoomSize));
        isBulletTimeActive = true;
    }

    void DeactivateBulletTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        StartCoroutine(ZoomEffect(normalZoom));
        isBulletTimeActive = false;
    }

    // Camera zoom when activated
    IEnumerator ZoomEffect(float targetSize)
    {
        float startSize = cam.orthographicSize;
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsed / zoomDuration);
            elapsed += Time.unscaledDeltaTime; 
            yield return null;
        }

        cam.orthographicSize = targetSize;
    }
}
