using System.Collections.Generic;
using UnityEngine;

[MonoSingletonUsage(false)]
public class EntityManager : MonoSingleton<EntityManager>
{
    private Dictionary<string, List<GameObject>> _trailedEntities;

    private Dictionary<GameObject, ObjectPool> _objectPools;

    protected override void OnCreate()
    {
        base.OnCreate();

        _trailedEntities = new Dictionary<string, List<GameObject>>();
        _objectPools = new Dictionary<GameObject, ObjectPool>();
    }

    public List<GameObject> GetGameObjectWithTag(string goTag)
    {
        if (_trailedEntities.ContainsKey(goTag))
            return _trailedEntities[goTag];
        //Debug.LogWarning(string.Format("The tag {0} gameobject can't trailed", goTag));
        return new List<GameObject>();
    }

    public GameObject CreateGO(GameObject prefab, Vector3 pos, Quaternion rotation, bool trialed = false)
    {
        if (!_objectPools.ContainsKey(prefab))
            CreatePool(prefab);

        var go = _objectPools[prefab].Instantiate(pos, rotation);

        if (trialed)
            AddTrialed(go);

        return go;
    }

    private void CreatePool(GameObject templete)
    {
        _objectPools[templete] = ObjectPool.CreateObjectPool(templete);
        _objectPools[templete].transform.SetParent(transform);
    }

    private void AddTrialed(GameObject go)
    {
        if (_trailedEntities.ContainsKey(go.tag))
            _trailedEntities[go.tag].Add(go);
        else
            _trailedEntities[go.tag] = new List<GameObject> {go};
    }

    public void DestroyGO(GameObject go, float time = 0f)
    {
        if (_trailedEntities.ContainsKey(go.tag))
            _trailedEntities[go.tag].Remove(go);

        var templete = go.GetComponent<PooledObject>().OwnerPool.Templete;
        _objectPools[templete].Destroy(go, time);
    }
}
