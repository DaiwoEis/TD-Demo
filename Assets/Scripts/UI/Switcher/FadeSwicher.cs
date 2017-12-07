using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(menuName = "Switcher/FadeSwitcher")]
public class FadeSwicher : UIWindowSwitcher 
{

    public override void OnOpen(UIWindow window)
    {
        var group = window.RealGetComponent<CanvasGroup>();
        group.alpha = 0f;
        group.DOFade(1f, openingTime).onComplete += () =>
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        };
    }

    public override void OnClose(UIWindow window)
    {
        var group = window.RealGetComponent<CanvasGroup>();
        group.alpha = 1f;
        window.GetComponent<CanvasGroup>().DOFade(0f, closingTime).onComplete += () =>
        {
            group.interactable = false;
            group.blocksRaycasts = false;
        };
    }

    //private void OnClosingComplete(UIWindow window)
    //{
    //    var group = window.GetComponent<CanvasGroup>();
    //    group.interactable = false;
    //    group.blocksRaycasts = false;
    //    window.onClosingComplete -= OnClosingComplete;
    //}
}
