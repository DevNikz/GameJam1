using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    //Scene 1 Controls
    public bool interactPress { get; private set; }

    //Scene 3 Controls
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }
    public Vector3 MoveInput { get; private set; }

    public bool interactE { get; private set; }

    //Set Reference to PlayerInput Component
    private PlayerInput _playerInput;

    //Set Reference to InputAction for Scene 1
    private InputAction _Scene1;
    private InputAction _Horizontal;
    private InputAction _Vertical;
    private InputAction _Interact;

    private void Start() {
        _playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    private void Update() {
        UpdateInputs();
    }

    private void SetupInputActions() {
        _Scene1 = _playerInput.actions["Scene1"];
        _Horizontal = _playerInput.actions["HorizontalMove"];
        _Vertical = _playerInput.actions["VerticalMove"];
        _Interact = _playerInput.actions["Interact"];
    }

    private void UpdateInputs() {
        //Scene1
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1) loadLevelOne();
        else if(SceneManager.GetActiveScene().buildIndex == 2)loadLevelThree();
    }

    private void loadLevelOne() {
        interactPress = _Scene1.WasPressedThisFrame();

        Broadcaster.Instance.AddBoolParam(LevelController.INPUT_PRESS, EventNames.KeyboardInput.INTERACT_PRESS, interactPress);
        Broadcaster.Instance.AddBoolParam(SFXController.PLAY_CLIP_S1, EventNames.KeyboardInput.INTERACT_PRESS, interactPress);
    }

    private void loadLevelThree() {
        Horizontal = _Horizontal.ReadValue<float>();
        Vertical = _Vertical.ReadValue<float>();
        MoveInput = new Vector3(Horizontal, 0f, Vertical);

        interactE = _Interact.WasPerformedThisFrame();

        //Move Directions Horizontal
        if(Horizontal == 1) PlayerData.HorizontalDir = Direction_Hor.Right;
        else if(Horizontal == -1) PlayerData.HorizontalDir = Direction_Hor.Left;

        //Move Directions Horizontal
        if(Vertical == 1) PlayerData.VerticalDir = Direction_Vert.Up;
        else if(Vertical == -1) PlayerData.VerticalDir = Direction_Vert.Down;

        //Player State
        if(Horizontal == 0 && Vertical == 0) PlayerData.playerState = PlayerState.Idle;
        else PlayerData.playerState = PlayerState.Moving;

        Broadcaster.Instance.AddVectorParam(LevelController.KEY_MOVE, EventNames.KeyboardInput.MOVE_INPUT, MoveInput);
        Broadcaster.Instance.AddBoolParam(LevelController.INPUT_E, EventNames.KeyboardInput.INTERACT_E, interactE);
    }
}
