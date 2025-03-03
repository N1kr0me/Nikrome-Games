using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    public Transform highscoreContainer;  // Parent container for high scores
    public GameObject highScoreTemplate;  // Prefab for a single entry
    private const int maxDisplayedScores = 5; // Show only 5 entries

    void Start()
    {
        DisplayHighScores();
    }

    void DisplayHighScores()
    {
        List<HighScoreEntry> highScores = LoadHighScores();
        highScores = highScores.Take(maxDisplayedScores).ToList(); // Limit to 5 entries

        foreach (var scoreEntry in highScores)
        {
            GameObject newEntry = Instantiate(highScoreTemplate, highscoreContainer);
            newEntry.SetActive(true);

            TextMeshProUGUI[] textElements = newEntry.GetComponentsInChildren<TextMeshProUGUI>();
            textElements[0].text = (highScores.IndexOf(scoreEntry) + 1).ToString(); // Position
            textElements[1].text = scoreEntry.playerName;  // Name
            textElements[2].text = scoreEntry.eggs.ToString(); // Egg Count
            textElements[3].text = scoreEntry.score.ToString(); // Score
        }
    }

    private List<HighScoreEntry> LoadHighScores()
    {
        string json = PlayerPrefs.GetString("HighScores", "{}");
        HighScoreList highScoreList = JsonUtility.FromJson<HighScoreList>(json) ?? new HighScoreList();
        return highScoreList.highScores ?? new List<HighScoreEntry>();
    }
}