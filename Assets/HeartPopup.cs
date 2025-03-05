// HeartPopup.cs
using UnityEngine;

public class HeartPopup : MonoBehaviour
{
    public float floatSpeed = 1f;    // vertical movement speed
    public float fadeSpeed = 1f;     // alpha fade speed
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // Move the heart upwards
        transform.Translate(Vector3.up * floatSpeed * Time.deltaTime);

        // Fade out
        if (spriteRenderer != null)
        {
            float newAlpha = spriteRenderer.color.a - (fadeSpeed * Time.deltaTime);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            // Once fully invisible, destroy
            if (newAlpha <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
