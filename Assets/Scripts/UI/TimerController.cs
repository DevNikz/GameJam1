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
        if(timer >= 60) ToMinutes();
        else ToSeconds();
    }

    private void ToMinutes() {
        //Minute
        int minutes = (int)timer / 60;

        //Second
        int seconds = (int)timer % 60;

        //Milisecond
        float secondsF = timer % 60f;
        float miliseconds = secondsF - seconds;
        miliseconds = Mathf.Round(miliseconds * 100f);

        //To Minute
        if(seconds > 9) {
            if((int)miliseconds > 9) timerObject.text = "0" + minutes + ":" + seconds + ":" + (int)miliseconds;
            else timerObject.text = "0" + minutes + ":" + seconds + ":0" + (int)miliseconds;
        }

        else {
            if((int)miliseconds > 9) timerObject.text = "0" + minutes + ":0" + seconds + ":" + (int)miliseconds;
            else if((int)miliseconds <= 9) timerObject.text = "0" + minutes + ":0" + seconds + ":0" + (int)miliseconds;
            else if((int)miliseconds <= 0) timerObject.text = "00:00:00";
        }
    }

    private void ToSeconds() {
        float a = timer;
        float b = a - (int)a;
        b = Mathf.Round(b * 100f);

        //Seconds
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
