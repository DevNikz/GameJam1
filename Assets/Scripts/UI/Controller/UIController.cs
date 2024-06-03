using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [ReadOnly] private GameObject[] UIArray;
    [SerializeField] public List<GameObject> UIList;

    [SerializeField] public int meterValue;

    private string UIName;

    public const string INCREASE_METER = "INCREASE_METER";

    public const string UI_NAME = "UI_NAME";

    private void Start() {
        UIList = new List<GameObject>();
        UIArray = GameObject.FindGameObjectsWithTag("UI");

        foreach(GameObject i in UIArray) {
            RectTransform rt = i.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 0);

            this.UIList.Add(i);
        }

        //Init Observer for Scene1
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.INCREASE_METER, this.IncreaseMeter);
        EventBroadcaster.Instance.AddObserver(EventNames.Scene1.INCREASE_METER, this.LocateMeter);
    }

    private void OnDestroy() {
        //Destroy Observer for Scene1
        EventBroadcaster.Instance.RemoveObserver(EventNames.Scene1.INCREASE_METER);
    }

    private void IncreaseMeter(Parameters parameters) {
        meterValue = parameters.GetIntExtra(INCREASE_METER, 0);
    }

    //Scene1 Functions 
    private void LocateMeter(Parameters parameters) {
        UIName = parameters.GetStringExtra(UI_NAME, "NONE");

        foreach(GameObject i in UIList) {
            if(i.name == UIName) {
                this.UpdateMeter(i.name);
            }
        }
    }

    private void UpdateMeter(string name) {
        foreach(GameObject i in UIList) {
            if(i.name == name) {
                RectTransform rt = i.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, meterValue);
            }
        }

    }
}
