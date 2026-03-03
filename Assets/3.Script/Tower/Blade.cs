using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Blade : Projectile
{
    
    public override void Move()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        transform.position += direction * moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }
}
