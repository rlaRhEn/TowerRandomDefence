using UnityEngine;
using System.Collections.Generic;

public class ObjectPool <T> where T : Component
{
    public T objPrefab;  //monster, tower, projectile, damageText
    public Transform parent;
    

    Queue<T> pool = new Queue<T>(); //미사용 오브젝트 보관
    public ObjectPool(T prefab, int count, Transform parent)
    {
        objPrefab = prefab;
        this.parent = parent;
        

        for(int i = 0; i < count; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    public T GetObject()
    {
        T obj = (pool.Count > 0) ? pool.Dequeue() : Object.Instantiate(objPrefab, parent);
     
        obj.gameObject.SetActive(true);
        return obj;
    }
    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(parent);
        pool.Enqueue(obj);
    }
    public void DestroyAll()
    {
        while(pool.Count > 0)
        {
            T obj = pool.Dequeue();
            if(obj != null) Object.Destroy(obj.gameObject);
        }
    }
}
