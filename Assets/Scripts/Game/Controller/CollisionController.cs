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
    private GameObject tempInputUI;
    private ParticleSystem particle;
    public GameObject canvasObject;
    public GameObject canvasObjectALT;
    public List<GameObject> canvasList;

    private void Start() {
        playerCollider = this.gameObject.GetComponent<Collider>();
        input.SetActive(false);

        //Score
        canvasObject.GetComponent<CanvasGroup>().alpha = 0;
        canvasObjectALT.GetComponent<CanvasGroup>().alpha = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Bounds")
        {
            interact.Add(other);
            highlight = other.transform.Find("Highlight").gameObject;
            highlight.SetActive(true);
            
            tempInputUI = Instantiate(input, input.transform.position, Quaternion.identity);
            tempInput.Add(tempInputUI);
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

                //Score
                EnableAnim();

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
                
                if(PlayerData.enableHorror) PlayerData.Score += 30;
                else PlayerData.Score += 10;
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

    public void EnableAnim() {
        GameObject tempCanvas;
        if(PlayerData.enableHorror) tempCanvas = Instantiate(canvasObjectALT, canvasObject.transform.position, Quaternion.identity);
        else tempCanvas = Instantiate(canvasObject, canvasObject.transform.position, Quaternion.identity);
        
        canvasList.Add(tempCanvas);

        tempCanvas.GetComponent<CanvasGroup>().alpha = 0;
        
        LeanTween.alphaCanvas(tempCanvas.GetComponent<CanvasGroup>(), 1, 0.35f);
        LeanTween.moveLocalY(tempCanvas.GetComponentInChildren<RectTransform>().gameObject, 7.39f, 0.25f).setOnComplete(FadeOut);
    }

    private void FadeOut() {
        canvasList[0].GetComponent<CanvasGroup>().alpha = 0;
        LeanTween.alphaCanvas(canvasList[0].GetComponent<CanvasGroup>(), 0, 0.5f).delay = 0.2f;

        Destroy(canvasList[0]);
        canvasList.Remove(canvasList[0]);
    }
}
