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
    [SerializeField] public StartState startState2 = StartState.Yes;

    [Tooltip("Current Game State")]
    [SerializeField] public GameState gameState = GameState.Play;

    [Header("Timer Settings")]
    
    [Tooltip("Set Timer Throughout The Game")]
    [SerializeField] public float timer = 120f;

    [Tooltip("Is Timer paused?")]
    [SerializeField] public TimerState timerState = TimerState.Playing;

    [Header("Score")] 
    [SerializeField] public int Score;

    public const string CHANGE_RUN = "CHANGE_RUN";
    public const string CHANGE_RUN2 = "CHANGE_RUN2";
    public const string PAUSE_TIMER = "PAUSE_TIMER";

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start() {
        startState = StartState.Yes;
        startState2 = StartState.Yes;
        gameState = GameState.Play;
        timer = Random.Range(60f, 120f);

        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.CHANGE_RUN, this.DetectRun);
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.CHANGE_RUN2, this.DetectRun2);
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.PAUSE_TIMER, this.DetectTimer);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.CHANGE_RUN);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.CHANGE_RUN2);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.PAUSE_TIMER);
    }

    private void Update() {
        Score = PlayerData.Score;

        if(timer > 0 && timerState == TimerState.Playing) {
            timer -= Time.deltaTime;
            PlayerData.Timer = timer;
            if(timer <= 0) {
                timer = 0;
                gameState = GameState.End;
                timerState = TimerState.Paused;
            }
        }
    }

    private void DetectRun(Parameters parameters) {
        startState = parameters.GetStartState(CHANGE_RUN, StartState.Yes);
    }

    private void DetectRun2(Parameters parameters) {
        startState2 = parameters.GetStartState(CHANGE_RUN2, StartState.Yes);
    }

    private void DetectTimer(Parameters parameters) {
        timerState = parameters.GetTimerState(PAUSE_TIMER, TimerState.Playing);
    }
}
