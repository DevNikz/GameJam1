using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    [SerializeReference] public Collider playerCollider;
    [SerializeField] private List<Collider> interact;

    private void Start() {
        playerCollider = this.gameObject.GetComponent<Collider>();
    }

    private void Update() {
        //Debug.Log(PlayerData.interactState);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Bounds")
        {
            interact.Add(other);
            GameObject highlight = other.transform.Find("Highlight").gameObject;
            highlight.SetActive(true);
        }
    }

        private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag == "Bounds")
        {
            //Check if pressed, enable object
            if(PlayerData.ePress == true) {  
                //Destroy Highlight
                GameObject highlight = other.transform.Find("Highlight").gameObject;
                Destroy(highlight);
                Destroy(other);

                //Set Daikon To true
                GameObject Daikon = other.transform.Find("Daikon").gameObject;
                Daikon.SetActive(true);

                //Enable particle
                ParticleSystem particle = other.transform.Find("Particle").gameObject.GetComponent<ParticleSystem>();
                particle.Play();

                PlayerData.counterDaikon += 1;
                PlayerData.Score += 10;
                Debug.Log(PlayerData.counterDaikon);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Bounds")
        {
            interact.Remove(other);
            GameObject highlight = other.transform.Find("Highlight").gameObject;
            highlight.SetActive(false);
            //Enable Enum
        }
    }
}
