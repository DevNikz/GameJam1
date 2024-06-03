using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicController : MonoBehaviour
{

    public AudioSource audioSource;

    [SerializeField] public float Volume = 0.7f;

    public AudioClip Theme1;

    private void Awake() {
        DontDestroyOnLoad(this);
    }

    private void Start() {
        audioSource.volume = Volume;

        audioSource.clip = Theme1;
        audioSource.loop = true;
        audioSource.Play();
    }

}
