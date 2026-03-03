using JetBrains.Annotations;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [Header("Wave")]
    public int waveLevel = 1; //웨이브 레벨
    public int lifeCount = 10;
    public int monsterCount = 20;
    public float monsterSpawnInterval = 1.5f;
    public float nextWaveTimer = 50; // 몬스터 카운트 0 되면 다음 웨이브 카운트 다운 시작 (몬스터 카운트, 시간 초기화)
    public int maxMonsterCount = 100; // 100초과 시 게임 끝

    public int aliveMonster = 0; //살아있는 몬스터 수 0 되면 다음웨이브

    [Header("Economy")]
    int towerCost = 50;
    public int gold = 100;
    [SerializeField] GameObject gameOverPanel;
    [Header("Click")]
    Tower heldTower;
    Tile heldTowerTile;


    bool isGameOver;
    Transform container;
    InputManager inputManager;

    [SerializeField] TowerSpawner towerSpawner;
    [SerializeField] TowerFactory towerFactory;
    [SerializeField] MonsterSpawner monsterSpawner;
    [SerializeField] MonsterFactory monsterFactory;
    [SerializeField] TowerAttackRange towerAttackRange;
    
    

    TowerUpgradeManager towerUpgradeManager;

    Tile selectedTile;
    Tile previousTile;
    Color orignalColor;

    private void Start()
    {
        
        Init();
        StartCoroutine(WaveLoop());

    }
    void Update()
    {
        OnInputHandler();
        if(lifeCount <= 0 && !isGameOver)
        {
            GameOver();
        }
    }
    void Init()
    {
        towerUpgradeManager = new TowerUpgradeManager();

        inputManager = new InputManager(container);

        RefreshAllUpgradeUI();
    }
    /// <summary>
    /// Input Handler
    /// </summary>


    void OnInputHandler()
    {
        if (!inputManager.isTouchDown) return;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(inputManager.touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider == null)
        {
            towerAttackRange.OffRange();
            return;
        }

        void ClearTileHighlight()
        {
            if (previousTile != null)
            {
                var prevR = previousTile.GetComponent<SpriteRenderer>();
                if (prevR != null) prevR.color = orignalColor;
            }
            previousTile = null;
            selectedTile = null;
        }

        if (hit.collider.CompareTag("Tower"))
        {
            // 1) 타워 선택 시 타일 하이라이트 끄기
            ClearTileHighlight();

            Tower tower = hit.collider.GetComponent<Tower>();
            if (tower == null) return;

            heldTower = tower;
            heldTowerTile = FindTileUnderTower(tower);

            // 타워 선택했으니 어택레인지 켜기
            towerAttackRange.OnRange(tower.transform.position, tower.GetAttackRange());
            return;
        }
        else if (hit.collider.CompareTag("TowerField"))
        {
            // 2) 타일 선택 시 타워 어택레인지 끄기
            towerAttackRange.OffRange();

            Tile tile = hit.collider.GetComponent<Tile>();
            if (tile == null) return;

            // 기존 하이라이트 복구
            if (previousTile != null)
            {
                var prevR = previousTile.GetComponent<SpriteRenderer>();
                if (prevR != null) prevR.color = orignalColor;
            }

            // 현재 타일 하이라이트
            var curR = tile.GetComponent<SpriteRenderer>();
            if (curR != null)
            {
                orignalColor = curR.color;
                Color newColor = curR.color;
                newColor.a = 0.5f;
                curR.color = newColor;
            }

            previousTile = tile;
            selectedTile = tile;

            // 3) 타워 선택된 상태면 타일 1번 클릭으로 바로 이동
            if (heldTower != null)
            {
                TryMoveTower(heldTower, heldTowerTile, tile);

                heldTower = null;
                heldTowerTile = null;
            }

            return;
        }

        // 그 외
        towerAttackRange.OffRange();
    }
    void TryMoveTower(Tower tower,Tile fromTile , Tile toTile)
    {
        if (tower == null || fromTile == null || toTile == null)
        {
            Debug.Log($"Move fail: tower={tower}, from={fromTile}, to={toTile}");
            return;
        }

        if (fromTile == toTile)
        {
            Debug.Log("같은 타일 클릭 - 이동 안함");
            return;
        }

        if (toTile.HasTower)
        {
            Debug.Log("이미 타워가 있음");
            return;
        }

        fromTile.ClearTower();
        toTile.SetTower(tower);
        tower.transform.position = toTile.transform.position;

    }
    Tile FindTileUnderTower(Tower tower)
    {
        if (tower == null) return null;

        Vector2 p = tower.transform.position;
        Collider2D[] cols = Physics2D.OverlapPointAll(p);

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i] != null && cols[i].CompareTag("TowerField"))
                return cols[i].GetComponent<Tile>();
        }
        return null;
    }
    /// <summary>
    /// Build Tower
    /// </summary>
    public void AddGold(int delta)
    {
        gold += delta;
        RefreshAllUpgradeUI();
    }
    public void BuildTower() //버튼
    {
        if (selectedTile == null) return;

        if(gold < towerCost)
        {
            Debug.Log("골드 부족");
            RefreshAllUpgradeUI();
            return;
        }

        if (!selectedTile.CanBuildTower(towerCost))
        {
            Debug.Log("타워 설치 불가");
            return;
        }
        AddGold(-towerCost);
        towerSpawner.SpawnTower(selectedTile, towerUpgradeManager);
        GameManager.instance.soundManager.PlayClip(GameManager.instance.soundManager.towerCreateClip);
    }
    /// <summary>
    /// Upgrad Tower
    /// </summary>
    /// <returns></returns>

  
    public void UpgradeArcher() => TryUpgradeApply(TowerType.Archer);
    public void UpgradeKnight() => TryUpgradeApply(TowerType.Knight);
    public void UpgradeMage() => TryUpgradeApply(TowerType.Mage);

    public void TryUpgradeApply(TowerType type)
    {
        //만렙 시 UI 갱신
        if(IsMaxLevel(type))
        {
            RefreshAllUpgradeUI();
            return;
        }
        //골드 부족/실패 시 UI 갱신
        if(!towerUpgradeManager.TryUpgrade(type, ref gold))
        {
            RefreshAllUpgradeUI();
            return;
        }
        int lv = towerUpgradeManager.GetLevel(type);
        //해당 타입 타워 스탯 반영
        foreach (var t in towerFactory.GetAliveTowers(type))
            t.ApplyGlobalLevel(lv);
        RefreshAllUpgradeUI();
        GameManager.instance.soundManager.PlayClip(GameManager.instance.soundManager.towerUpgradeClip);
    }

    private void RefreshAllUpgradeUI()
    {
        //처음 켜질때 전체  갱신 
        foreach (TowerType t in Enum.GetValues(typeof(TowerType)))
        {
            int lv = towerUpgradeManager.GetLevel(t);
            int cost = towerUpgradeManager.GetUpgradeCost(t);
            bool isMax = IsMaxLevel(t);

            GameManager.instance.uiManager.SetTowerData(t, lv, cost, isMax, gold);
        }

        GameManager.instance.uiManager.InitUI(waveLevel, lifeCount, gold);
    }
    bool IsMaxLevel(TowerType type)
    {
        int lv = towerUpgradeManager.GetLevel(type);
        int max = 10;
        return lv >= max;
    }

    /// <summary>
    /// Wave
    /// </summary>
    /// <returns></returns>
    IEnumerator WaveLoop()
    {
        GameManager.instance.uiManager.StartNextRoundTimer(10f);
        yield return new WaitForSeconds(10f);
        while (true)
        {
            aliveMonster = 0;

            for (int i = 0; i < monsterCount; i++)
            {
                Monster monster = monsterSpawner.SpawnMonster(waveLevel);
                aliveMonster++;
                if (monster != null)
                {
                    monster.OnDead -= OnMonsterDead;
                    monster.OnDead += OnMonsterDead;

                    monster.OnReachedGoal -= OnMonsterReachedGoal;
                    monster.OnReachedGoal += OnMonsterReachedGoal;
                }
                yield return new WaitForSeconds(monsterSpawnInterval);
            }

            while (aliveMonster > 0)
                yield return null;

            GameManager.instance.uiManager.StartNextRoundTimer(nextWaveTimer);
            yield return new WaitForSeconds(nextWaveTimer);

            int prevLevel = waveLevel;
            waveLevel++;

            monsterFactory.ReleaseLevelPool(prevLevel);
            RefreshAllUpgradeUI();

        }

        void OnMonsterDead(Monster monster)
        {
            aliveMonster--;
        }
        void OnMonsterReachedGoal(Monster monster)
        {
            aliveMonster--;
            lifeCount--;

            RefreshAllUpgradeUI();
        }
    }
    void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        
    }
}

