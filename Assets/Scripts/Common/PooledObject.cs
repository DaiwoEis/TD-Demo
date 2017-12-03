using System;
using UnityEngine;

public sealed class PooledObject : MonoBehaviour
{
    public ObjectPool OwnerPool { get; set; }

    public event Action<PooledObject> OnRetrieveFromPool;

    public event Action<PooledObject> OnReturnToPool;

    public void RetrieveFromPool()
    {
        if (OnRetrieveFromPool != null)
            OnRetrieveFromPool(this);
    }

    public void ReturnToPool()
    {
        if (OnReturnToPool != null)
            OnReturnToPool(this);
    }
}
