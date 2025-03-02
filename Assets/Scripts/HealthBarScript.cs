using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public void setHealth(int health)
    {
        slider.value = health;
    }
}
