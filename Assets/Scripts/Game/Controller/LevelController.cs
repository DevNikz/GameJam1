using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    //Game
    [Header("States")]
    [SerializeField] public StartState startState = StartState.Yes;
    [SerializeField] public StartState startState2 = StartState.Yes;
    [SerializeField] public GameState gameState = GameState.Play; 
    [SerializeField] public LevelState levelState = LevelState.Playable;
    [SerializeField] public VFXState vfxState = VFXState.Paused;

    //Input
    [Header("Input")]
    [SerializeField] public bool spacePress;
    [SerializeField] public bool spaceHold;
    [SerializeField] public bool spaceReleased;
    [SerializeField] public bool ePress;

    //Scene 1 Specific
    [Header("Scene 1")]
    [Range(0, 100)]
    [SerializeField] public int meterValue;
    private GameObject meterUI;
    private GameObject Daikon;

    [Header("Scene2")]
    [Range(0, 100)]
    [SerializeField] public int meterFill;
    public GameObject fillUI;
    public GameObject dropUI;
    private float dropPos;

    //Scene 3 Specific
    [Header("Scene 3")]
    [SerializeField] public int counter;
    [Range(0,100)]
    [SerializeField] public int meterValue_Vert;
    private Vector3 moveInput;
    private GameObject player;
    private Rigidbody rb;
    private GameObject interactCollider;
    private ParticleSystem playerDust;
    private GameObject meterUI_Vert;

    //UI
    private GameObject inputUI;
    private TextMeshProUGUI inputText;

    //PlayHUD
    private GameObject playHUD;

    //PauseHUD
    private GameObject pauseHUD;
    private GameObject pauseText;

    //EndHUD
    private GameObject endHUD;
    private GameObject endScreen;

    //PostProcess
    [Header("Filter")]
    [SerializeField] private bool enableHorror = false;
    private GameObject pauseProfile;
    private GameObject horrorProfile;
    private GameObject horrorPauseProfile;

    //Score
    [Header("Score")]
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject canvasObjectALT;
    [SerializeField] private List<GameObject> canvasList;

    //Effects
    private ParticleSystem ground;
    private ParticleSystem pop;
    private ParticleSystem water;

    //Animation
    private Color startColor = new Color(1,1,1,1);
    private Color endColor = new Color(1,1,1,0);
    [Range(0,10)]
    private float speed = 2f;

    //EventBroadcaster
    public const string INPUT_PRESS = "INPUT_PRESS";
    public const string INPUT_HOLD = "INPUT_HOLD";
    public const string INPUT_RELEASED = "INPUT_RELEASED";
    public const string INPUT_E = "INPUT_E";
    public const string KEY_MOVE = "KEY_MOVE";

    private void Start() {
        //Load Game UI
        LoadGameUI();

        //Load Game Views
        LoadGameViews();

        //Load Initial Level / Level One
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1) LoadLevelOne();
        else if(SceneManager.GetActiveScene().buildIndex == 2) LoadLevelTwo();
        else if(SceneManager.GetActiveScene().buildIndex == 3) LoadLevelThree();
    }

    private void FixedUpdate() {
        //Level One Specific
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1) {
            //Update Meter Dynamically
            Broadcaster.Instance.AddIntParam(UIController.INCREASE_METER, EventNames.Scene1.INCREASE_METER, meterValue);
        }

        //Level Two Specific
        else if(SceneManager.GetActiveScene().buildIndex == 2) {
            Broadcaster.Instance.AddIntParam(UIController.INCREASE_METER_VERT, EventNames.Scene1.INCREASE_METER_VERT, meterFill);
        }

        //Level Three Specific
        else if(SceneManager.GetActiveScene().buildIndex == 3) {
            if(levelState == LevelState.Playable) CheckDir();
            UpdateCounter();
            Broadcaster.Instance.AddIntParam(UIController.INCREASE_METER_VERT, EventNames.Scene1.INCREASE_METER_VERT, meterValue_Vert);
        }
    }

    private void LoadGameUI() {

        //General
        pauseText = transform.Find("GAME UI/Pause").gameObject;
        pauseText.SetActive(false);

        playHUD = transform.Find("GAME UI/PlayHUD").gameObject;
        playHUD.SetActive(true);

        pauseHUD = transform.Find("GAME UI/PauseHUD").gameObject;
        pauseHUD.SetActive(false);

        endHUD = transform.Find("GAME UI/EndHUD").gameObject;
        endHUD.SetActive(false);

        endScreen = transform.Find("View/Game/EndScreen").gameObject;
        endScreen.SetActive(false);

        //Score
        canvasObject = transform.Find("ANIMATE UI").gameObject;
        canvasObject.GetComponent<CanvasGroup>().alpha = 0;

        //Level 0 Specific
        if(SceneManager.GetActiveScene().buildIndex == 0) { 
            inputUI = transform.Find("INPUT UI").gameObject;
            inputText = inputUI.transform.Find("Input").GetComponent<TextMeshProUGUI>();
            meterUI = transform.Find("METER UI").gameObject;
            meterUI.SetActive(true);
        }

        //Level 0 and 1 Specific
        else if(SceneManager.GetActiveScene().buildIndex == 1) {
            meterUI = transform.Find("METER UI").gameObject;
            meterUI.SetActive(true);

            canvasObjectALT = transform.Find("ANIMATE UI (ALT)").gameObject;
            canvasObjectALT.GetComponent<CanvasGroup>().alpha = 0;
            canvasObjectALT.SetActive(false);
        }

        //Level 2 Specific
        else if(SceneManager.GetActiveScene().buildIndex == 2) { 
            inputUI = transform.Find("INPUT UI").gameObject;
            inputText = inputUI.transform.Find("Input").GetComponent<TextMeshProUGUI>();
            
            fillUI = transform.Find("QTE UI").gameObject;
            fillUI.SetActive(true);

            dropUI = fillUI.transform.Find("Drop").gameObject;
            dropUI.SetActive(true);

            canvasObjectALT = transform.Find("ANIMATE UI (ALT)").gameObject;
            canvasObjectALT.GetComponent<CanvasGroup>().alpha = 0;
            canvasObjectALT.SetActive(false);
        }

        //Level 3 Specific
        else if(SceneManager.GetActiveScene().buildIndex == 3) { 
            meterUI_Vert = transform.Find("METER UI (Vertical)").gameObject;
            meterUI_Vert.SetActive(true);

            canvasObjectALT = transform.Find("ANIMATE UI (ALT)").gameObject;
            canvasObjectALT.GetComponent<CanvasGroup>().alpha = 0;
            canvasObjectALT.SetActive(false);
        }
    }

    private void LoadGameViews() {
        //Vars
        enableHorror = false;

        //PostProcess
        pauseProfile = transform.Find("View/Game/PauseProfile").gameObject;
        pauseProfile.SetActive(false);
        
        horrorProfile = transform.Find("View/Game/DarkProfile").gameObject;
        horrorProfile.SetActive(false);

        horrorPauseProfile = transform.Find("View/Game/PauseProfile_Dark").gameObject;
        horrorPauseProfile.SetActive(false);
    }

    //Init Level One
    private void LoadLevelOne() {
        //Init Vars
        gameState = GameTimeManager.Instance.gameState;
        levelState = LevelState.Playable;
        meterValue = 0;

        //Init Daikon
        Daikon = transform.Find("Daikon").gameObject;

        //Set Daikon Property
        Daikon.SetActive(true);
        Rigidbody rb = transform.Find("Daikon").GetComponentInChildren<Rigidbody>();
        rb.useGravity = false;
        Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, -1.25f, Daikon.transform.localPosition.z);

        //Init Particle
        ground = this.transform.Find("GroundDust").GetComponentInChildren<ParticleSystem>();
        ground.Stop();

        //AddObservers
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputPress);

        //Level 0 Specific
        if(SceneManager.GetActiveScene().buildIndex == 0) {
            pop = this.transform.Find("Pop Particle").GetComponentInChildren<ParticleSystem>();
            pop.Clear();
            EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.FirstRun);
        }

        //Level 1 Specific
        else if(SceneManager.GetActiveScene().buildIndex == 1) InitPostProcess();
    }

    //Init Level Two
    private void LoadLevelTwo() {
        //Set Vars
        gameState = GameTimeManager.Instance.gameState;
        levelState = LevelState.Playable;
        meterFill = 0;

        //Init Particle
        water = this.transform.Find("Water").GetComponentInChildren<ParticleSystem>();
        water.Stop();

        //Level 2 Specific
        pop = this.transform.Find("Pop Particle").GetComponentInChildren<ParticleSystem>();
        pop.Clear();
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.SecondRun);

        //Init PostProcess
        InitPostProcess();

        //Add Observer
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputListen);
    }

    //Init Level Three
    private void LoadLevelThree() {
        //Set Vars
        gameState = GameTimeManager.Instance.gameState;
        levelState = LevelState.Playable;
        meterValue_Vert = 0;
        counter = 0;
        PlayerData.counterDaikon = counter;

        //Set Player
        player = this.transform.Find("Player").gameObject;
        rb = player.GetComponent<Rigidbody>();

        //Set Collider
        interactCollider = this.transform.Find("Player/Interact").gameObject;

        //Set Particle
        playerDust = this.transform.Find("Player/PlayerDust").gameObject.GetComponent<ParticleSystem>();
        playerDust.Clear();

        //Init PostProcess
        InitPostProcess();

        //Add Observer
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.MOVE_INPUT, this.MoveEvent);
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_E, this.InteractPile);
    }

    private void OnDestroy() {
        //Level 0 and Level 1
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);

        //Level 3
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_E);
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.MOVE_INPUT);
    }

    private void FirstRun(Parameters parameters) {
        spacePress = parameters.GetBoolExtra(INPUT_PRESS, false);
        if(startState == StartState.Yes) {
            BlinkAnim();
            if(spacePress) {
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

    private void SecondRun(Parameters parameters) {
        spacePress = parameters.GetBoolExtra(INPUT_PRESS, false);
        if(startState2 == StartState.Yes) {
            BlinkAnim();
            if(spacePress) {
                //Set State to no
                startState2 = StartState.No;

                //Disable UI then play Particle
                this.inputUI.SetActive(false);
                DestroyInputUI();

                //Play SFX
                this.pop.Play();

                //Change First Run to false
                Broadcaster.Instance.AddStartState(GameTimeManager.CHANGE_RUN2, EventNames.Scene1.CHANGE_RUN2, startState2);
            }
        }
    }
    
    //Level One
    private void InputPress(Parameters parameters) {
        spacePress = parameters.GetBoolExtra(INPUT_PRESS, false);
        if(spacePress) {
            EnableAnim();
        }
        StateHandler();
        PlayParticle();
    }

    //State Handler
    private void StateHandler() {
        gameState = GameTimeManager.Instance.gameState;
        if (gameState == GameState.Play) {
            if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1) PlayGame();
            EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.RESTART_GAME);
        }
        else {
            gameState = GameState.End;
            levelState = LevelState.Unplayable;
            
            //Remove Observer
            // EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
            // EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_E);

            if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1) {
                ground.Stop();
                Broadcaster.Instance.AddSFXState(SFXController.DISABLE_SFX, EventNames.Scene1.DISABLE_SFX, SFXState.Paused);
            }   

            //Hide Meter
            HideMeters();

            //Enable Pause Camera
            EnableEndCamera();

            EventBroadcaster.Instance.AddObserver(EventNames.Scene1.RESTART_GAME, this.RestartGame);
        }
        
    }

    private void RestartGame(Parameters parameters) {
        spacePress = parameters.GetBoolExtra(INPUT_PRESS, false);
        if(spacePress) {
            //Change Player Data
            PlayerData.Score = 0;
            PlayerData.Timer = UnityEngine.Random.Range(60f, 120f);

            //Change Local Data
            startState = StartState.Yes;
            startState2 = StartState.Yes;
            gameState = GameState.Play;

            //Change Manager Data
            GameTimeManager.Instance.gameState = GameState.Play;
            GameTimeManager.Instance.timerState = TimerState.Playing;
            GameTimeManager.Instance.timer = 60;

            Time.timeScale = 1;

            SceneController.Instance.RestartGame();
        }
    }

    private void PlayGame() {
        if(meterValue >= 100) {
            //GAME PAUSED
            HideMeters();
            EnablePauseCamera_Variation();

            //Pause Func
            GameTimeManager.Instance.timerState = TimerState.Paused;

            //Init Level as Unplayable
            levelState = LevelState.Unplayable;

            // Disable SFX
            Broadcaster.Instance.AddSFXState(SFXController.DISABLE_SFX, EventNames.Scene1.DISABLE_SFX, SFXState.Paused);

            //Launch Object
            Invoke(nameof(DelayedForce), 0.025f);

            //Change Scene
            Invoke(nameof(ChangeScene), 2.5f);
        }

        if(spacePress && levelState == LevelState.Playable) {
            Rigidbody rb = Daikon.GetComponent<Rigidbody>();

            if(enableHorror) PlayerData.Score += 20;

            else PlayerData.Score += 10;

            //Enable Camera Shake when Input
            vfxState = VFXState.Playing;

            //Update VFX
            Broadcaster.Instance.AddBoolParam(CameraShake.CAMERA_SHAKE, EventNames.Scene1.CAMERA_SHAKE, true);

            meterValue += 5;

            //Update Object
            if(meterValue == 25) UpdateDaikon(-1f);
            else if(meterValue == 50) UpdateDaikon(-0.75f);
            else if(meterValue == 75) UpdateDaikon(-0.5f);
        }
    }

    //Daikon Controller
    private void UpdateDaikon(float value) {
        Daikon.transform.localPosition = new Vector3(Daikon.transform.localPosition.x, value, Daikon.transform.localPosition.z);
    }

    private void DelayedForce() {
        //Launch The Radish!
        Rigidbody rb = Daikon.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        float throwSpeed = 6f;
        float launchAngle = -2.5f;
        
        Quaternion rotation = Quaternion.Euler( launchAngle, 0, 0);
        Quaternion rotationSpeed = Quaternion.Euler(10f,10f,10f);
        Vector3 velocity = rotation * (Vector3.up * throwSpeed) + (Vector3.forward * 2.5f);
        
        rb.velocity = velocity;
        rb.rotation = rotationSpeed;
    }

    //Level Two
    private void InputListen(Parameters parameters) {
        spacePress = parameters.GetBoolExtra(INPUT_PRESS, false);
        if(spacePress) {
            dropPos = dropUI.transform.localPosition.x;
            CheckHit();
        }
        StateHandler();
    }

    private void CheckHit() {
        if(dropPos > -15 && dropPos < 15) {

            //Update Meter
            meterFill += 10;

            //Enable Camera Shake when Input
            vfxState = VFXState.Playing;

            //Update VFX
            Broadcaster.Instance.AddBoolParam(CameraShake.CAMERA_SHAKE, EventNames.Scene1.CAMERA_SHAKE, true);
            Broadcaster.Instance.AddBoolParam(SFXController.PLAY_CLIP_S2, EventNames.KeyboardInput.INTERACT_PRESS, true);
            
            //Add Score
            if(enableHorror) PlayerData.Score += 30;
            else PlayerData.Score += 20;

            EnableAnim();
            PlayParticle();

            //If fill is Full
            if(meterFill >= 100) {
                meterFill = 100; 

                //GAME PAUSED
                HideMeters();
                EnablePauseCamera_Variation();

                //Pause Func
                GameTimeManager.Instance.timerState = TimerState.Paused;

                water.Pause();

                //Init Level as Unplayable
                levelState = LevelState.Unplayable;

                // Disable SFX
                Broadcaster.Instance.AddSFXState(SFXController.DISABLE_SFX, EventNames.Scene1.DISABLE_SFX, SFXState.Paused);

                //Change Scene
                Invoke(nameof(ChangeScene), 2.5f);
            }
        }
        else {
            meterFill -= 10;
            if(meterFill <= 0) {
                meterFill = 0;
            }
        }
    }

    //Level Three
    private void CheckDir() {
        switch(PlayerData.HorizontalDir) {
            case Direction_Hor.Right:
                player.transform.localScale = new Vector3(-0.625f, player.transform.localScale.y, player.transform.localScale.z); 
                break;
            case Direction_Hor.Left:
                player.transform.localScale = new Vector3(0.625f, player.transform.localScale.y, player.transform.localScale.z); 
                break;
        }
    }

    private void MoveEvent(Parameters parameters) {
        moveInput = parameters.GetVector3Extra(KEY_MOVE, Vector3.zero);
        if(levelState == LevelState.Playable) {
            //Particle
            PlayParticle();
            rb.MovePosition(player.transform.position + moveInput.ToIso() * moveInput.normalized.magnitude * 4f * Time.deltaTime);
        }
    }

    private void PlayerParticle() {
        ParticleSystem.EmissionModule temp = playerDust.emission;
        if(PlayerData.playerState == PlayerState.Moving) temp.enabled = true;
        else temp.enabled = false;
    }

    private void InteractPile(Parameters parameters) {
        ePress = parameters.GetBoolExtra(INPUT_E, false);
        if(ePress && levelState == LevelState.Playable) {
            PlayerData.ePress = true;
        }
        else {
            PlayerData.ePress = false;
        }
        StateHandler();
    }


    private void UpdateCounter() {
        counter = PlayerData.counterDaikon;
        
        if(counter == 0) meterValue_Vert = 0; 
        else if(counter == 1) meterValue_Vert = 25;
        else if(counter == 2) meterValue_Vert = 50;
        else if(counter == 3) meterValue_Vert = 75;
        else {
            meterValue_Vert = 100;
            HideMeters();
            EnablePauseCamera_Variation();

            GameTimeManager.Instance.timerState = TimerState.Paused;

            // Disable SFX
            Broadcaster.Instance.AddSFXState(SFXController.DISABLE_SFX, EventNames.Scene1.DISABLE_SFX, SFXState.Paused);

            levelState = LevelState.Unplayable;
            Invoke(nameof(ChangeScene),3f);
        }
    }

    //Effects
    private void PlayParticle() {
        switch(SceneManager.GetActiveScene().buildIndex) {
            case 0:
                if(spacePress) ground.Play();
                else ground.Stop();
                break;
            case 1:
                if(spacePress) ground.Play();
                else ground.Stop();
                break;
            case 2:
                if(spacePress) water.Play();
                else water.Stop();
                break;
            case 3:
                ParticleSystem.EmissionModule temp = playerDust.emission;
                if(PlayerData.playerState == PlayerState.Moving) temp.enabled = true;
                else temp.enabled = false;
                break;
        }
    }

    //Post Process Things
    private void InitPostProcess() {
        float chancesFloat = UnityEngine.Random.Range(0f,1f);
        if(chancesFloat <= 0.25) {
            //Score
            canvasObject.SetActive(false);
            canvasObjectALT.SetActive(true);

            //Toggle
            enableHorror = true;
            PlayerData.enableHorror = true;

            //PostProcess
            pauseProfile.SetActive(false);
            horrorPauseProfile.SetActive(false);
            horrorProfile.SetActive(true);

            //HUD
            playHUD.SetActive(true);
            pauseHUD.SetActive(false);

            //Elements
            pauseText.SetActive(false);
        }
        else PlayerData.enableHorror = false;
    }

    //HideStuffs
    private void HideMeters() {
        switch(SceneManager.GetActiveScene().buildIndex) {
            case 0:
                meterUI.SetActive(false);
                break;
            case 1:
                meterUI.SetActive(false);
                break;
            case 2:
                fillUI.SetActive(false);
                break;
            case 3:
                meterUI_Vert.SetActive(false);
                break;
        }
    }

    //Views
    private void EnablePauseCamera_Variation() {
        if(enableHorror == true) {
            pauseProfile.SetActive(false);
            horrorPauseProfile.SetActive(true);
            horrorProfile.SetActive(false);
        }
        else {
            //PostProcess
            pauseProfile.SetActive(true);
            horrorPauseProfile.SetActive(false);
            horrorProfile.SetActive(false);
        }
        playHUD.SetActive(false);
        pauseHUD.SetActive(true);
        pauseText.SetActive(true);
    }

    private void EnableEndCamera() {
        pauseProfile.layer = LayerMask.NameToLayer("PostProcessing");

        //Elements
        endScreen.SetActive(true);

        //PostProcess
        pauseProfile.SetActive(true);
        horrorPauseProfile.SetActive(false);
        horrorProfile.SetActive(false);

        //HUD
        playHUD.SetActive(false);
        pauseHUD.SetActive(false);
        endHUD.SetActive(true);

        if(SceneManager.GetActiveScene().buildIndex == 0) inputUI.SetActive(false);
    }

    //Scene Changer
    private void ChangeScene() {

        //Change Next Scene
        SceneController.Instance.ChangeSceneManager();

        //Resume Func
        GameTimeManager.Instance.timerState = TimerState.Playing;
    }

    //Animation Stuffs
    //Score Animation
    public void EnableAnim() {
        GameObject tempCanvas;
        if(enableHorror) tempCanvas = Instantiate(canvasObjectALT, canvasObject.transform.position, Quaternion.identity);
        else tempCanvas = Instantiate(canvasObject, canvasObject.transform.position, Quaternion.identity);
        
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
