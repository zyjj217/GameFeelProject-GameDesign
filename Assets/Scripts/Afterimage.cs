using UnityEngine;
public class Afterimage : MonoBehaviour
{
    private SpriteRenderer sr;
    private float alpha = 0.5f;
    private float fadeSpeed = 2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        alpha -= fadeSpeed * Time.deltaTime;
        sr.color = new Color(1, 1, 1, alpha);

        if (alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
