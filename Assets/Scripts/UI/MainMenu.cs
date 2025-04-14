using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        Debug.Log("START GAME!");
        SceneManager.LoadScene("Room1");
    }
    public void ExitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
