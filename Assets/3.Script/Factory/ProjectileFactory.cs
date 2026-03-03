using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public enum ProjectileType
{
    Arrow,
    Blade,
    Fireball
}
[Serializable]
public class ProjectilePoolSetting
{
    public ProjectileType type;
    public Projectile projectilePrefab;
    public int initialCount = 5;
}
public class ProjectileFactory : MonoBehaviour
{

    public ProjectilePoolSetting[] settings;
    Dictionary<ProjectileType, ObjectPool<Projectile>> pools = new Dictionary<ProjectileType, ObjectPool<Projectile>>();
    Dictionary<Projectile, ProjectileType> spawnedType = new Dictionary<Projectile, ProjectileType>();


    private void Awake()
    {
        for (int i = 0; i < settings.Length; i++)
        {
            ProjectilePoolSetting setting = settings[i];

            GameObject poolObj = new GameObject($"Pool_{setting.type}");
            poolObj.transform.SetParent(transform);

            pools[setting.type] = new ObjectPool<Projectile>(setting.projectilePrefab, setting.initialCount, poolObj.transform);
        }
    }
    public Projectile Spawn(ProjectileType type, Vector3 pos, Transform target, float power)
    {
        if (!pools.TryGetValue(type, out ObjectPool<Projectile> pool))
            return null;

        Projectile proj = pool.GetObject(); //이때 트루
        proj.transform.position = pos;
        spawnedType[proj] = type;
        proj.Init(this,target, power);
      
        return proj;
    }
    public void DeSpawn(Projectile p)
    {
        if (p == null) return;

        if (!spawnedType.TryGetValue(p, out ProjectileType type))
        {
            Destroy(p.gameObject);
            return;
        }

        pools[type].ReturnObject(p);
        spawnedType.Remove(p);
    }
}
