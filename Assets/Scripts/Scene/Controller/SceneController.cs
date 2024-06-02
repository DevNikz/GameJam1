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
        //Debug
        //Debug.Log(SceneManager.GetActiveScene().name);

        EventBroadcaster.Instance.AddObserver(EventNames.SceneChange.CHANGE_SCENE, this.NextScene);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.SceneChange.CHANGE_SCENE);
    }

    public void NextScene(Parameters parameters) {
        Debug.Log("Loading Next Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
