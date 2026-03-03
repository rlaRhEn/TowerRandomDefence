using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "MonsterStatSO", menuName = "Scriptable Objects/MonsterStatSO")]
public class MonsterStatSO : ScriptableObject
{
    [Serializable]
    public struct Row
    {
        public int level;
        public float hp;
        public float speed;
        public GameObject monsterPrefab;
    }

    [SerializeField] List<Row> rows = new();
    Dictionary<int, Row> cache;

    public void SetRows(List<Row> newRows)
    {
        
        var prefabByLevel = new Dictionary<int, GameObject>();
        for (int i = 0; i < rows.Count; i++)
        {
            prefabByLevel[rows[i].level] = rows[i].monsterPrefab;
        }
        rows = newRows; 
        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];

            if (prefabByLevel.TryGetValue(r.level, out var prefab))
                r.monsterPrefab = prefab;             
            rows[i] = r;                         
        }

        cache = null;
    }
    public Row Get(int level)
    {
        cache ??= BuildCache();
        if (!cache.TryGetValue(level, out var row))
        {
            return default;
        }
        return row;
    }
    Dictionary<int, Row> BuildCache()
    {
        var dic = new Dictionary<int, Row>();
        if (rows == null) rows = new List<Row>();
        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];
            dic[r.level] = r; // 중복이면 마지막 값
        }
        return dic;
    }
}
