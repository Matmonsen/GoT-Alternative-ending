using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int Scene_Menu = 0;
    public const int Scene_Game = 1;

    void Awake()
    {

        Screen.SetResolution(1280, 720, true);
    }
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void ReloadScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1;
    }
}
