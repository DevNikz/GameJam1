using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    public AudioSource audioSource;

    [SerializeField] public float Volume = 0.7f;

    public AudioClip[] clips;
    
    public GameState gameState = GameState.Play;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else Destroy(gameObject);
        
    }

    private void Start() {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        audioSource.volume = Volume;
        audioSource.loop = true;

        audioSource.clip = clips[0];
        audioSource.Play();
    }

    private void Update() {
        CheckState();
    }

    private void CheckState() {
        if(GameTimeManager.Instance.gameState == GameState.End) {
            Debug.Log("Stopping Music");
            gameState = GameState.End;
            audioSource.clip = clips[1];
            audioSource.Play();
            Destroy(this);
        }
    }

}
