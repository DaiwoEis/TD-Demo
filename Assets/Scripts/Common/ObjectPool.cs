using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Object Pool")]
public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject _templete;

    public GameObject Templete { get { return _templete; } }

    [SerializeField]
    private int _initSize = 10;

    [SerializeField] private bool _instantiateOnAwake = false;

    [SerializeField]
    private List<GameObject> _allObjectList;

    [SerializeField]
    private List<GameObject> _availableObjectList;

    public string PoolName { get { return gameObject.name; } }

    private void Awake()
    {
        if (_instantiateOnAwake)
        {            
            InstantiatePool();
        }
    }

    public static ObjectPool CreateObjectPool(GameObject template, int initSize = 0)
    {
        string poolName = "Pool of " + template.name;
        GameObject go = new GameObject(poolName);
        ObjectPool pool = go.AddComponent<ObjectPool>();
        pool._templete = template;
        pool._initSize = initSize;

        pool.InstantiatePool();

        return pool;
    }

    public void InstantiatePool()
    {
        _allObjectList = new List<GameObject>(_initSize);
        _availableObjectList = new List<GameObject>(_initSize);

        if (_templete == null)
        {
            Debug.LogError("Object Pool: " + PoolName + ": Template GameObject is null! Make sure you assigned a template either in the inspector or in your scripts.");
            return;
        }

        ClearPool();

        for (int i = 0; i < _initSize; i++)
        {
            GameObject go = NewActiveObject();
            _allObjectList.Add(go);
            _availableObjectList.Add(go);
            go.SetActive(false);
        }
    }

    public GameObject Instantiate(Vector3 pos, Quaternion rot)
    {
        GameObject newGO;

        if (_availableObjectList.Count > 0)
        {
            int lastIndex = _availableObjectList.Count - 1;
            if (_availableObjectList[lastIndex] == null)
            {
                Debug.LogError("EZObjectPool " + PoolName + " has missing objects in its pool! Are you accidentally destroying any GameObjects retrieved from the pool?");
                return null;
            }

            newGO = _availableObjectList[lastIndex];
            _availableObjectList.RemoveAt(lastIndex);
        }
        else
        {
            newGO = NewActiveObject();
            _allObjectList.Add(newGO);
        }

        newGO.transform.position = pos;
        newGO.transform.rotation = rot;

        newGO.SetActive(true);

        newGO.GetComponent<PooledObject>().RetrieveFromPool();

        return newGO;
    }

    private GameObject NewActiveObject()
    {
        GameObject newGO = Instantiate(_templete, transform);
        PooledObject pooledObject = newGO.GetComponent<PooledObject>() ?? newGO.AddComponent<PooledObject>();
        pooledObject.OwnerPool = this;

        return newGO;
    }

    public void Destroy(GameObject pooledObject, float time = 0f)
    {
        if (time.Equals(0f))
        {
            DestroyInternal(pooledObject);
        }
        else
        {
            this.Invoke(time, () => { DestroyInternal(pooledObject); });
        }        
    }

    private void DestroyInternal(GameObject pooledObject)
    {
        var pooled = pooledObject.GetComponent<PooledObject>();
        if (pooled == null || pooled.OwnerPool != this)
        {
            Debug.LogWarning(string.Format("Object {0} is not {1} pool member", pooledObject.name, PoolName));
            return;
        }

        pooled.ReturnToPool();
        pooledObject.SetActive(false);
        pooledObject.transform.SetParent(transform);
        _availableObjectList.Add(pooledObject);
    }

    public void ClearPool()
    {
        foreach (GameObject go in _allObjectList)
        {
            Destroy(go);
        }

        _allObjectList.Clear();
        _availableObjectList.Clear();
    }

    public int ActiveObjectCount()
    {
        return _allObjectList.Count - _availableObjectList.Count;
    }

    public int AvailableObjectCount()
    {
        return _availableObjectList.Count;
    }
}