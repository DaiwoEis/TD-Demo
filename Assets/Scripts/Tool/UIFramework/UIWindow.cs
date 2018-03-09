using System;
using UnityEngine;

public class UIWindow : MonoBehaviour 
{
    public event Action<UIWindow> onOpeningStart;
    public event Action<UIWindow> onOpeningComplete;

    public event Action<UIWindow> onClosingStart;
    public event Action<UIWindow> onClosingComplete;

    public string ID;

    public UIWindowSwitcher swicher;

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

