using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //Scene Properties
    private int randomIndex;

    private Scene currentScene;

    public const string SCENE_NAME = "SCENE_NAME";

    private void Start() {

        EventBroadcaster.Instance.AddObserver(EventNames.SceneChange.CHANGE_SCENE, this.NextScene);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.SceneChange.CHANGE_SCENE);
    }

    public void NextScene(Parameters parameters) {
        Debug.Log("Reloading Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
