using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadMatch()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}

