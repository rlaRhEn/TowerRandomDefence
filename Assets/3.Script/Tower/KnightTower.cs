using UnityEngine;

public class KnightTower : Tower
{
    public override void AttackAnimation()
    {
        spum_Prefabs.PlayAnimation("5_Skill_Normal");
    }
}
