using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI handling
/// </summary>

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public Text fillText;
    public Text timerText;
    public Text gameOver;

    public GameObject startScreen;

    public InputField enemiesAmountInputField;
    public InputField fillAreaInputField;

    public void SetActiveGameOver() => gameOver.gameObject.SetActive(true);
    public void SetLivesText(int lives) => livesText.text = lives.ToString();
    public void SetScoresText(int scores) => scoreText.text = scores.ToString();
    public void SetFillText(int waterArea) => fillText.text = $"{waterArea.ToString()} %";
    public void SetTimerText(int time) => timerText.text = time.ToString();

}
