using UnityEngine;
using TMPro;

public class HUDScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countText;
    public SpriteRenderer[] lives;
    public Sprite FullEgg; 
    public Sprite BrokenEgg;   
    private GameManager gameManager; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        UpdateLives();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: "+gameManager.GetScore();
        countText.text = "Eggs: "+gameManager.GetCount();
        UpdateLives();
    }

    void UpdateLives()
    {
        for(int i=0; i<lives.Length;i++)
        {     
            if(i<gameManager.GetChances())
            {
                lives[i].sprite=FullEgg;
            }
            else
            {
                lives[i].sprite = BrokenEgg;
            }
        }
    }
}
