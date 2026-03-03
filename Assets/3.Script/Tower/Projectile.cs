using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float power;
    public float moveSpeed = 5f;
    public Transform target;
    public Vector2 endPos;

    ProjectileFactory factory;
  
    void Update()
    {
        if (!IsTargetNull()) DeSpawn();
        Move();
    }
    public void Init(ProjectileFactory factory,Transform target, float power)
    {
        this.factory = factory;
        this.target = target;
        this.power = power;


    }

    public virtual void Move()
    {
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster")) return;
        if(collision.transform != target) return;

        //충돌한 몬스터에게 데미지 주기
        Monster monster = collision.GetComponent<Monster>();
        if(monster!=null)
        {
            monster.TakeDamage(power);
        }
        DeSpawn();
    }
    bool IsTargetNull()
    {
        return target != null && target.gameObject.activeInHierarchy;
    }
    public virtual void DeSpawn()
    {
        if (factory != null) factory.DeSpawn(this);
    }
}
