using UnityEngine;

public class DamageTextFactory : MonoBehaviour
{
    [SerializeField] DamageText prefab;
    [SerializeField] Transform worldCanvas;

    ObjectPool<DamageText> pool;

    private void Awake()
    {
        pool = new ObjectPool<DamageText>(prefab, 30, worldCanvas);
    }


    public void Spawn(Vector3 pos, float damage)
    {
        DamageText txt = pool.GetObject();
        txt.Init(this);
        txt.transform.position = pos;
        txt.ShowDamageText(damage);
    }
    public void DeSpawn(DamageText txt)
    {
        pool.ReturnObject(txt);
    }   
}
