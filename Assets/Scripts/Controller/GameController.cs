using System;
using FSM;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController>
{
    public enum GameStateType
    {
        GameInit,
        GameRunning,
        GamePaused,
        GameSucceed,
        GameFailure
    }

    [SerializeField]
    private int gold = 400;

    [HideInInspector]
    public int intrudeCount;

    public int intrudeTop = 10;

    [HideInInspector]
    public int killedCount;

    [SerializeField]
    private int enemyTotal;

    public event Action OnGameStart;

    private EnemySpawner _spawner;

    private FSM<GameStateType> _fsm;

    public ISceneChangeEffect sceneChangeEffect;

    public event Action onFadeInSceneComplete;

    private void Start()
    {
        _spawner = FindObjectOfType<EnemySpawner>();
        enemyTotal = _spawner.EnemyTotal;

        sceneChangeEffect = Camera.main.GetComponent<ISceneChangeEffect>();

        _fsm = new FSM<GameStateType>(this);

        _fsm.AddState(new GameInit(_fsm));
        _fsm.AddState(new GameRunning(_fsm));
        _fsm.AddState(new GamePaused(_fsm));
        _fsm.AddState(new GameFailure(_fsm));
        _fsm.AddState(new GameSucceed(_fsm));

        UIManager.Instance.GetWindow(UIWindowID.GamePaused).transform.FindChildComponentByName<Button>("Btn_Back")
            .onClick.AddListener(
                () =>
                {
                    _fsm.ChangeState(GameStateType.GameRunning);
                });

        if (sceneChangeEffect != null)
        {
            sceneChangeEffect.Run(false, () =>
            {
                if (onFadeInSceneComplete != null) onFadeInSceneComplete();
            });
        }
        else
        {
            if (onFadeInSceneComplete != null) onFadeInSceneComplete();
        }
    }

    private void Update()
    {
        _fsm.Update();
    }

    public void ChangeState(GameStateType newStateType)
    {
        _fsm.ChangeState(newStateType);
    }

    public void AddIntrude()
    {
        intrudeCount += 1;
    }

    public void AddKilled()
    {
        killedCount += 1;
    }

    private bool CheckDefeat()
    {
        return intrudeCount >= intrudeTop;
    }

    private bool CheckVictory()
    {
        return intrudeCount < intrudeTop && killedCount + intrudeCount == enemyTotal;
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
        GameManager.Instance.QuitGame();
    }

    public void TriggerGameStart()
    {
        if (OnGameStart != null)
            OnGameStart();
    }

    public void GamePause()
    {
        if (_fsm.CurrentState == GameStateType.GamePaused)
            _fsm.ChangeState(GameStateType.GameRunning);
        else
            _fsm.ChangeState(GameStateType.GamePaused);
    }

    public class GameInit : FSM<GameStateType>.State
    {
        public GameInit(FSM<GameStateType> fsm) : base(fsm, GameStateType.GameInit)
        {
        }

        public override void Enter()
        {
            base.Enter();
           
            UIManager.Instance.FindWindowInScene();
            UIManager.Instance.OpenGloablWindow(UIWindowID.GameMain);
        }
    }

    public class GameRunning : FSM<GameStateType>.State
    {
        private bool _first = true;

        public GameRunning(FSM<GameStateType> fsm) : base(fsm, GameStateType.GameRunning)
        {
        }

        public override void Enter()
        {
            base.Enter();

            if (_first)
            {
                _first = false;
                Instance.TriggerGameStart();
            }
        }

        public override void Update()
        {
            base.Update();

            if (Instance.CheckVictory())
            {
                _fsm.ChangeState(GameStateType.GameSucceed);
            }     
            else if (Instance.CheckDefeat())
            {
                _fsm.ChangeState(GameStateType.GameFailure);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _fsm.ChangeState(GameStateType.GamePaused);
            }
        }
    }

    public class GamePaused : FSM<GameStateType>.State
    {
        public GamePaused(FSM<GameStateType> fsm) : base(fsm, GameStateType.GamePaused)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Time.timeScale = 0f;

            UIManager.Instance.OpenWindow(UIWindowID.GamePaused);
        }

        public override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _fsm.ChangeState(GameStateType.GameRunning);
                UIManager.Instance.BackToLastWindow();
            }
        }

        public override void Exit()
        {
            base.Exit();

            Time.timeScale = 1f;
        }
    }

    public class GameSucceed : FSM<GameStateType>.State
    {
        public GameSucceed(FSM<GameStateType> fsm) : base(fsm, GameStateType.GameSucceed)
        {
        }

        public override void Enter()
        {
            base.Enter();

            UIManager.Instance.OpenWindow(UIWindowID.GameSucceed);
        }
    }

    public class GameFailure : FSM<GameStateType>.State
    {
        public GameFailure(FSM<GameStateType> fsm) : base(fsm, GameStateType.GameFailure)
        {
        }

        public override void Enter()
        {
            base.Enter();

            UIManager.Instance.OpenWindow(UIWindowID.GameFailure);
        }
    }
}
