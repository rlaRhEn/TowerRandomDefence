using System;
using System.Collections.Generic;
using UnityEngine;


public class TowerUpgradeManager 
{
    Dictionary<TowerType, int> levelByType = new(); //레벨
    Dictionary<TowerType, int> costByType = new(); //비용
    

    public event Action<TowerType, int> OnTypeLevelChanged;
    public TowerUpgradeManager()
    {
        levelByType[TowerType.Archer] = 1;
        levelByType[TowerType.Mage] = 1;
        levelByType[TowerType.Knight] = 1;

        costByType[TowerType.Archer] = 50;
        costByType[TowerType.Mage] = 60;
        costByType[TowerType.Knight] = 55;
    }
    public int GetLevel(TowerType type) => levelByType[type];

    public int GetUpgradeCost(TowerType type)
    {
        int lv = levelByType[type];
        return costByType[type] + (lv - 1) * 5; //비용 5원씩 상승
    }

    public bool TryUpgrade(TowerType type, ref int gold)
    {
        int cost = GetUpgradeCost(type);
        if (gold < cost) return false;

        gold -= cost;
        levelByType[type]++;

        OnTypeLevelChanged?.Invoke(type, levelByType[type]);
        return true;
    }
}
