using UnityEngine;
using UnityEngine.SceneManagement;
public class KillZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when player enters kill zone reload scene
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Map");
        }
        else
        {
            collision.gameObject.SetActive(false); // deactivate any enemies that fall into kill zone
        }
        
    }
}
