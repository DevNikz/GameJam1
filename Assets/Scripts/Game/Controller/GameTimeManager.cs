using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;

    [Header("Game Settings")]

    [Tooltip("First Running The Game?")]
    [SerializeField] public bool isFirstRun = true;

    [Tooltip("Game End?")]
    [SerializeField] public bool isGameEnd = false;

    [Header("Timer Settings")]
    
    [Tooltip("Set Timer Throughout The Game")]
    [SerializeField] public float timer = 30f;

    [Tooltip("Is Timer paused?")]
    [SerializeField] public bool pause = false;


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
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.CHANGE_RUN, this.DetectRun);
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.PAUSE_TIMER, this.DetectTimer);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.CHANGE_RUN);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.PAUSE_TIMER);
    }

    private void Update() {
        this.Score = PlayerData.Score;

        if(this.timer > 0 && !pause) {
            timer -= Time.deltaTime;
            if(this.timer <= 0) {
                this.isGameEnd = true;
            }
        }
    }

    private void DetectRun(Parameters parameters) {
        this.isFirstRun = parameters.GetBoolExtra(CHANGE_RUN, true);

        if(isFirstRun) Debug.Log("Run: First Run!");
        else {
            this.isFirstRun = false;
        }
    }

    private void DetectTimer(Parameters parameters) {
        this.pause = parameters.GetBoolExtra(PAUSE_TIMER, false);
    }
}
