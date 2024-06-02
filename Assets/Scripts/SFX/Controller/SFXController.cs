using UnityEngine;
using UnityEngine.SceneManagement;

public class SFXController : MonoBehaviour
{

    public AudioSource audioSource;

    [SerializeField] public float Volume = 1f;

    public AudioClip dirt1;
    public AudioClip dirt2;
    public AudioClip dirt3;
    public AudioClip dirt4;
    private int currentIndex = 0;
    private bool inputPress;

    public const string PLAY_CLIP_S1 = "PLAY_CLIP_S1";

    private void Start() {
        audioSource.volume = Volume;

        if(SceneManager.GetActiveScene().buildIndex == 0) {
            EventBroadcaster.Instance.AddObserver(EventNames.KeyboardInput.INTERACT_PRESS, this.PlayClip);
        }
    }

    private void OnDestroy() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.KeyboardInput.INTERACT_PRESS);
    }

    public void PlayClip(Parameters parameters) {
        inputPress = parameters.GetBoolExtra(PLAY_CLIP_S1, false);

        if(inputPress) {
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
