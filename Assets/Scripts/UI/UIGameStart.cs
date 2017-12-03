using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameStart : MonoBehaviour
{
    public void EnterMainScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
