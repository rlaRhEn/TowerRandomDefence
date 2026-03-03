using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;



public class MonsterFactory : MonoBehaviour
{
    [SerializeField] MonsterStatSO monsterStats;
    [SerializeField] int initialCount = 20;

    Dictionary<int, ObjectPool<Monster>> poolsByLevel = new Dictionary<int, ObjectPool<Monster>>(); // 레벨 전용 오브젝트 풀
    Dictionary<Monster, int> spawnedLevel = new Dictionary<Monster, int>(); //현재 살아있는 몬스터의 레벨
    Transform poolRoot;
    private void Awake()
    {
        poolRoot = new GameObject($"Pool_Monster").transform;
        poolRoot.transform.SetParent(transform);

    }
    public IEnumerable<Monster> GetAliveMonsters()
    {
        return spawnedLevel.Keys; // Dictionary<Monster,int>의 Key = Monster 인스턴스

    }
    ObjectPool<Monster> GetOrCreatePool(int level)
    {
        if (poolsByLevel.TryGetValue(level, out var pool))
            return pool;

        var row = monsterStats.Get(level);
        if (row.monsterPrefab == null)
            return null;

        var prefabMonster= row.monsterPrefab.GetComponent<Monster>();
        if(prefabMonster == null)
            return null;

        pool = new ObjectPool<Monster>(prefabMonster, initialCount, poolRoot);
        poolsByLevel[level] = pool;
        return pool;
    }
    public Monster Spawn(int level, Vector3 position)
    {
        var pool = GetOrCreatePool(level);
        if (pool == null)
            return null;
        var row = monsterStats.Get(level);

        Monster monster = pool.GetObject();
        monster.transform.position = position;

        monster.Init(this, level, row.hp, row.speed);
        spawnedLevel[monster] = level;
        return monster;

    }
    public void DeSpawn(Monster monster)
    {
        if (monster == null) return;
        
        if(!spawnedLevel.TryGetValue(monster, out var level))
        {
            Destroy(monster.gameObject);
            return;
        }
        spawnedLevel.Remove(monster);
        poolsByLevel[level].ReturnObject(monster);
    }
    public void ReleaseLevelPool(int level)
    {
        //해당 스테이지 레벨의 몬스터 풀 제거
        if(!poolsByLevel.TryGetValue(level, out var pool))
            return;
        pool.DestroyAll();
        poolsByLevel.Remove(level);

    }
}
