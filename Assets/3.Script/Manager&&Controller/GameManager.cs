using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    

    public StageController stageController;
    public UIManager uiManager;
    public MonsterFactory monsterFactory;
    public TowerFactory towerFactory;
    public DamageTextFactory damageTextFactory;
    public ProjectileFactory projectileFactory;
    public SoundManager soundManager;
    private void OnEnable()
    {
        Time.timeScale = 1f;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReStartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
