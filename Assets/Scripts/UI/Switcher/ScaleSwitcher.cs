using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Switcher/ScaleSwitcher")]
public class ScaleSwitcher : UIWindowSwitcher
{
    public override void OnOpen(UIWindow window)
    {
        window.transform.localScale = Vector3.zero;
        ((RectTransform) window.transform).DOScale(Vector3.one, openingTime);
    }

    public override void OnClose(UIWindow window)
    {
        ((RectTransform) window.transform).DOScale(Vector3.zero, openingTime);
    }
}
