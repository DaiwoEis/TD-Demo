using UnityEngine.SceneManagement;

public class CSceneManager 
{
    public const string IntermidiateScene = "IntermediateScene";

    public static string CurrentScene { get { return SceneManager.GetActiveScene().name; } }

    public static string NextScene = "";

    public static int GetSceneNumberByName(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).buildIndex;
    }

    public static string GetSceneNameByNumber(int sceneNumber)
    {
        return SceneManager.GetSceneByBuildIndex(0).name;
    }

    public static void LoadScene(string nextScene)
    {
        NextScene = nextScene;
        SceneManager.LoadScene(IntermidiateScene);
    }
}
