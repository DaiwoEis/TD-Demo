using UnityEngine.UI;

public class UIWindow_About : UIWindow 
{

    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Back").onClick.AddListener(() =>
        {
            UIManager.Instance.BackToLastWindow();
        });
    }
}
