using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int Scene_Menu = 0;
    public const int Scene_Game = 1;
    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
