using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeReference] public TextMeshProUGUI scoreObject;

    [SerializeField] public int score;

    private void Start() {
        scoreObject = this.gameObject.GetComponent<TextMeshProUGUI>();
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
    }
}
