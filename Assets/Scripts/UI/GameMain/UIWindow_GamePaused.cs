using UnityEngine.UI;

public class UIWindow_GamePaused : UIWindow 
{

    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Back").onClick
            .AddListener(() => UIManager.Instance.BackToLastWindow());

        transform.FindChildComponentByName<Button>("Btn_Restart").onClick.AddListener(() =>
        {
            GameController.Instance.GameRestart();
        });

        transform.FindChildComponentByName<Button>("Btn_Exit").onClick.AddListener(() =>
        {
            GameController.Instance.ShutDown();
        });

        transform.FindChildComponentByName<Button>("Btn_Setting").onClick
            .AddListener(() => UIManager.Instance.OpenWindow(UIWindowID.Setting));
    }
}
