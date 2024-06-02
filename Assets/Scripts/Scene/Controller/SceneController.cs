using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Input")]

    [SerializeField] public bool inputPress;
    [SerializeField] public int meterValue;

    public const string INPUT_PRESS = "INPUT_PRESS";

    private Parameters parameters;

    private SFXController sfxController;

    private void Start() {
        //Init Observer
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputPress);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
    }

    private void Update() {
        StateHandler();
    }

    private void InputPress(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(INPUT_PRESS, false);
    }

    private void StateHandler() {
        if(meterValue >= 240) {
            meterValue = 0;
        }

        if(this.inputPress) {
            //Update Meter
            meterValue += 10;

            parameters = new Parameters();
            parameters.PutExtra(UIController.INCREASE_METER, meterValue);
            parameters.PutExtra(UIController.UI_NAME, "Level [Meter]");

            EventBroadcaster.Instance.PostEvent(EventNames.Scene1.INCREASE_METER, parameters);
        }

    }
}
