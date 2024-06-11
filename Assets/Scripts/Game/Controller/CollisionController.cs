using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeReference] public Collider playerCollider;
    [SerializeField] private List<Collider> interact;
    [SerializeReference] public GameObject input;
    public List<GameObject> tempInput;
    private GameObject highlight;
    private GameObject Daikon;
    private GameObject tempCanvas;
    private ParticleSystem particle;

    private void Start() {
        playerCollider = this.gameObject.GetComponent<Collider>();
        input.SetActive(false);

        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Bounds")
        {
            interact.Add(other);
            highlight = other.transform.Find("Highlight").gameObject;
            highlight.SetActive(true);
            
            tempCanvas = Instantiate(input, input.transform.position, Quaternion.identity);
            tempInput.Add(tempCanvas);
            tempInput[0].SetActive(true);
        }
    }

        private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Bounds")
        {
            //Check if pressed, enable object
            if(PlayerData.ePress == true) {  
                //Destroy Highlight
                highlight = other.transform.Find("Highlight").gameObject;
                Destroy(highlight);
                Destroy(other);

                //Destroy Input Clone
                if(tempInput != null) {
                    Destroy(tempInput[0]);
                    tempInput.Remove(tempInput[0]);
                }

                //Set Daikon To true
                Daikon = other.transform.Find("Daikon").gameObject;
                Daikon.SetActive(true);

                //Enable particle
                particle = other.transform.Find("Particle").gameObject.GetComponent<ParticleSystem>();
                particle.Play();

                //Enable SFX
                Broadcaster.Instance.AddBoolParam(SFXController.PLAY_CLIP_S1, EventNames.KeyboardInput.INTERACT_PRESS, true);

                //Enable VFX
                Broadcaster.Instance.AddBoolParam(CameraShake.CAMERA_SHAKE, EventNames.Scene1.CAMERA_SHAKE, true);

                PlayerData.counterDaikon += 1;
                PlayerData.Score += 10;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Bounds")
        {
            interact.Remove(other);
            highlight = other.transform.Find("Highlight").gameObject;
            highlight.SetActive(false);
            
            if(tempInput != null) {
                Destroy(tempInput[0]);
                tempInput.Remove(tempInput[0]);
            }
        }
    }
}
