using UnityEngine;
using System.Collections.Generic;


public class ObjectPooler : MonoBehaviour
{
    public bool autoPoolChildren = false;
    private List<PooledObject> _pooledObjects = new List<PooledObject>();
    private GameObject _prefab;
    private Transform _parentTransform;
    public Transform attachToOnGet;

    void Awake()
    {
        if (autoPoolChildren)
        {
            AutoPoolChildren();
        }

        _parentTransform = transform;

        gameObject.SetActive(false);
    }

    public void AutoPoolChildren()
    {
        this.gameObject.SetActive(false);
        // add each child to pool
        for (int x = 0; x < transform.childCount; ++x)
        {
            GameObject obj = transform.GetChild(x).gameObject;
            PooledObject poolObj = obj.GetComponent<PooledObject>();
            if (poolObj == null)
            {
                poolObj = obj.AddComponent<PooledObject>();
            }
            poolObj.pool = this;
            _pooledObjects.Add(poolObj);

            obj.SetActive(true);
        }

        if (_pooledObjects.Count > 0)
        {
            _prefab = _pooledObjects[0].gameObject;
        }
    }

    public void InitPool(GameObject prefab, int initialSize)
    {
        _parentTransform = transform;
        _prefab = prefab;
        for (int x = 0;x < initialSize;++x)
        {
            CreateObject();
        }
    }

    public GameObject Get()
    {
        if (_pooledObjects.Count == 0)
        {
            if (_prefab == null)
            {
                Debug.LogError("Need more in object pool " + name);
                return null;
            }

            CreateObject();
        }

        GameObject ret = _pooledObjects[0].gameObject;
        //ret.SetActive(true);
        _pooledObjects[0].SetAllocated(true);
        _pooledObjects.RemoveAt(0);

        if (attachToOnGet != null)
        {
            ret.transform.SetParent(attachToOnGet);
        }
        return ret;
    }

    public T Get<T>()
    {
        GameObject go = Get();

        if (go == null)
        {
            return default(T);
        }

        return go.GetComponent<T>();
    }

    void CreateObject()
    {
        if (_prefab == null)
        {
            Debug.LogError("Pooler missing prefab");
            return;
        }
        GameObject obj = Instantiate(_prefab);
        //obj.SetActive(false);
        PooledObject poolObj = obj.GetComponent<PooledObject>();
        if (poolObj == null)
        {
            poolObj = obj.AddComponent<PooledObject>();
        }
        poolObj.pool = this;
        _pooledObjects.Add(poolObj);

        poolObj.transform.SetParent(this.transform, false);


        RectTransform rect = obj.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localScale = new Vector3(1, 1, 1);
        }
    }

    public void ReturnToPool(PooledObject inObj)
    {
        if (_pooledObjects.Contains(inObj))
        {
            Debug.LogError("Object Already Freed");
            return;
        }
        inObj.SetAllocated(false);
        _pooledObjects.Add(inObj);
        //inObj.gameObject.SetActive(false);
        Debug.Log("Return " + inObj.name + " to " + _parentTransform.name);
        inObj.transform.SetParent(_parentTransform, false);
    }
}

