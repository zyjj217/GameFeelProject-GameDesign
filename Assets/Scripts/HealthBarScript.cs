using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public Image glowImage;

    void Awake()
    {
        // Ensure the glow image is invisible at the start by setting alpha to 0.
        if(glowImage != null)
        {
            Color c = glowImage.color;
            c.a = 0f;
            glowImage.color = c;
        }
    }

    public void setHealth(int health)
    {
        slider.value = health;
    }

    public void GlowEffect()
    {
        if (glowImage != null)
        {
            StartCoroutine(DoGlow());
        }
    }

    private IEnumerator DoGlow()
    {
        float glowDuration = 0.5f;
        float timer = 0f;
        
        // Start from invisible (alpha 0)
        Color originalColor = glowImage.color;
        originalColor.a = 0;
        glowImage.color = originalColor;

        // Fade in
        while (timer < glowDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / glowDuration);
            Color c = glowImage.color;
            c.a = alpha;
            glowImage.color = c;
            yield return null;
        }
        
        // Fade out
        timer = 0f;
        while (timer < glowDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / glowDuration);
            Color c = glowImage.color;
            c.a = alpha;
            glowImage.color = c;
            yield return null;
        }
        
        // Ensure it's fully transparent after the glow effect
        Color finalColor = glowImage.color;
        finalColor.a = 0;
        glowImage.color = finalColor;
    }
}
