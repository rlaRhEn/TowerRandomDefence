using System;
using System.Collections;
using Unity.Hierarchy;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] protected MonsterStatSO stats;


    [field: SerializeField]
    public int Level { get; private set; }
    [field: SerializeField]

    public float MaxHp { get; private set; }
    [field: SerializeField]

    public float Speed { get; private set; }

    float curHp;
    float curSpeed;
    [SerializeField] Transform[] wayPoints;
    int currentWayPointIndex = 0;

    Vector3 moveDirection;

    MonsterFactory factory;
    SPUM_Prefabs spum_Prefabs;
    public Action<Monster> OnReachedGoal;   // 도착(라이프 깎이는 상황)
    public Action<Monster> OnDead;          // 타워에게 죽음

    private void Awake()
    {
        spum_Prefabs = GetComponent<SPUM_Prefabs>(); 
    }
    private void OnEnable()
    {
        curHp = MaxHp;
        currentWayPointIndex = 0;

        GameObject[] wayPointsObj = GameObject.FindGameObjectsWithTag("WayPoint");

        Array.Sort(wayPointsObj, (a, b) => a.name.CompareTo(b.name));

        wayPoints = new Transform[wayPointsObj.Length];
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = wayPointsObj[i].transform;
        }
        spum_Prefabs.PlayAnimation("run");
        Speed = curSpeed;
        
    }
    private void FixedUpdate()
    {
        transform.position += moveDirection * Speed * Time.deltaTime;
    }
    public void Init(MonsterFactory factory, int level, float hp, float speed)
    {
        this.factory = factory;
        Level = level;
        MaxHp = hp;
        Speed = speed;
        curHp = MaxHp;
        curSpeed = Speed;

        NextMoveTo();
        StartCoroutine(MoveTo());
    }
  
    IEnumerator MoveTo()
    {
        while(currentWayPointIndex < wayPoints.Length)
        {
            if (Vector3.Distance(transform.position, wayPoints[currentWayPointIndex].position) < 0.05f ) 
            {
                NextMoveTo();
            }
            yield return null;
        }
    }
    void NextMoveTo()
    {
        currentWayPointIndex++;
        if (currentWayPointIndex >= wayPoints.Length)
        {
            OnReachedGoal?.Invoke(this);

            Die();
            return;
        }
        
        Vector3 direction = (wayPoints[currentWayPointIndex].position - transform.position).normalized;
        
        moveDirection = direction;

    }
    public void TakeDamage(float damage)
    {

        curHp -= damage;
        GameManager.instance.damageTextFactory.Spawn(transform.position + Vector3.up * 0.5f, damage);
        if (curHp <= 0)
        {
            GameManager.instance.stageController.gold++;
            GameManager.instance.uiManager.SetGold(GameManager.instance.stageController.gold);
            OnDead?.Invoke(this);
            Die();

        }
    }
    public void Die() //일반적인 사망
    {
        if (!gameObject.activeSelf) return;
        
        spum_Prefabs.PlayAnimation("death");
        Speed = 0;
    }
    public void OnDeadAnimationEnd()
    {
        factory.DeSpawn(this);
        OnDead?.Invoke(this);
        GameManager.instance.soundManager.PlayClip(GameManager.instance.soundManager.monsterDieClip, 0.3f);
    }
}
