using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class SimpleObjectPool
{
    private DiContainer _container;

    private GameObject _prefab;     
    private int _poolSize;     
    private Queue<GameObject> _poolQueue;
    private HashSet<GameObject> _allObjects;

    public SimpleObjectPool(DiContainer container)
    {
        _container = container;
    }

    public void CreatePool(GameObject prefab, int poolSize, bool instansiateWDependencies)
    {
        if (prefab == null || poolSize <= 0)
        {
            Debug.LogError($"Trying to create incorrect pool! prefab: {prefab} poolsize: {poolSize}");
        }
        _poolSize = poolSize;
        _prefab = prefab;
        _poolQueue = new Queue<GameObject>();
        _allObjects = new HashSet<GameObject>();

        if (instansiateWDependencies) {
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject obj = _container.InstantiatePrefab(_prefab);
                obj.gameObject.SetActive(false);
                _allObjects.Add(obj);
                _poolQueue.Enqueue(obj);
            }
            return;
        }

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = GameObject.Instantiate(_prefab);
            obj.gameObject.SetActive(false);
            _allObjects.Add(obj);
            _poolQueue.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (_poolQueue.Count > 0)
        {
            GameObject obj = _poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.LogWarning($"Object pool is empty or null! Failed to create {_prefab}");
            return null;
        }
    }
   
    public void ReturnObject(GameObject obj)
    {
        if (!_allObjects.Contains(obj))
        {
            Debug.LogError("This object does not belong to this pool!");
            return;
        }

        if (_poolQueue.Contains(obj))
        {
            Debug.LogWarning("This object is already in the pool!");
            return;
        }

        obj.SetActive(false);
        _poolQueue.Enqueue(obj);
    }
}
