using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeReference] public TextMeshProUGUI scoreObject;

    [SerializeField] public int score;

    private void Reset() {
        scoreObject = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        scoreObject = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        score = PlayerData.Score;
        UpdateScore();
    }

    private void UpdateScore() {
        //00000000 -> 99999990
        if(score == 0) scoreObject.text = "00000000";
        else if(score <= 90) scoreObject.text = "000000" + score;
        else if(score <= 990) scoreObject.text = "00000" + score;
        else if(score <= 9990) scoreObject.text = "0000" + score;
        else if(score <= 99990) scoreObject.text = "000" + score;
        else if(score <= 999990) scoreObject.text = "00" + score;
        else if(score <= 9999990) scoreObject.text = "0" + score;
    }
}
