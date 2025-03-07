using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;

public class HighScoreManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    private TouchScreenKeyboard mobileKeyboard;
    public TMP_Text SaveMessage; // Assign "Score Saved" TMP
    private bool scoreSaved = false;
    private static int lastSavedScore = -1; // Track if this session's score was saved

    private const int maxHighScores = 10;

    void Start()
    {
        nameInput.onSelect.AddListener(OpenKeyboard);
        nameInput.onDeselect.AddListener(CloseKeyboard);
    }

    void Update()
    {
        // Ensure text input syncs with keyboard input
        if (mobileKeyboard != null && mobileKeyboard.active)
        {
            nameInput.text = mobileKeyboard.text;
        }
        if (IsMobileBrowser() && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended) // Detect touch release
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(
                        nameInput.GetComponent<RectTransform>(), touch.position, null))
                {
                    nameInput.DeactivateInputField(); // Close keyboard
                }
            }
        }
    }

    public void SaveScore()
    {
        if (scoreSaved || lastSavedScore == GameManager.Instance.GetScore())
        {
            SaveMessage.text = "Already saved.";
            SaveMessage.gameObject.SetActive(true);
            AudioManager.Instance.PlayClickSound();
            return;
        }

        if (string.IsNullOrWhiteSpace(nameInput.text)) // Check if name is empty
        {
            SaveMessage.text = "Enter name to save.";
            SaveMessage.gameObject.SetActive(true);
            AudioManager.Instance.PlayEggBrokenSound();
            return;
        }

        SaveMessage.gameObject.SetActive(false); // Hide any previous messages

        string playerName = nameInput.text.Length > 6 ? nameInput.text.Substring(0, 6) : nameInput.text;
        int score = GameManager.Instance.GetScore();
        int eggs = GameManager.Instance.GetCount();
        long timestamp = DateTime.UtcNow.Ticks; // Unique timestamp for different game sessions

        List<HighScoreEntry> highScores = LoadHighScores();

        // Save new entry
        highScores.Add(new HighScoreEntry { playerName = playerName, score = score, eggs = eggs, timestamp = timestamp });

        // Sort and keep only the top 5 (since you mentioned only 5 fit)
        highScores = highScores.OrderByDescending(h => h.score).Take(maxHighScores).ToList();

        SaveHighScores(highScores);

        SaveMessage.text = "Score saved!";
        AudioManager.Instance.PlayEggBonusSound();
        SaveMessage.gameObject.SetActive(true);
        scoreSaved = true;
        lastSavedScore = score; // Mark this session’s score as saved
    }

    private List<HighScoreEntry> LoadHighScores()
    {
        string json = PlayerPrefs.GetString("HighScores", "{}");
        HighScoreList highScoreList = JsonUtility.FromJson<HighScoreList>(json) ?? new HighScoreList();
        return highScoreList.highScores ?? new List<HighScoreEntry>();
    }

    private void SaveHighScores(List<HighScoreEntry> highScores)
    {
        HighScoreList highScoreList = new HighScoreList { highScores = highScores };
        string json = JsonUtility.ToJson(highScoreList);
        PlayerPrefs.SetString("HighScores", json);
        PlayerPrefs.Save();
    }
    
    void OpenKeyboard(string text)
    {
        if (IsMobileBrowser())
        {
            Debug.Log("Opening Mobile Keyboard...");
            mobileKeyboard = TouchScreenKeyboard.Open(nameInput.text, TouchScreenKeyboardType.Default);
        }
    }

    void CloseKeyboard(string text)
    {
        if (IsMobileBrowser() && mobileKeyboard != null)
        {
            Debug.Log("Closing Mobile Keyboard...");
            mobileKeyboard.active = false;
        }
    }

    bool IsMobileBrowser()
    {
        return Input.touchSupported; // More reliable in WebGL than checking `DeviceType.Handheld`
    }
}

[System.Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
    public int eggs;
    public long timestamp; // Used to differentiate identical scores from different sessions
}

[System.Serializable]
public class HighScoreList
{
    public List<HighScoreEntry> highScores;
}