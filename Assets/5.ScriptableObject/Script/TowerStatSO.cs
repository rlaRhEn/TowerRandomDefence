using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TowerStatSO", menuName = "Scriptable Objects/TowerStatSO")]
public class TowerStatSO : ScriptableObject
{
    [Serializable]
    public struct Row
    {
        public TowerType type;
        public int level;
        public int power;
        public float attackSpeed;
    }

    [SerializeField] private List<Row> rows = new();

    Dictionary<(TowerType, int), Row> cache;

    public void SetRows(List<Row> newRows)
    {
        rows = newRows;
        cache = null;
    }

    public Row Get(TowerType type, int level)
    {
        cache ??= BuildCache();

        if (!cache.TryGetValue((type, level), out var row))
        {
            return default;
        }

        return row;
    }

    Dictionary<(TowerType, int), Row> BuildCache()
    {
        var dic = new Dictionary<(TowerType, int), Row>();
        if (rows == null) rows = new List<Row>();

        for (int i = 0; i < rows.Count; i++)
        {
            var r = rows[i];
            dic[(r.type, r.level)] = r; // 중복이면 마지막 값
        }
        return dic;
    }
}
