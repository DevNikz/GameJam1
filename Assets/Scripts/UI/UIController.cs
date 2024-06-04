using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] public GameObject UIMeter;
    [SerializeField] public int meterValue;

    public const string INCREASE_METER = "INCREASE_METER";

    private void Start() {

        UIMeter = this.gameObject;

        //Init Observer for Scene1
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.INCREASE_METER, this.IncreaseMeter);
    }

    private void OnDestroy() {
        //Destroy Observer for Scene1
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.INCREASE_METER);
    }

    private void IncreaseMeter(Parameters parameters) {
        meterValue = parameters.GetIntExtra(INCREASE_METER, 0);
        this.UpdateMeter();
    }

    private void UpdateMeter() {
        RectTransform rt = this.gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, meterValue);
    }
}
