using UnityEngine.UI;

public class UIWindow_MainMenu : UIWindow 
{

    private void Awake()
    {
        transform.FindChildComponentByName<Button>("Btn_Setting").onClick.AddListener(() =>
        {
            OpenWindowAndPauseSelf<UIWindow_Setting>();
        });

        transform.FindChildComponentByName<Button>("Btn_About").onClick.AddListener(() =>
        {
            OpenWindowAndPauseSelf<UIWindow_About>();
        });

        transform.FindChildComponentByName<Button>("Btn_Exit").onClick.AddListener(() =>
        {
            GameManager.Instance.QuitGame();
        });
    }
}
