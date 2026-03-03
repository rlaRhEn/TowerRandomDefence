using UnityEngine;
using System.Collections.Generic;
using System;

public enum TowerType
{
    Archer,
    Knight,
    Mage
}
[Serializable]
public class TowerPoolSetting
{
    public TowerType type;
    public Tower towerPrefab;
    public int initialCount = 5;
}
public class TowerFactory : MonoBehaviour
{
    [SerializeField] TowerPoolSetting[] poolSettings;
    

    private readonly List<Tower> aliveTowers = new List<Tower>();
    Dictionary<TowerType, ObjectPool<Tower>> pools = new Dictionary<TowerType, ObjectPool<Tower>>();
    Dictionary<Tower, TowerType> spawnedType = new Dictionary<Tower, TowerType>(); 

    void Awake()
    {
        for (int i = 0; i < poolSettings.Length; i++)
        {
            TowerPoolSetting setting = poolSettings[i];

            GameObject poolObj = new GameObject($"Pool_{setting.type}");
            poolObj.transform.SetParent(transform);

            pools[setting.type] = new ObjectPool<Tower>(setting.towerPrefab, setting.initialCount, poolObj.transform);
        }
    }

    public Tower Spawn(TowerType type, Vector3 position)
    {
        if (!pools.TryGetValue(type, out ObjectPool<Tower> pool))
            return null;

        Tower tower = pool.GetObject();
        tower.transform.position = position;


        spawnedType[tower] = type;
        Register(tower);
        return tower;
    }

    public void DeSpawn(Tower tower)
    {
        if (tower == null) return;

        if (!spawnedType.TryGetValue(tower, out TowerType type))
        {
            Destroy(tower.gameObject);
            return;
        }

        Unregister(tower);
        pools[type].ReturnObject(tower);
        spawnedType.Remove(tower);
    }

    public void Register(Tower tower)
    {
        if (tower != null && !aliveTowers.Contains(tower))
            aliveTowers.Add(tower);
    }

    public void Unregister(Tower tower)
    {
        if (tower != null)
            aliveTowers.Remove(tower);
    }

    public IEnumerable<Tower> GetAliveTowers(TowerType type)
    {
        for (int i = 0; i < aliveTowers.Count; i++)
        {
            var t = aliveTowers[i];
            if (t == null) continue;
            if (!t.gameObject.activeInHierarchy) continue;
            if (t.Type == type) yield return t;
        }
    }

}
