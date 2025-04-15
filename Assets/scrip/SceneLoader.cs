using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadMatch()
    {
        SceneManager.LoadScene("Match");
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

