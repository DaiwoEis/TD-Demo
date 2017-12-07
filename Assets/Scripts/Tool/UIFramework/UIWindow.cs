using System;
using UnityEngine;

public class UIWindow : MonoBehaviour 
{
    public event Action<UIWindow> onOpeningStart;
    public event Action<UIWindow> onOpeningComplete;

    public event Action<UIWindow> onClosingStart;
    public event Action<UIWindow> onClosingComplete;

    public string ID { get { return WindowID(GetType()); } }

    public string WindowID(Type type)
    {
        return UIManager.Instance.GetWindowID(type);
    }

    protected void OpenWindow<T>() where T : UIWindow
    {
        UIManager.Instance.OpenWindow(WindowID(typeof(T)));
    }

    protected void OpenWindowAndPauseSelf<T>() where T : UIWindow
    {
        UIManager.Instance.CloseWindow(ID);
        UIManager.Instance.GetWindow(WindowID(typeof(T))).onClosingStart += OnResume;
        UIManager.Instance.OpenWindow(WindowID(typeof(T)));
    }

    private void OnResume(UIWindow closingWindow)
    {
        closingWindow.onClosingStart -= OnResume;
        UIManager.Instance.OpenWindow(ID);
    }

    protected void CloseSelf()
    {
        UIManager.Instance.CloseWindow(ID);
    }

    public void TriggerOnOpeningStartEvent()
    {
        if (onOpeningStart != null) onOpeningStart(this);
    }

    public void TriggerOnOpeningCompleteEvent()
    {
        if (onOpeningComplete != null) onOpeningComplete(this);
    }

    public void TriggerOnClosingStartEvent()
    {
        if (onClosingStart != null) onClosingStart(this);
    }

    public void TriggerOnClosingCompleteEvent()
    {
        if (onClosingComplete != null) onClosingComplete(this);
    }

    public event Action onUpdate;

    public virtual void OnUpdate()
    {
        if (onUpdate != null) onUpdate();
    }

}

