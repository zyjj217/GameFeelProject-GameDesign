using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healthIncrease = 1;  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerScript playerScript = other.GetComponent<PlayerScript>();
            if(playerScript != null)
            {
                // Increase player's health
                playerScript.IncreaseHealth(healthIncrease);

                // Trigger any pickup visual effects on the player
                playerScript.OnHealthPickup();

                Debug.Log("Player picked up health!");
            }

            // Destroy the health item after pickup
            Destroy(gameObject);
        }
    }
}
