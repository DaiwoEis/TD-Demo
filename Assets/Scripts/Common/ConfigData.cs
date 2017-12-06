using Sirenix.OdinInspector;
using UnityEngine;

public class ConfigData : SerializedScriptableObject
{
    public virtual void Init()
    {
        
    }

    public T As<T>() where T : Object
    {
        return this as T;
    }
}
