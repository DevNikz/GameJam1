using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] public GameObject UIMeter;
    [SerializeField] public int meterValue;

    public const string INCREASE_METER = "INCREASE_METER";
    public const string INCREASE_METER_VERT = "INCREASE_METER_VERT";

    private void Start() {
        UIMeter = this.gameObject;

        //Init Observer for Scene1
        if(SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex == 1) EventBroadcaster.Instance.AddObserver(EventNames.Scene1.INCREASE_METER, this.IncreaseMeter);
        if(SceneManager.GetActiveScene().buildIndex == 2) EventBroadcaster.Instance.AddObserver(EventNames.Scene1.INCREASE_METER_VERT, this.IncreaseMeterVert);
    }

    private void OnDestroy() {
        //Destroy Observer for Scene1
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.INCREASE_METER);
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.INCREASE_METER_VERT);
    }

    //Level 0 and Level 1
    private void IncreaseMeter(Parameters parameters) {
        meterValue = parameters.GetIntExtra(INCREASE_METER, 0);
        this.UpdateMeter();
    }

    private void UpdateMeter() {
        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, meterValue);
    }

    //Level 3
    private void IncreaseMeterVert(Parameters parameters) {
        meterValue = parameters.GetIntExtra(INCREASE_METER_VERT, 0);
        this.UpdateMeter_Vert();
    }

    private void UpdateMeter_Vert() {
        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(meterValue, rt.sizeDelta.y);
    }
}
