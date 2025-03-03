using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    private GameManager gameManager; 
    public TextMeshProUGUI finalScoreText;
    void Start()
    {
        gameManager = GameManager.Instance;
        finalScoreText.text = "Score: "+gameManager.GetScore();   
    }

}
