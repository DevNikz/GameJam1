using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public static RunManager Instance;

    [Header("Game Settings")]

    [Tooltip("First Running The Game?")]
    [SerializeField] public bool isFirstRun;

    public const string CHANGE_RUN = "CHANGE_RUN";

    private Parameters tempParam;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    private void Start() {
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.CHANGE_RUN, this.DetectRun);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.CHANGE_RUN);
    }

    private void DetectRun(Parameters parameters) {
        this.isFirstRun = parameters.GetBoolExtra(CHANGE_RUN, true);

        if(isFirstRun) Debug.Log("Run: First Run!");
        else {
            Debug.Log("Run: Game is Ongoing.");
            this.isFirstRun = false;
        }
    }
}
