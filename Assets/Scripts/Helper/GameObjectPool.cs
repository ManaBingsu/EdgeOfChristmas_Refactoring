using Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    private int initNum = 6;
    [SerializeField]
    private GameObject poolingObjectPrefab;
    public Queue<T> poolingObjectQueue = new Queue<T>();

    private void Awake()
    {
        Initialize(initNum);
    }
    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }
    private T CreateNewObject()
    {
        var newObj = Instantiate(poolingObjectPrefab).GetComponent<T>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }
    public T GetObject()
    {
        if (poolingObjectQueue.Count > 0)
        {
            var obj = poolingObjectQueue.Dequeue();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        poolingObjectQueue.Enqueue(obj);
    }
}
