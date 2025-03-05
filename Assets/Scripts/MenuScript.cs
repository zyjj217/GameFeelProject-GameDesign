using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject instructionPanel;
    public void startButton()
    {
        //load main scene
        SceneManager.LoadScene("Map");
    }

    public void openInstructions()
    {
        instructionPanel.SetActive(true);
    }

    public void closeInstructions() { 
        instructionPanel.SetActive(false);
    }
}
