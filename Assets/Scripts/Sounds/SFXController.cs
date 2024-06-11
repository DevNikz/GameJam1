using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXController : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] public float Volume = 1f;

    //Level 1
    public AudioClip dirt1;
    public AudioClip dirt2;
    public AudioClip dirt3;
    public AudioClip dirt4;

    //Level 2
    public AudioClip water1;
    private int currentIndex = 0;
    private bool inputPress;
    [SerializeField] public SFXState sfxState = SFXState.Paused;
    public const string PLAY_CLIP_S1 = "PLAY_CLIP_S1";
    public const string DISABLE_SFX = "DISABLE_SFX";

    private void Start() {
        audioSource.volume = Volume;
        audioSource = this.gameObject.GetComponent<AudioSource>();

        EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.PlayClip);
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.DISABLE_SFX, this.PlayClip);
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.DISABLE_SFX);
    }

    public void PlayClip(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(PLAY_CLIP_S1, false);
        sfxState = parameters.GetSFXState(DISABLE_SFX, SFXState.Playing);
        if(sfxState == SFXState.Paused) Destroy(this);
        
        if(inputPress && sfxState == SFXState.Playing) {
            if(currentIndex > 3) currentIndex = 0;
            switch(currentIndex) {
                case 0:
                    audioSource.PlayOneShot(dirt1, Volume);
                    break;
                case 1:
                    audioSource.PlayOneShot(dirt2, Volume);
                    break;
                case 2:
                    audioSource.PlayOneShot(dirt3, Volume);
                    break;
                case 3:
                    audioSource.PlayOneShot(dirt4, Volume);
                    break;
            }
            currentIndex += 1;
        }
    }
}
