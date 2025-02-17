using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private bool flag= true; //to signal stoppage of spawn
    public int score = 0; // Score 
    public int chances = 0; // Chances 
    public int maxChances = 3; // Max chances before game over
    public int count=0;//no of items caught

    void Awake()
    {
        // Singleton pattern to ensure only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseScore(int value)
    {
        score += value; // Increase score when egg is caught
        count++; 
        Debug.Log("Egg caught") ;  
    }

    public void DecreaseChances()
    {
        
        chances++; // Decrease chances when an egg is dropped
        Debug.Log("Oops, Dropped " + chances);
        if (chances == maxChances)
        {
            // Handle game over (you can add scene switching or UI handling here)
            Debug.Log("Game Over");
            flag = false;
        }
    }

    public void ResetGame()
    {
        score = 0;
        chances = 0;
        count=0;
        flag=true;
    }

    public int GetCount()
    {
        return count;   //returns to item count
    }

    public int GetScore()
    {
        return score;
    }
    public bool GetFlag()
    {
        return flag;
    }
}
