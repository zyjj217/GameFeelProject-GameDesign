using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGameScript : MonoBehaviour
{
    public GameObject endPanel;
    public PlayerScript player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        endPanel.SetActive(true);
        player.gameOver = true;
    }

    public void restartGame()
    {
        SceneManager.LoadScene("Map");
    }
}
