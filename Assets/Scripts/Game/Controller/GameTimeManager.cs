using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;

    [Header("Game Settings")]

    [Tooltip("First Running The Game?")]
    [SerializeField] public StartState startState = StartState.Yes;

    [Tooltip("Current Game State")]
    [SerializeField] public GameState gameState = GameState.Play;

    [Header("Timer Settings")]
    
    [Tooltip("Set Timer Throughout The Game")]
    [SerializeField] public float timer = 30f;

    [Tooltip("Is Timer paused?")]
    [SerializeField] public TimerState timerState = TimerState.Playing;

    [Header("Score")] 
    [SerializeField] public int Score;

    public const string CHANGE_RUN = "CHANGE_RUN";

    public const string PAUSE_TIMER = "PAUSE_TIMER";

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    private void Start() {
        InitStart();
    }

    private void OnDestroy() {
        RemoveObservers();
    }

    private void InitStart() {
        InitVars();
        AddObservers();
    }

    private void InitVars() {
        startState = StartState.Yes;
        gameState = GameState.Play;
    }

    private void AddObservers() {
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.CHANGE_RUN, this.DetectRun);
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.PAUSE_TIMER, this.DetectTimer);
    }

    private void RemoveObservers() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.CHANGE_RUN);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.PAUSE_TIMER);
    }

    private void Update() {
        Score = PlayerData.Score;

        //Debug
        Debug.Log((int)PlayerData.Timer);

        if(timer > 0 && timerState == TimerState.Playing) {
            timer -= Time.deltaTime;
            PlayerData.Timer = timer;
            if(timer <= 0) {
                gameState = GameState.End;
            }
        }
    }

    private void DetectRun(Parameters parameters) {
        startState = parameters.GetStartState(CHANGE_RUN, StartState.Yes);

        if(startState == StartState.Yes) Debug.Log("Run: First Run!");
        else {
            startState = StartState.No;
        }
    }

    private void DetectTimer(Parameters parameters) {
        timerState = parameters.GetTimerState(PAUSE_TIMER, TimerState.Playing);
    }
}
