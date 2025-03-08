using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System;

public class HighScoreManager : MonoBehaviour
{
    public TMP_InputField nameInput;
    private TouchScreenKeyboard mobileKeyboard;
    public TMP_Text SaveMessage;
    private bool scoreSaved = false;
    private static int lastSavedScore = -1;

    private string sheetURL = "https://script.google.com/macros/s/AKfycbwArpzfgrwxbcvGJJBMBY1jtdZjHHtFjxSQcl1k8IoLq5XHJSuWib0ZPHikWYdogQw_/exec";

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

        // Close keyboard when tapping outside
        if (IsMobileBrowser() && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(nameInput.GetComponent<RectTransform>(), touch.position, null))
                {
                    nameInput.DeactivateInputField();
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
            return;
        }

        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            SaveMessage.text = "Enter name to save.";
            SaveMessage.gameObject.SetActive(true);
            return;
        }

        SaveMessage.gameObject.SetActive(false);

        string playerName = nameInput.text.Length > 6 ? nameInput.text.Substring(0, 6) : nameInput.text;
        int score = GameManager.Instance.GetScore();
        int eggs = GameManager.Instance.GetCount();

        StartCoroutine(PostHighScore(playerName, score, eggs));

        SaveMessage.text = "Score saved!";
        SaveMessage.gameObject.SetActive(true);
        scoreSaved = true;
        lastSavedScore = score;
    }

    IEnumerator PostHighScore(string name, int score, int eggs)
    {
        string json = $"{{\"name\":\"{name}\",\"score\":{score},\"eggs\":{eggs}}}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        UnityWebRequest request = new UnityWebRequest(sheetURL, "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Score saved successfully!");
        else
            Debug.LogError("Error saving score: " + request.error);
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
        return Input.touchSupported; // Works better in WebGL
    }
}