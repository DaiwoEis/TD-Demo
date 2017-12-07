using System;
using System.Collections;
using UnityEngine;

public static class MonoExtension 
{
    public static Coroutine Invoke(this MonoBehaviour mono, float time, Action action)
    {
        if (time.Equals(0f))
        {
            if (action != null) action();
            return null;
        }
        return mono.StartCoroutine(_TimeAction(time, action));
    }

    public static IEnumerator _TimeAction(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        if (action != null) action();
    }

    public static T RealGetComponent<T>(this MonoBehaviour mono) where T : Component
    {
        var component = mono.GetComponent<T>();
        if (component == null)
            component = mono.gameObject.AddComponent<T>();
        return component;
    }
}
