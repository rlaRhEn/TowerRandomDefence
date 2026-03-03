using System.Collections;
using UnityEngine;

public class Fireball : Projectile
{

    BoxCollider2D boxCollider;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        boxCollider.enabled = false;
    }
    //수정
    public override void Move()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z),
            moveSpeed * Time.deltaTime);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster")) return;
        if (collision.transform != target) return;

        //충돌한 몬스터에게 데미지 주기
        Monster monster = collision.GetComponent<Monster>();
        if (monster != null)
        {
            monster.TakeDamage(power);
        }
        
        DeSpawn();
    }
    public void HitAnimation()
    {
        boxCollider.enabled = true;
    }
}
