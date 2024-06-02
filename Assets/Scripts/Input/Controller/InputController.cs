using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    //Scene 1 Controls
    public bool interactPress { get; private set; }

    //Set Reference to PlayerInput Component
    private PlayerInput _playerInput;

    //Set Reference to InputAction for Scene 1
    private InputAction _Scene1;

    private void Awake() {
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
        interactPress = _Scene1.WasPressedThisFrame();

        Parameters parameters = new Parameters();
        parameters.PutExtra(SceneController.INPUT_PRESS, interactPress);

        EventBroadcaster.Instance.PostEvent(EventNames.KeyboardInput.INTERACT_PRESS, parameters);
    }
}
