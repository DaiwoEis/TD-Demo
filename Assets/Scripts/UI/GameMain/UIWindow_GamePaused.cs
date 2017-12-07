using UnityEngine.UI;

public class UIWindow_GamePaused : UIWindow 
{

    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Restart").onClick.AddListener(() =>
        {
            GameController.Instance.GameRestart();
        });

        transform.FindChildComponentByName<Button>("Btn_Exit").onClick.AddListener(() =>
        {
            GameController.Instance.ShutDown();
        });
    }
}
