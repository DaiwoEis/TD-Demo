using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class SceneChangeEffect<T> : MonoBehaviour, ISceneChangeEffect where T : Object
{
    public SceneChangeEffectConfigData configData;

    public T ConfigData
    {
        get { return configData.As<T>(); }
    }

    public void Run(bool reverse, Action onComplete)
    {
        StartCoroutine(_Run(reverse, onComplete));
    }

    protected abstract IEnumerator _Run(bool reverse, Action onComplete = null);
}

public interface ISceneChangeEffect
{
    void Run(bool reverse = false, Action onComplete = null);
}