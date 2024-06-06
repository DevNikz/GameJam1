using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    //Game
    [Header("States")]
    [SerializeField] public StartState startState = StartState.Yes;
    [SerializeField] public GameState gameState = GameState.Play; 
    [SerializeField] public LevelState levelState = LevelState.Playable;
    [SerializeField] public VFXState vfxState = VFXState.Paused;

    //Input
    [Header("Input")]
    [SerializeField] public bool inputPress;

    //Scene 1 Specific
    [Header("Scene 1")]
    [Range(0, 100)]
    [SerializeField] public int meterValue;
    [SerializeReference] public GameObject meterUI;
    [SerializeField] public GameObject Daikon;

    //UI
    [Header("INPUT UI")]
    [SerializeField] public GameObject inputUI;
    [SerializeField] public TextMeshProUGUI inputText;

    [Header("PLAY UI")]
    [SerializeField] public GameObject playHUD;

    [Header("PAUSE UI")]
    [SerializeField] public GameObject pauseHUD;
    [SerializeField] public GameObject pauseText;

    [Header("END UI")]
    [SerializeField] public GameObject endHUD; //EndHUD
    [SerializeField] public GameObject endScreen; //EndScreen

    //PostProcess
    [Header("PostProcess")]
    [SerializeField] public GameObject pauseProfile; //For Pause and End Screen

    //Score
    [Header("Score")]
    [SerializeField] public GameObject canvasObject;
    [SerializeReference] public List<GameObject> canvasList;

    //Effects
    [Header("Particle")]
    [SerializeField] public ParticleSystem ground;
    [SerializeField] public ParticleSystem pop;

    //Animation
    [Header("Input Animation")]
    [SerializeField] Color startColor = new Color(1,1,1,1);
    [SerializeField] Color endColor = new Color(1,1,1,0);
    [Range(0,10)]
    [SerializeField] public float speed = 2f;

    //EventBroadcaster
    public const string INPUT_PRESS = "INPUT_PRESS";

    private void Start() {
        //Load Initial Level / Level One
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1) LoadLevelOne();

        //Load Game UI
        LoadGameUI();

        //Load Game Views
        LoadGameViews();

        //Detect Current Run
        DetectRun();
    }

    private void FixedUpdate() {
        //Update Meter Dynamically
        Broadcaster.Instance.AddIntParam(UIController.INCREASE_METER, EventNames.Scene1.INCREASE_METER, meterValue);

        //Update VFX
        Broadcaster.Instance.AddVFXState(CameraShake.CAMERA_SHAKE, EventNames.Scene1.CAMERA_SHAKE, vfxState);
    }

    private void LoadGameUI() {
        //UI
        meterUI = transform.Find("METER UI").gameObject;
        pauseText = transform.Find("GAME UI/Pause").gameObject;

        //Overlay
        playHUD = transform.Find("GAME UI/PlayHUD").gameObject;
        pauseHUD = transform.Find("GAME UI/PauseHUD").gameObject;
        endHUD = transform.Find("GAME UI/EndHUD").gameObject;

        //Screen
        endScreen = transform.Find("View/Game/EndScreen").gameObject;

        //Set Overlay
        playHUD.SetActive(true);
        pauseHUD.SetActive(false);
        endHUD.SetActive(false);

        //Set Screen
        endScreen.SetActive(false);

        //Set Elements
        meterUI.SetActive(true);
        pauseText.SetActive(false);
    }

    private void LoadGameViews() {
        //PostProcess
        pauseProfile = transform.Find("View/Game/PauseProfile").gameObject;
        pauseProfile.SetActive(false);
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
        rb.useGravity = false;
        Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, -1.25f, Daikon.transform.localPosition.z);

        //Init VFX
        ground = this.transform.Find("GroundDust").GetComponentInChildren<ParticleSystem>();
        pop = this.transform.Find("Pop Particle").GetComponentInChildren<ParticleSystem>();

        //Particle Properties
        ground.Stop();
        pop.Clear();
        pop.Stop();

        //Input UI
        inputUI = transform.Find("INPUT UI").gameObject;
        inputText = inputUI.transform.Find("Input").GetComponent<TextMeshProUGUI>();

        //Score
        canvasObject = transform.Find("ANIMATE UI").gameObject;
        canvasObject.GetComponent<CanvasGroup>().alpha = 0;

        //AddObservers
        if(SceneManager.GetActiveScene().buildIndex == 0) EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.FirstRun); //Initial Level / Tutorial
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputPress);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
    }

    private void DetectRun() {
        startState = GameTimeManager.Instance.startState;

        if(startState == StartState.Yes) {
            this.inputUI.SetActive(true);
        }
        else {
            this.inputUI.SetActive(false);
            DestroyInputUI();
            
        }
    }

    private void FirstRun(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(INPUT_PRESS, false);
        if(startState == StartState.Yes) {
            BlinkAnim();
            if(inputPress) {
                //Set State to no
                startState = StartState.No;

                //Disable UI then play Particle
                this.inputUI.SetActive(false);
                DestroyInputUI();

                //Play SFX
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

    //Particle Player
    private void PlayParticle() {
        if(inputPress) ground.Play();
        else ground.Stop();
    }


    //State Handler
    private void StateHandler() {
        gameState = GameTimeManager.Instance.gameState;
        if(gameState == GameState.Play) {
            PlayGame();
        }
        else {
            Time.timeScale = 0;
            levelState = LevelState.Unplayable;

            //Pause Timer
            GameTimeManager.Instance.timerState = TimerState.Paused;
            MusicController.Instance.gameState = GameState.End;
            
            //Remove Observer
            EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);

            //Stop Ground vfx
            ground.Stop();

            // Disable SFX
            Broadcaster.Instance.AddSFXState(SFXController.DISABLE_SFX, 
                                                EventNames.Scene1.DISABLE_SFX, SFXState.Paused);

            //Hide Meter
            HideMeter();

            //Enable Pause Camera
            EnableEndCamera();
        }
    }

    private void PlayGame() {
        if(meterValue >= 100) {
            //GAME PAUSED
            HideMeter();
            EnablePauseCamera();

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

            //Enable Camera Shake when Input
            vfxState = VFXState.Playing;
            

            //Update Object
            if(meterValue == 25) UpdateDaikon(-1f);
            else if(meterValue == 50) UpdateDaikon(-0.75f);
            else if(meterValue == 75) UpdateDaikon(-0.5f);

            //Update Meter
            meterValue += 5;
        }
    }

    //HideStuffs
    private void HideMeter() {
        meterUI.SetActive(false);
    }

    private void EnablePauseCamera() {
        //PostProcess
        pauseProfile.SetActive(true);

        //HUD
        playHUD.SetActive(false);
        pauseHUD.SetActive(true);

        //Elements
        pauseText.SetActive(true);
    }

    private void EnableEndCamera() {
        //PostProcess
        pauseProfile.SetActive(true);

        //HUD
        playHUD.SetActive(false);
        pauseHUD.SetActive(false);
        endHUD.SetActive(true);

        //Elements
        endScreen.SetActive(true);
    }

    //Daikon Controller
    private void UpdateDaikon(float value) {
        Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, value, Daikon.transform.localPosition.z);
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

    //Scene Changer
    private void ChangeScene() {

        //Change Next Scene
        PlayerData.currentScene = 0;
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

    private void BlinkAnim() {
        inputText.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * speed, 1));
    }

    private void DestroyInputUI() {
        Destroy(inputUI);
    }
}
