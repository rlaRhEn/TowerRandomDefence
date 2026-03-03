using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ArcherTower : Tower
{

    public override void AttackAnimation()
    {
        spum_Prefabs.PlayAnimation("5_Skill_Bow");
    }



}
