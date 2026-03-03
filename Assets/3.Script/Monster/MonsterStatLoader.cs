using UnityEngine;
using System.Collections.Generic;
using UGS;

public class MonsterStatLoader : MonoBehaviour
{
    [SerializeField] MonsterStatSO monsterStats;

    public void LoadFromGoogle(System.Action onDone)
    {
        UnityGoogleSheet.LoadFromGoogle<int, MonsterStats.Data>((list, map) =>
        {
            var soRows = new List<MonsterStatSO.Row>(list.Count);
            foreach (var x in list)
            {

                soRows.Add(new MonsterStatSO.Row
                {
                    level = x.id,
                    hp = x.hp,
                    speed = x.speed
                });
            }
            monsterStats.SetRows(soRows);
            onDone?.Invoke();
        });
    }

}

