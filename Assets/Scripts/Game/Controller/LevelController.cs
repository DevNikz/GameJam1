using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    [Header("Object")]
    [SerializeField] public GameObject Daikon;

    [Header("UI")]
    [SerializeField] public GameObject UI;

    [Header("Particle")]
    [SerializeField] public ParticleSystem ground;
    [SerializeField] public ParticleSystem pop;

    [Header("Input")]

    [SerializeField] public bool inputPress;
    [SerializeField] public int meterValue;

    public const string INPUT_PRESS = "INPUT_PRESS";

    private Parameters parameters;

    [Header("Settings")]

    [SerializeField] public StartState startState = StartState.Yes;
    [SerializeField] public GameState gameState = GameState.Play; 
    [SerializeField] public LevelState levelState = LevelState.Playable;

    private void Start() {
        Debug.Log("Init Start");
        InitStart();
    }

    private void OnDestroy() {
        RemoveObservers();
    }

    private void InitStart() {
        InitObjects();
        InitVars();
        InitDaikon();
        DetectRun();
        AddObservers();
    }

    private void InitObjects() {
        ground.Stop();
        pop.Clear();
        pop.Stop();
    } 

    private void InitVars() {
        gameState = GameState.Play;
        levelState = LevelState.Playable;
    }

    private void InitDaikon() {
        Daikon.SetActive(true);
        Rigidbody rb = Daikon.GetComponent<Rigidbody>();
        rb.useGravity = false;
        Daikon.transform.localPosition = new Vector3(0f, -2.25f, 1.75f);
    }

    private void DetectRun() {
        startState = GameTimeManager.Instance.startState;
        gameState = GameTimeManager.Instance.gameState;

        if(startState == StartState.Yes) this.UI.SetActive(true);
        else this.UI.SetActive(false);

        if(gameState == GameState.End) Debug.Log("Game Ended");
    }

    private void AddObservers() {
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputPress);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.FirstRun);
    }

    private void RemoveObservers() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
    }

    private void FirstRun(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(INPUT_PRESS, false);
        if(startState == StartState.Yes) {
            if(inputPress) {
                //Set State to no
                startState = StartState.No;

                //Disable UI then play Particle
                this.UI.SetActive(false);
                this.pop.Play();

                //Change First Run to false
                Broadcaster.Instance.AddStartState(GameTimeManager.CHANGE_RUN, EventNames.Scene1.CHANGE_RUN, startState);
            }
        }
    }
    
    private void InputPress(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(INPUT_PRESS, false);
        StateHandler();
        PlayParticle();
    }

    private void PlayParticle() {
        if(inputPress) ground.Play();
        else ground.Stop();
    }

    private void StateHandler() {
        if(meterValue >= 100) {
            //Pause Func
            Broadcaster.Instance.AddTimerState(GameTimeManager.PAUSE_TIMER, 
                                                EventNames.Scene1.PAUSE_TIMER, TimerState.Paused);

            //Init Level as Unplayable
            levelState = LevelState.Unplayable;

            // Disable SFX
            Broadcaster.Instance.AddSFXState(SFXController.DISABLE_SFX, 
                                                EventNames.Scene1.DISABLE_SFX, SFXState.Paused);

            //Launch Object
            Invoke(nameof(DelayedForce), 0.025f);

            //Change Scene
            Invoke(nameof(ChangeScene), 2.5f);
        }

        if(inputPress && levelState == LevelState.Playable) {
            Rigidbody rb = Daikon.GetComponent<Rigidbody>();

            PlayerData.Score += 10;

            //Update VFX
            Parameters tempParam = new Parameters();
            tempParam.PutExtra(CameraShake.CAMERA_SHAKE, true);
            EventBroadcaster.Instance.PostEvent(EventNames.Scene1.CAMERA_SHAKE, tempParam);

            //Update Object
            if(meterValue == 25) Daikon.transform.localPosition = new Vector3(0f, -2f, 1.75f);
            else if(meterValue == 50) Daikon.transform.localPosition = new Vector3(0f, -1.75f, 1.75f);
            else if(meterValue == 75) Daikon.transform.localPosition = new Vector3(0f, -1.5f, 1.75f);

            //Update Meter
            meterValue += 5;

            parameters = new Parameters();
            parameters.PutExtra(UIController.INCREASE_METER, meterValue);
            parameters.PutExtra(UIController.UI_NAME, "Level [Meter]");

            EventBroadcaster.Instance.PostEvent(EventNames.Scene1.INCREASE_METER, parameters);
        }
    }

    private void DelayedForce() {
        //Debug.Log("Add Force");
        Rigidbody rb = Daikon.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        float throwSpeed = 6f;
        float launchAngle = -2.5f;
        Quaternion rotationSpeed;
        
        Quaternion rotation = Quaternion.Euler( launchAngle, 0, 0);

        rotationSpeed = Quaternion.Euler(10f,10f,10f);
        
        Vector3 velocity = rotation * (Vector3.up * throwSpeed) + (Vector3.forward * 2.5f);
        
        rb.velocity = velocity;
        rb.rotation = rotationSpeed;
    }

    private void ChangeScene() {

        //ChangeScene
        Broadcaster.Instance.AddStringParam(SceneController.SCENE_NAME, 
                                            EventNames.SceneChange.CHANGE_SCENE ,SceneManager.GetActiveScene().name);

        //Resume Func
        Broadcaster.Instance.AddTimerState(GameTimeManager.PAUSE_TIMER, 
                                            EventNames.Scene1.PAUSE_TIMER, TimerState.Playing);
    }
}
