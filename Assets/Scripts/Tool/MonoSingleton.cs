using System;
using System.Linq;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                Create();
            return instance;
        }
    }

    public static void Create()
    {
        instance = FindObjectOfType(typeof(T)) as T;
        if (instance == null)
        {
            var type = typeof(T);
            string goName = "Singleton of " + type;
            instance = new GameObject(goName).AddComponent<T>();

            MonoSingletonUsageAttribute attribure =
                type.GetCustomAttributes(typeof(MonoSingletonUsageAttribute), false).FirstOrDefault() as
                    MonoSingletonUsageAttribute;
            if (attribure != null && attribure.DontDestroyOnLoad)
            {
                DontDestroyOnLoad(instance.gameObject);
            }
        }
        instance.OnCreate();
    }

    protected virtual void OnCreate()
    {
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class MonoSingletonUsageAttribute : Attribute
{
    public bool DontDestroyOnLoad;

    public MonoSingletonUsageAttribute(bool dontDestroyOnLoad = true)
    {
        DontDestroyOnLoad = dontDestroyOnLoad;
    }
}