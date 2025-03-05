using UnityEngine;
using UnityEngine.SceneManagement;
public class KillZoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when player enters killzone reload scene
        SceneManager.LoadScene("Map");
    }
}
