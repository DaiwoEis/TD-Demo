using UnityEngine;

public class UIController : MonoBehaviour 
{

	private void Start () 
	{
	    UIManager.Instance.FindWindowInScene();
	    UIManager.Instance.OpenWindow(UIWindowID.MainMenu);
    }
}
