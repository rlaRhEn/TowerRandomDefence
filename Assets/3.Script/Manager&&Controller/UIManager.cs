using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("TopPanel")]
    public Text round; //현재 라운드
    public Text nextRoundTimer; //다음 웨이브 시작 타이머
    public Text currentLife;
    public Slider life; // 라이프
    

    [Space]

    [Header("BottomPanel")]
    public GameObject upgradeViewr;
    public Text gold; //골드

    [Space]

    [Header("UpgraderText")]
    public Text archerLevel;
    public Text knightLevel;
    public Text mageLevel;
    public Text archerUpgradeCost;
    public Text knightUpgradeCost;
    public Text mageUpgradeCost;
    public Text GotchaCost;

    [Space]
    [Header("Btn")]
    [SerializeField] Button archerUpgradeBtn;
    [SerializeField] Button knightUpgradeBtn;
    [SerializeField] Button mageUpgradeBtn;

    Animator ani;

    public Dictionary<TowerType, Text> towerLv;
    public Dictionary<TowerType, Text> towerUpgradeCost;
    

    private void Awake()
    {
        ani = upgradeViewr.GetComponent<Animator>();
        towerLv = new Dictionary<TowerType, Text>()
        { 
            { TowerType.Knight, knightLevel},
            { TowerType.Archer, archerLevel},
            { TowerType.Mage, mageLevel}
        };

        towerUpgradeCost = new Dictionary<TowerType, Text>()
        {
            { TowerType.Knight, knightUpgradeCost},
            { TowerType.Archer, archerUpgradeCost},
            { TowerType.Mage, mageUpgradeCost}
        };
    }
    public IEnumerator UpdateNextRoundTimer(float time)
    {
        float currentTime = time;

        while (currentTime > 0)
        {
            int seconds = Mathf.CeilToInt(currentTime);

            nextRoundTimer.text = $"00 : {seconds:00}";

            yield return new WaitForSeconds(1f);
            currentTime -= 1f;
        }

        nextRoundTimer.text = "00 : 00";
    }
    public void StartNextRoundTimer(float time)
    {
        StartCoroutine(UpdateNextRoundTimer(time));
    }
    public void SetGold(int value) //골드
    {
        gold.text = value.ToString();
    }
    public void SetRound(int wave) //라운드
    {
        round.text = $"WAVE {wave}";
    }
    public void SetLife(int current ,int life = 20) //라이프
    {
        this.life.maxValue = life;
        this.life.value = current;
        currentLife.text = $"{current} / {life}"; 
    }
    public void SetRUpgradePanel()
    {
        bool cur = ani.GetBool("IsMove");
        ani.SetBool("IsMove", !cur);
        
    }
    public void SetTowerData(TowerType type, int level, int cost,bool isMax, int currentGold)
    {
       
        towerLv[type].text = isMax? "MAX"  : $"Lv.{level}";

        towerUpgradeCost[type].gameObject.SetActive(!isMax);
        if (!isMax)
            towerUpgradeCost[type].text = $"{cost}";

        towerUpgradeCost[type].color = (currentGold >= cost) ? Color.black : Color.red;
        GetUpgradeButton(type).interactable = !isMax;


    }
    Button GetUpgradeButton(TowerType type)
    {
        switch(type)
        {
            case TowerType.Archer: return archerUpgradeBtn;
            case TowerType.Knight: return knightUpgradeBtn;
            case TowerType.Mage: return mageUpgradeBtn;
        }
        return null;
    }
    public void InitUI(int waveLevel, int life, int gold)
    {
        SetRound(waveLevel);
        SetLife(life);
        SetGold(gold);
    }

}
