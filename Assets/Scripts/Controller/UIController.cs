using Newtonsoft.Json;
using UnityEngine;

public class UIController : MonoBehaviour 
{

	private void Start () 
	{
	    UIManager.Instance.SetConfigData(JsonConvert.DeserializeObject<UIManagerConfigData>(ResourceManager.Load<TextAsset>("MainMenu").text));
	    UIManager.Instance.FindWindowInScene();
	    UIManager.Instance.OpenWindow<UIWindow_MainMenu>();	    
    }
}
