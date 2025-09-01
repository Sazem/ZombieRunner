using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button playButton;
    [SerializeField] private TextMeshProUGUI scoreText;
    void Start()
    {
        if (playButton != null)
        {
            playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play!";
        }
    }

    public void HideMenu()
    {
        gameObject.SetActive(false);
    }
    public void ShowScoreboard(int end_score)
    {
        print("we are at the menu.");
        scoreText.text = "Your score: " + end_score.ToString();
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Replay?";
    }

}
