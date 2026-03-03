using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] MonsterFactory factory;
    [SerializeField] Transform spawnPoint;
    Vector3 spawnOffset;

    public Monster SpawnMonster(int level)
    {
        Vector3 pos = spawnPoint.position + spawnOffset;
        return factory.Spawn(level, pos);

    }

}
