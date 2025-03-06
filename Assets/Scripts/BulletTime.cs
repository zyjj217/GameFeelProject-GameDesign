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

    // for fun and more effects
    public GameObject afterimagePrefab;  
    public float afterimageSpawnRate = 0.1f; // Time interval for spawning afterimages
    private Coroutine afterimageCoroutine;

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

        // Start spawning afterimages
        afterimageCoroutine = StartCoroutine(SpawnAfterimages());
    }

    void DeactivateBulletTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        StartCoroutine(ZoomEffect(normalZoom));
        isBulletTimeActive = false;

        // Stop afterimage effect
        if (afterimageCoroutine != null)
        {
            StopCoroutine(afterimageCoroutine);
        }
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

    IEnumerator SpawnAfterimages()
    {
        while (isBulletTimeActive)
        {
            GameObject afterimage = Instantiate(afterimagePrefab, transform.position, transform.rotation);
            SpriteRenderer afterimageRenderer = afterimage.GetComponent<SpriteRenderer>();
            SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();

            if (afterimageRenderer && playerRenderer)
            {
                afterimageRenderer.sprite = playerRenderer.sprite;
                afterimageRenderer.color = new Color(1, 1, 1, 0.5f); 
            }

            Destroy(afterimage, 0.5f); // Destroy afterimage after a short time
            yield return new WaitForSeconds(afterimageSpawnRate);
        }
    }
}
