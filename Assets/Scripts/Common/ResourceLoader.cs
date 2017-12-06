using System;
using Object = UnityEngine.Object;

[Serializable]
public class ResourceLoader<T> where T : Object
{
    public string ResourceName;

    public T Load()
    {
        return ResourceManager.Load<T>(ResourceName);    
    }
}
