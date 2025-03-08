using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

[System.Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
    public int eggs;
}

[System.Serializable]
public class HighScoreList
{
    public List<HighScoreEntry> highScores;
}

public class HighScoreDisplay : MonoBehaviour
{
    public Transform highscoreContainer;
    public GameObject highScoreTemplate;
    
    private string sheetURL = "https://script.google.com/macros/s/AKfycbwArpzfgrwxbcvGJJBMBY1jtdZjHHtFjxSQcl1k8IoLq5XHJSuWib0ZPHikWYdogQw_/exec";

    void Start()
    {
        LoadHighScores();
    }

    public void LoadHighScores()
    {
        StartCoroutine(GetHighScores());
    }

    IEnumerator GetHighScores()
    {
        UnityWebRequest request = UnityWebRequest.Get(sheetURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Raw JSON Response: " + request.downloadHandler.text);

            try
            {
                HighScoreList highScoreList = JsonUtility.FromJson<HighScoreList>(request.downloadHandler.text);

                if (highScoreList != null && highScoreList.highScores != null)
                {
                    int rank = 1;

                    foreach (var entry in highScoreList.highScores)
                    {
                        GameObject newEntry = Instantiate(highScoreTemplate, highscoreContainer);
                        newEntry.SetActive(true);
                        
                        TextMeshProUGUI[] textElements = newEntry.GetComponentsInChildren<TextMeshProUGUI>();
                        textElements[0].text = rank.ToString(); // Position
                        textElements[1].text = entry.playerName;  // Name
                        textElements[2].text = entry.eggs.ToString(); // Egg Count
                        textElements[3].text = entry.score.ToString(); // Score
                        rank++;
                    }
                }
                else
                {
                    Debug.LogWarning("Parsed HighScoreList is empty or null.");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}");
            }
        }
        else
        {
            Debug.LogError("Error retrieving scores: " + request.error);
        }
    }
}