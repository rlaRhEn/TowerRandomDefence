using System;
using UnityEngine;


public class TowerSpawner : MonoBehaviour
{
    [SerializeField] TowerFactory factory;
    [SerializeField] Vector3 spawnOffset;

    public void SpawnTower(Tile tile, TowerUpgradeManager towerUpgradeManager)
    {
        Vector3 pos = tile.transform.position + spawnOffset;
        TowerType type = GetRandomTower();
        Tower tower = factory.Spawn(type,  pos);
        tower.GetTowerType(type);
        if (tower == null) return;
        int lv = towerUpgradeManager.GetLevel(type);
        tower.ApplyGlobalLevel(lv);

        tile.SetTower(tower);

    }
    TowerType GetRandomTower()
    {
        int count = Enum.GetValues(typeof(TowerType)).Length;
        int randomIndex = UnityEngine.Random.Range(0, count);
        return (TowerType)randomIndex;
    }
}
