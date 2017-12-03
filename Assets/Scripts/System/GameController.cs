using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoSingleton<GameController>
{
    [SerializeField]
    private int gold = 400;

    [HideInInspector]
    public int intrudeCount;

    public int intrudeTop = 10;

    [HideInInspector]
    public int killedCount;

    private int enemyTotal;

    public event Action<bool> OnGamePaused;

    private bool _gamePaused;

    private EnemySpawner _spawner;

    private void Start()
    {
        ChangeGold(0);

        _spawner = FindObjectOfType<EnemySpawner>();

        enemyTotal = _spawner.enemyTotal; 
    }

    public void AddIntrude()
    {
        intrudeCount += 1;

        CheckDefeat();
  
        CheckVictory();
    }

    private void CheckDefeat()
    {
        if (intrudeCount >= intrudeTop)
        {
            SceneManager.LoadScene("Defeat"); 
        }
    }

    private void CheckVictory()
    {
        if (intrudeCount < intrudeTop && killedCount + intrudeCount == enemyTotal)
        {   
            SceneManager.LoadScene("Victory");
        }
    }


    public void AddKilled()
    {
        killedCount += 1;
        CheckVictory();
    }

    public void ChangeGold(int amount)
    {
        gold += amount;
    }

    public int GetGold()
    {
        return gold;
    }

    public void GameRestart()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }

    public void ShutDown()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GamePause()
    {
        _gamePaused = !_gamePaused;

        if (OnGamePaused != null)
            OnGamePaused(_gamePaused);

        Time.timeScale = _gamePaused ? 0 : 1;
    }
}
