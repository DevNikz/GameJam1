using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;
    public GameObject Theme;
    public GameObject Gameover;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else Destroy(gameObject);
    }

    private void Update() {
        CheckState();
    }

    private void CheckState() {
        if(GameTimeManager.Instance.gameState == GameState.End) {
            Theme.SetActive(false);
            Gameover.SetActive(true);
        }
        else {
            Theme.SetActive(true);
            Gameover.SetActive(false);
        }
    }

}
