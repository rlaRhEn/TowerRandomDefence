using UnityEngine;

public class MonsterAniEvent : MonoBehaviour
{
    [SerializeField] Monster monster; // 부모 몬스터 참조

    private void Awake()
    {
        if (monster == null)
            monster = GetComponentInParent<Monster>();
    }

    // Animation Event에서 호출할 함수 이름
    public void OnDeathAnimationEnd()
    {
        if (monster != null)
            monster.OnDeadAnimationEnd();
    }
}
