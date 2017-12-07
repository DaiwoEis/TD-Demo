using UnityEngine;

public abstract class UIWindowSwitcher : ScriptableObject
{
    public float openingTime;

    public float closingTime;

    public abstract void OnOpen(UIWindow window);

    public abstract void OnClose(UIWindow window);
}