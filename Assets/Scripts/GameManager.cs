using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour, IDependentScript
{
    #region Singleton
    public static GameManager Instance;
    //reference this only version as MultiplierManager.Instance.SpawnSprite(randPos, UnityEngine.Random.rotation);
    private void Awake() {
        Instance = this;
    }
    #endregion

    #region GameState
    public enum GameState {
        GAMEOVER,
        PAUSED,
        PLAYING,
        STARTING,
    }
    private GameState _currGameState;
    public GameState CurrGameState
    {
        get {
            return this._currGameState;
        }

        set {  
            GameState prevGameState = this._currGameState;
            this._currGameState = value;   
            if (prevGameState != value)   
                OnGameStateChange?.Invoke(prevGameState, value);
        }
    }
    public bool IsPlaying {
        get {
            return this.CurrGameState == GameState.PLAYING;
        }
    }
    public bool IsGameOver {
        get {
            return this.CurrGameState == GameState.GAMEOVER;
        }
        set {
            if (value)
                this.CurrGameState = GameState.GAMEOVER;
        }
    }
    public event Action<GameState, GameState> OnGameStateChange;
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGamePause;
    public event Action OnGameResume;
    #endregion
 
    // Start is called before the first frame update
    void Start()
    {
        OnGameStateChange += OnGameStateChangeHandler;
        OnGameStart += OnGameStartHandler;
        OnGameOver += OnGameOverHandler;

        AddDependencies();
    }

    #region IDependentScript
    protected virtual void AddDependencies() {
        List<ILoadableScript> dependencies = new List<ILoadableScript>();
        dependencies.Add(ObjectPooler.Instance);
        dependencies.Add(TilemapInitialization.Instance);
        ScriptDependencyManager.Instance.UpdateDependencyDicts(this, dependencies);
    }

    public void OnAllDependenciesLoaded() {
        StartGame();
    }
    #endregion

    #region OnGameStateEventHandlers
    void OnGameStateChangeHandler(GameState prevGameState, GameState newGameState) {
        if (newGameState == GameState.STARTING) {
            OnGameStart?.Invoke();
        } else if (newGameState == GameState.GAMEOVER) {
            OnGameOver?.Invoke();
        } else if (newGameState == GameState.PAUSED) {
            OnGamePause?.Invoke();
        } else if (newGameState == GameState.PLAYING) {
            if (prevGameState == GameState.PAUSED) {
                OnGameResume?.Invoke();
            }
        }
    }

    void OnGameStartHandler() {
        CurrGameState = GameState.PLAYING;
    }

    void OnGameOverHandler() {
        
    }
    #endregion

    public void StartGame() {
        TilemapInitialization.Instance.InitNewTilemap();
        CurrGameState = GameState.STARTING;
    }

    public void RestartGame() {
        CurrGameState = GameState.GAMEOVER;
        StartGame();
    }

}