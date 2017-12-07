using Newtonsoft.Json;
using UnityEngine;

[MonoSingletonUsage]
public class GameManager : MonoSingleton<GameManager>
{

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
