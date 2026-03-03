using UnityEngine;
using System.Collections;
using UGS;

public class StatBootsTrapManager : MonoBehaviour
{
    [SerializeField] TowerStatLoader towerLoader;
    [SerializeField] MonsterStatLoader monsterLoader;

    IEnumerator Start()
    {
        bool towerDone = false;
        bool monsterDone = false;

        towerLoader.LoadFromGoogle(() => towerDone = true);
        while (!towerDone) yield return null;

        monsterLoader.LoadFromGoogle(() => monsterDone = true);
        while (!monsterDone) yield return null;
    }
}