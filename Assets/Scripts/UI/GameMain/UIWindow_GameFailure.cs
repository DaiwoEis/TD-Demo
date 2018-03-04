using UnityEngine.UI;

public class UIWindow_GameFailure : UIWindow 
{

    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Exit").onClick.AddListener(() =>
        {
            GameController.Instance.ShutDown();
        });
    }
}
