using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public enum TowerState
    {
        Idle,     //대기 및 적 추적
        Attack    //공격
    }
    [SerializeField]TowerState currentState = TowerState.Idle;
    TowerState prevState;
    float searchTimer;

    public Tile CurrentTile { get; private set; }

   


    [SerializeField]protected TowerStatSO stats;
    [SerializeField]protected TowerType towerType;
    [SerializeField]protected int level;
    [SerializeField]protected int power;
    [SerializeField]protected float attackSpeed;
    [SerializeField]protected float attackRange;

    [SerializeField] GameObject targetMonster;
    protected SPUM_Prefabs spum_Prefabs;
    Coroutine attackCoroutine; // update 코루틴 중복방지
    public TowerType Type => towerType;
    public TowerType GetTowerType(TowerType type) => towerType = type;
    public float GetAttackRange() => attackRange;

    
    private void Awake()
    {
        spum_Prefabs = GetComponent<SPUM_Prefabs>();
    }
    private  void Update()
    {
        if(currentState != prevState)
        {
            OnStateEnter(currentState);
            prevState = currentState;
        }

        // 상태별 "지속 로직"만 실행
        if (currentState == TowerState.Idle) TickIdle();
        else if (currentState == TowerState.Attack) TickAttack();
    }
    protected void ApplyStats()
    {
        var row = stats.Get(towerType, level);
        power = row.power;
        attackSpeed = row.attackSpeed;
    }
    public void ApplyGlobalLevel(int globalLevel)
    {
        level = globalLevel;
        ApplyStats();
    }
    void OnStateEnter(TowerState state)
    {
        switch (state)
        {
            case TowerState.Idle:
                spum_Prefabs.PlayAnimation("0_idle");
                StopAttackCoroutine();
                break;

            case TowerState.Attack:
                AttackAnimation();
                break;
        }
    }

    void TickIdle()
    {
        searchTimer -= Time.deltaTime;
        if (searchTimer > 0f) return;

        searchTimer = 0.15f; // 0.1~0.2 추천
        SearchTarget();
    }
    void TickAttack()
    {
        if (!TargetNull(targetMonster))
        {
            currentState = TowerState.Idle;
            return;
        }

        float d = Vector3.Distance(transform.position, targetMonster.transform.position);
        if (d > attackRange)
        {
            targetMonster = null;
            currentState = TowerState.Idle;
            return;
        }

        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(DoAttack());   
    }

    void SearchTarget()
    {
        
        float closetDistance = Mathf.Infinity;
        Monster closetM = null;

       
       foreach(var monster in GameManager.instance.monsterFactory.GetAliveMonsters())
        {
            if (monster == null || !monster.gameObject.activeInHierarchy)
                continue;
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < closetDistance && distance <= attackRange)
            {
                closetDistance = distance;
                closetM = monster;
            }
        }
      
        if (closetM != null)
        {
            
            targetMonster = closetM.gameObject;
            currentState = TowerState.Attack;
        }
    }
    public virtual IEnumerator DoAttack()
    {
        while (TargetNull(targetMonster))
        {
            ShootProjectile();
           yield return new WaitForSeconds(1f/attackSpeed);
        }
        attackCoroutine = null;
    }
    public void StopAttackCoroutine()
    {
        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;

        }
    }
    public virtual void ShootProjectile()
    {
        switch (towerType)
        {
            case TowerType.Archer:
                GameManager.instance.projectileFactory.Spawn(ProjectileType.Arrow, transform.position, targetMonster.transform, power);
                GameManager.instance.soundManager.PlayClip(GameManager.instance.soundManager.towerAttackClip[0]);
                break;
            case TowerType.Knight:
                GameManager.instance.projectileFactory.Spawn(ProjectileType.Blade, transform.position, targetMonster.transform, power);
                GameManager.instance.soundManager.PlayClip(GameManager.instance.soundManager.towerAttackClip[1]);
                break;
            case TowerType.Mage:
                GameManager.instance.projectileFactory.Spawn(ProjectileType.Fireball, transform.position, targetMonster.transform, power);
                GameManager.instance.soundManager.PlayClip(GameManager.instance.soundManager.towerAttackClip[2]);
                break;
            default:
                break;
        }
        
    }
    public abstract void AttackAnimation();
    bool TargetNull(GameObject tMonster)
    {
        return tMonster != null && tMonster.gameObject.activeInHierarchy;
    }
    public void SetTile(Tile tile)
    {
        CurrentTile = tile;
    }

    public void PlaceOn(Tile tile)
    {
        if (tile == null) return;

        if (CurrentTile != null && CurrentTile.PlacedTower == this)
            CurrentTile.ClearTower();

        CurrentTile = tile;
        tile.SetTower(this);

        transform.position = tile.transform.position;
    }
}
