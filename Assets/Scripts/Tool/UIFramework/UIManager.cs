using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Dictionary<string, UIWindow> _windowDic;

    private Dictionary<string, UIDialog> _dialogDic;

    private Stack<UIWindow> _windowStack;

    private List<UIDialog> _dialogs;

    private UIWindow _globalWindow;

    private Transform _canvasTrans;

    protected override void OnCreate()
    {
        base.OnCreate();

        _windowDic = new Dictionary<string, UIWindow>();
        _windowStack = new Stack<UIWindow>();
        _dialogDic = new Dictionary<string, UIDialog>();
        _dialogs = new List<UIDialog>();
 
        GetCanvas();
        var canvas = _canvasTrans.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.planeDistance = 1f;
    }

    private void GetCanvas()
    {
        _canvasTrans = GameObject.FindWithTag("UICanvas").transform;
        if (_canvasTrans == null)
            _canvasTrans = Instantiate(ResourceManager.Load<GameObject>("UICanvas")).transform;
    }

    public void FindWindowInScene()
    {
        foreach (Transform childTran in _canvasTrans)
        {
            var window = childTran.GetComponent<UIWindow>();
            if (window == null) continue;
            
            string windowID = window.ID;
            if(!_windowDic.ContainsKey(windowID))
                _windowDic.Add(windowID, window);  
            window.gameObject.SetActive(false);
        }
    }

    public void OpenGloablWindow(string windowID)
    {
        _globalWindow = GetWindow(windowID);
        StartCoroutine(_OpenWindowInternal(_globalWindow, _globalWindow.swicher));
    }

    public void OpenWindow(string windowID)
    {       
        var nextWindow = GetWindow(windowID);
        StartCoroutine(_OpenWindow(nextWindow));
    }

    public void BackToLastWindow()
    {
        if (_windowStack.Count > 0)
        {
            StartCoroutine(_BackToLastWindow());
        }
        else
        {
            Debug.LogWarning("window count must > 0 in window stack can back to last window!");
        }
    }

    public void OpenDialog(string dialogID)
    {
        var dialog = GetDialog(dialogID);
        dialog.gameObject.SetActive(true);
        dialog.Enter();
        _dialogs.Add(dialog);
    }

    public void CloseDialog(string dialogID)
    {
        var dialog = _dialogs.Find(d => d.ID == dialogID);
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false);
            dialog.Exit();
            _dialogs.Remove(dialog);
        }
        else
        {
            Debug.Log("the dialog " + dialogID + " is not opened.");
        }
    }

    public void CloseAllDialog()
    {
        foreach (var dialog in _dialogs)
        {
            dialog.gameObject.SetActive(false);
            dialog.Exit();
        }

        _dialogs.Clear();        
    }
    
    public UIWindow GetWindow(string windowID)
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

    public UIDialog GetDialog(string dialogID)
    {
        if (_dialogDic.ContainsKey(dialogID))
            return _dialogDic[dialogID];

        var windowPrefab = ResourceManager.Load<GameObject>(dialogID);
        var dialog = Instantiate(windowPrefab, _canvasTrans).GetComponent<UIDialog>();
        dialog.gameObject.SetActive(false);
        dialog.gameObject.name = dialog.gameObject.name.Replace("(Clone)", "").TrimEnd();
        _dialogDic.Add(dialogID, dialog);
        return dialog;
    }

    private IEnumerator _OpenWindow(UIWindow nextWindow)
    {
        if (_windowStack.Count != 0)
        {
            UIWindow lastWindow = _windowStack.Peek();
            if (lastWindow != null)
                yield return StartCoroutine(_CloseWindowInternal(lastWindow, lastWindow.swicher));
        }         
        yield return StartCoroutine(_OpenWindowInternal(nextWindow, nextWindow.swicher));
        _windowStack.Push(nextWindow);
    }

    private IEnumerator _BackToLastWindow()
    {
        UIWindow lastWindow = _windowStack.Pop();
        if (lastWindow != null)
            yield return _CloseWindowInternal(lastWindow, lastWindow.swicher);

        if (_windowStack.Count > 0)
        {
            UIWindow nextWindow = _windowStack.Peek();
            yield return _OpenWindowInternal(nextWindow, nextWindow.swicher);
        }
    }

    private IEnumerator _CloseWindowInternal(UIWindow window, UIWindowSwitcher switcher)
    {
        window.TriggerOnClosingStartEvent();
        switcher.OnClose(window);
        yield return new WaitForSecondsRealtime(switcher.closingTime);
        window.TriggerOnClosingCompleteEvent();
        window.gameObject.SetActive(false);
    }

    private IEnumerator _OpenWindowInternal(UIWindow window, UIWindowSwitcher switcher)
    {
        window.gameObject.SetActive(true);
        window.TriggerOnOpeningStartEvent();
        switcher.OnOpen(window);
        yield return new WaitForSecondsRealtime(switcher.openingTime);
        window.TriggerOnOpeningCompleteEvent();
    }
}
