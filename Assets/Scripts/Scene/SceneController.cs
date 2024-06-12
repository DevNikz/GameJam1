using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;

    private int currentScene;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void ChangeSceneManager() {
        if(SceneManager.GetActiveScene().buildIndex == 0) LoadScene1();
        else if(SceneManager.GetActiveScene().buildIndex == 1) LoadScene2();
        else if(SceneManager.GetActiveScene().buildIndex == 2) LoadScene3();
        else if(SceneManager.GetActiveScene().buildIndex == 3) LoadScene1();
    }

    private void LoadScene1() {
        currentScene = 1;
        SceneManager.LoadScene(currentScene);
    }
    
    private void LoadScene2() {
        currentScene = 2;
        SceneManager.LoadScene(currentScene);
    }

    private void LoadScene3() {
        currentScene = 3;
        SceneManager.LoadScene(currentScene);
    }
}
