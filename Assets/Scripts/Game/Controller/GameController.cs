using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] public GameObject Daikon;
    [SerializeField] public ParticleSystem ground;

    [Header("Input")]

    [SerializeField] public bool inputPress;
    [SerializeField] public int meterValue;

    public const string INPUT_PRESS = "INPUT_PRESS";

    private Parameters parameters;

    private string currentScene;

    private bool isPressable = true;

    private void Start() {
        ground.Stop();

        currentScene = SceneManager.GetActiveScene().name;

        Daikon.SetActive(true);
        Rigidbody rb = Daikon.GetComponent<Rigidbody>();
        rb.useGravity = false;
        Daikon.transform.localPosition = new Vector3(0f, -2.25f, 1.75f);

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

        if(inputPress) {
            //Play Particle
            ground.Play();

            Debug.Log("Pressed");
        }
        else ground.Stop();
    }

    private void StateHandler() {
        if(meterValue >= 100) {
            //Init this scene as not pressable
            this.isPressable = false;

            parameters = new Parameters();
            parameters.PutExtra(SFXController.DISABLE_SFX, false);
            EventBroadcaster.Instance.PostEvent(EventNames.Scene1.DISABLE_SFX, parameters);

            //Launch Object
            Invoke(nameof(DelayedForce), 0.025f);

            //Change Scene
            Invoke(nameof(ChangeScene), 2.5f);
        }

        if(this.inputPress && this.isPressable) {
            Rigidbody rb = Daikon.GetComponent<Rigidbody>();

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
        Debug.Log("Add Force");
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
        parameters = new Parameters();
        parameters.PutExtra(SceneController.SCENE_NAME, SceneManager.GetActiveScene().name);

        EventBroadcaster.Instance.PostEvent(EventNames.SceneChange.CHANGE_SCENE, parameters);
    }
}
