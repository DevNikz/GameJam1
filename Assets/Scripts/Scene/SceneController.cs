using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private int currentScene = 0;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void ChangeSceneManager() {
        if(PlayerData.currentScene == 0) { 
            LoadScene1();
        }
        else if(PlayerData.currentScene == 1) {
            LoadScene1();
        }
    }

    private void LoadScene1() {
        currentScene = 1;
        SceneManager.LoadScene(currentScene);
    }
}
