using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Input")]

    [SerializeField] public bool inputPress;
    [SerializeField] public int meterValue;

    public const string INPUT_PRESS = "INPUT_PRESS";

    private Parameters parameters;

    private string currentScene;

    private void Start() {
        currentScene = SceneManager.GetActiveScene().name;

        //Debug
        Debug.Log(currentScene);

        //Init Observer
        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.InputPress);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
    }
    
    private void InputPress(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(INPUT_PRESS, false);
        StateHandler();
        if(inputPress) Debug.Log("Pressed");
    }

    private void StateHandler() {
        
        if(meterValue >= 240) {
            meterValue = 0;

            parameters = new Parameters();
            parameters.PutExtra(SceneController.SCENE_NAME, SceneManager.GetActiveScene().name);

            EventBroadcaster.Instance.PostEvent(EventNames.SceneChange.CHANGE_SCENE, parameters);
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
