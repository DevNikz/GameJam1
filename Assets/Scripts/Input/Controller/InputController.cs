using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    //Scene 1 Controls
    public bool interactPress { get; private set; }

    //Set Reference to PlayerInput Component
    private PlayerInput _playerInput;

    //Set Reference to InputAction for Scene 1
    private InputAction _Scene1;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }
    
    private void Start() {
        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    private void Update() {
        UpdateInputs();
    }

    private void SetupInputActions() {
        _Scene1 = _playerInput.actions["Scene1"];
    }

    private void UpdateInputs() {
        //Scene1
        if(SceneManager.GetActiveScene().buildIndex == 0) loadLevelOne();
    }

    private void loadLevelOne() {
        interactPress = _Scene1.WasPressedThisFrame();
        Broadcaster.Instance.AddBoolParam(LevelController.INPUT_PRESS, EventNames.KeyboardInput.INTERACT_PRESS, interactPress);
        Broadcaster.Instance.AddBoolParam(SFXController.PLAY_CLIP_S1, EventNames.KeyboardInput.INTERACT_PRESS, interactPress);
    }
}
