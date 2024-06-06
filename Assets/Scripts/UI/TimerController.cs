using TMPro;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    [SerializeReference] public TextMeshProUGUI timerObject;

    [SerializeField] public float timer;

    private void Reset() {
        timerObject = transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        timerObject = transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        timer = PlayerData.Timer;
        UpdateTimer();
    }

    private void UpdateTimer() {
        float a = timer;
        float b = a - (int)a;
        b = Mathf.Round(b * 100f);

        if((int)a > 9) {
            if((int) b > 9) timerObject.text = "00:" + (int)a + ":" + (int)b;
            else timerObject.text = "00:" + (int)a + ":0" + (int)b;
        }

        else {
            if((int) b > 9) timerObject.text = "00:0" + (int)a + ":" + (int)b;
            else if((int)b <= 9) timerObject.text = "00:0" + (int)a + ":0" + (int)b;
            else if((int)b <= 0) timerObject.text = "00:00:00";
        }
    }
}
