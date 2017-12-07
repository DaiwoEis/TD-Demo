using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Switcher/MoveSwitcher")]
public class MoveSwitcher : UIWindowSwitcher
{
    public Vector3 direction;

    public float distance;

    public override void OnOpen(UIWindow window)
    {
        var rs = window.transform as RectTransform;
        rs.DOMove(direction * distance, openingTime).SetRelative();
    }

    public override void OnClose(UIWindow window)
    {
        var rs = window.transform as RectTransform;
        rs.DOMove(-direction * distance, closingTime).SetRelative();
    }
}
