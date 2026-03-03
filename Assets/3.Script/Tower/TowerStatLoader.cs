using System;
using System.Collections.Generic;
using UGS;
using UnityEngine;


public class TowerStatLoader : MonoBehaviour
{
    [SerializeField] private TowerStatSO towerStats;

    public void LoadFromGoogle(System.Action onDone)
    {
        UnityGoogleSheet.LoadFromGoogle<int, TowerStats.Data>((list, map) =>
        {
            var soRows = new List<TowerStatSO.Row>(list.Count);

            foreach (var x in list)
            {
                if (!Enum.TryParse<TowerType>(x.type, true, out var towerType))
                {
                    continue;
                }

                soRows.Add(new TowerStatSO.Row
                {
                    type = towerType,
                    level = x.level,
                    power = x.power,
                    attackSpeed = x.attackSpeed
                });
            }

            towerStats.SetRows(soRows);
            onDone?.Invoke();
        });
    }


}
