using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerConfigData
{
    public Dictionary<string, string> WindowSwitcherMap;

    public string DefaultSwitcherName;
}

public class UIManager : MonoSingleton<UIManager>
{
    private Dictionary<string, UIWindow> _windowDic;

    private Dictionary<string, UIWindowSwitcher> _windowSwitcherMap;

    private UIWindowSwitcher _defaultSwither;

    private List<UIWindow> _currOpenedWindows;

    private Transform _canvasTrans;

    protected override void OnCreate()
    {
        base.OnCreate();

        _windowDic = new Dictionary<string, UIWindow>();
        _canvasTrans = GameObject.FindWithTag("UICanvas").transform;
        _currOpenedWindows = new List<UIWindow>();
    }

    public void FindWindowInScene()
    {
        foreach (var window in FindObjectsOfType<UIWindow>())
        {
            string windowID = window.GetType().RemoveNameSpace();
            if(!_windowDic.ContainsKey(windowID))
                _windowDic.Add(windowID, window);           
        }
    }

    public void SetConfigData(UIManagerConfigData data)
    {
        if (data.WindowSwitcherMap != null)
        {
            _windowSwitcherMap = new Dictionary<string, UIWindowSwitcher>(data.WindowSwitcherMap.Count);
            foreach (var pair in data.WindowSwitcherMap)
                _windowSwitcherMap.Add(pair.Key, ResourceManager.Load<UIWindowSwitcher>(pair.Value));
        }
        else
        {
            _windowSwitcherMap = new Dictionary<string, UIWindowSwitcher>();
        }

        _defaultSwither = ResourceManager.Load<UIWindowSwitcher>(data.DefaultSwitcherName);
    }

    public void OpenWindow<T>() where T : UIWindow
    {       
        OpenWindow(GetWindowID(typeof(T)));
    }

    private void OpenWindow(string windowID)
    {
        StartCoroutine(_OpenWindowInternal(GetWindow(windowID), GetSwitcher(windowID)));
    }

    public void CloseWindow<T>() where T : UIWindow
    {
        CloseWindow(GetWindowID(typeof(T)));
    }

    private void CloseWindow(string windowID)
    {
        StartCoroutine(_CloseWindowInternal(GetWindow(windowID), GetSwitcher(windowID)));
    }

    public void CloseAllOpenedWindow()
    {
        foreach (var currOpenedWindow in _currOpenedWindows)
        {
            string windowID = GetWindowID(currOpenedWindow.GetType());
            CloseWindow(windowID);
        }
    }

    public T GetWindow<T>() where T : UIWindow
    {
        var window = GetWindow(GetWindowID(typeof(T)));
        return window != null ? window as T : null;        
    }

    private UIWindow GetWindow(string windowID)
    {
        if (_windowDic.ContainsKey(windowID))
            return _windowDic[windowID];

        var windowPrefab = ResourceManager.Load<GameObject>(windowID);
        var window = Instantiate(windowPrefab, _canvasTrans).GetComponent<UIWindow>();     
        window.gameObject.SetActive(false);
        window.gameObject.name = window.gameObject.name.Replace("(Clone)", "").TrimEnd();
        _windowDic.Add(windowID, window);
        return window;
    }

    private string GetWindowID(Type windowType)
    {
        return windowType.RemoveNameSpace();
    }

    private UIWindowSwitcher GetSwitcher(string windowID)
    {
        UIWindowSwitcher switcher;
        if (_windowSwitcherMap.TryGetValue(windowID, out switcher))
            return switcher;

        return _defaultSwither;
    }

    private IEnumerator _CloseWindowInternal(UIWindow window, UIWindowSwitcher switcher)
    {
        window.TriggerOnClosingStartEvent();
        switcher.OnClose(window);
        yield return new WaitForSeconds(switcher.closingTime);
        window.TriggerOnClosingCompleteEvent();
        window.gameObject.SetActive(false);
        _currOpenedWindows.Remove(window);
    }

    private IEnumerator _OpenWindowInternal(UIWindow window, UIWindowSwitcher switcher)
    {
        window.gameObject.SetActive(true);
        window.TriggerOnOpeningStartEvent();
        switcher.OnOpen(window);
        yield return new WaitForSeconds(switcher.openingTime);
        window.TriggerOnOpeningCompleteEvent();
        _currOpenedWindows.Add(window);
    }
}
