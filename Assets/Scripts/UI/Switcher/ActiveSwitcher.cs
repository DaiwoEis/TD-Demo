using UnityEngine;

[CreateAssetMenu(menuName = "Switcher/ActiveSwitcher")]
public class ActiveSwitcher : UIWindowSwitcher
{
    public override void OnOpen(UIWindow window)
    {
        window.gameObject.SetActive(true);
    }

    public override void OnClose(UIWindow window)
    {
        window.gameObject.SetActive(false);
    }
}
