using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance;

    public AudioSource audioSource;

    [SerializeField] public float Volume = 0.7f;

    public AudioClip Theme1;

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

        audioSource.clip = Theme1;
        audioSource.loop = true;
        audioSource.Play();
    }

}
