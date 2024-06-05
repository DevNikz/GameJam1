using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    [Header("Object")]
    [SerializeField] public GameObject Daikon;

    [Header("UI")]
    [SerializeField] public GameObject UI;

    [Header("Score")]
    [SerializeField] public GameObject canvasObject;
    [SerializeReference] public List<GameObject> canvasList;

    [Header("Particle")]
    [SerializeField] public ParticleSystem ground;
    [SerializeField] public ParticleSystem pop;

    [Header("Input")]

    [SerializeField] public bool inputPress;
    [SerializeField] public int meterValue;

    public const string INPUT_PRESS = "INPUT_PRESS";

    [Header("Settings")]
    [SerializeField] public StartState startState = StartState.Yes;
    [SerializeField] public GameState gameState = GameState.Play; 
    [SerializeField] public LevelState levelState = LevelState.Playable;
    [SerializeField] public VFXState vfxState = VFXState.Paused;

    private void Start() {
        if(SceneManager.GetActiveScene().buildIndex == 0) LoadLevelOne();
        DetectRun();
    }

    private void LoadLevelOne() {
        //Init Vars
        gameState = GameState.Play;
        levelState = LevelState.Playable;
        meterValue = 0;

        //Init Daikon
        Daikon = transform.Find("Daikon").gameObject;

        //Set Daikon Property
        Daikon.SetActive(true);
        Rigidbody rb = transform.Find("Daikon").GetComponentInChildren<Rigidbody>();
        //Rigidbody rb = Daikon.GetComponent<Rigidbody>();
        rb.useGravity = false;
        Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, -1.25f, Daikon.transform.localPosition.z);

        //Init VFX
        ground = this.transform.Find("GroundDust").GetComponentInChildren<ParticleSystem>();
        pop = this.transform.Find("Pop Particle").GetComponentInChildren<ParticleSystem>();

        //Particle Properties
        ground.Stop();
        pop.Clear();
        pop.Stop();

        //Init UI
        //Input UI
        UI = transform.Find("INPUT UI").gameObject;

        //Score
        canvasObject = transform.Find("ANIMATE UI").gameObject;
        canvasObject.GetComponent<CanvasGroup>().alpha = 0;

        //AddObservers
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputPress);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.FirstRun);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
    }

    private void DetectRun() {
        startState = GameTimeManager.Instance.startState;
        gameState = GameTimeManager.Instance.gameState;

        if(startState == StartState.Yes) this.UI.SetActive(true);
        else this.UI.SetActive(false);

        if(gameState == GameState.End) Debug.Log("Game Ended");
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
    
        if(inputPress) {
            EnableAnim();
        }

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
            GameTimeManager.Instance.timerState = TimerState.Paused;

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
            vfxState = VFXState.Playing;
            Broadcaster.Instance.AddVFXState(CameraShake.CAMERA_SHAKE, EventNames.Scene1.CAMERA_SHAKE, vfxState);

            //Update Object
            if(meterValue == 25) Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, -1f, Daikon.transform.localPosition.z);
            else if(meterValue == 50) Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, -0.75f, Daikon.transform.localPosition.z);
            else if(meterValue == 75) Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, -0.5f, Daikon.transform.localPosition.z);

            //Update Meter
            meterValue += 5;

            Broadcaster.Instance.AddIntParam(UIController.INCREASE_METER, EventNames.Scene1.INCREASE_METER, meterValue);
        }
    }

    private void DelayedForce() {
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
        SceneController.Instance.LoadScene();

        //Resume Func
        GameTimeManager.Instance.timerState = TimerState.Playing;
    }


    //Animation Stuffs
    //Score Animation
    private void EnableAnim() {
        GameObject tempCanvas = Instantiate(canvasObject, canvasObject.transform.position, Quaternion.identity);
        canvasList.Add(tempCanvas);

        tempCanvas.GetComponent<CanvasGroup>().alpha = 0;
        
        LeanTween.alphaCanvas(tempCanvas.GetComponent<CanvasGroup>(), 1, 0.35f);
        LeanTween.moveLocalY(tempCanvas.GetComponentInChildren<RectTransform>().gameObject, 2f, 0.25f).setOnComplete(FadeOut);
    }

    private void FadeOut() {
        canvasList[0].GetComponent<CanvasGroup>().alpha = 0;
        LeanTween.alphaCanvas(canvasList[0].GetComponent<CanvasGroup>(), 0, 0.5f).delay = 0.2f;

        Destroy(canvasList[0]);
        canvasList.Remove(canvasList[0]);
    }
}
