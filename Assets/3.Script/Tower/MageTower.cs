using UnityEngine;

public class MageTower : Tower
{
    //공격 애니메이션 

    public override void AttackAnimation()
    {
        spum_Prefabs.PlayAnimation("5_Skill_Magic");
    }
}
