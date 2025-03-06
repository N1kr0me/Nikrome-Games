using UnityEngine;

public class Item : MonoBehaviour
{
    public CatchItem itemData;
    private GameManager gameManager;
    private float Fall;
    private bool Collided=false;
    void Start()
    {
        gameManager = GameManager.Instance; // Get the GameManager instance
        GetComponent<SpriteRenderer>().sprite = itemData.objectSprite;
        Fall= Mathf.Max(itemData.fallSpeed*0.5f, itemData.fallSpeed*(1f-(0.01f*gameManager.GetCount())));;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.GetFlag())
        {
            transform.Translate(Vector2.down * Fall * Time.deltaTime);
        }
        else if(!Collided)
        {             
            DestroyAll();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Collided)
        {
            //DestroyAll();
            return;
        }
        else
        {
            // If the item collides with the ground
            if (collision.CompareTag("Ground"))
            {
                Collided = true;
                Fall=0f;
                if(!itemData.Bonus)
                {
                    gameManager.DecreaseChances(); // Decrease chances only is NormalEgg
                }
                GetComponent<SpriteRenderer>().sprite = itemData.brokenSprite; // Change to broken egg sprite
                Invoke(nameof(DestroyAll), 5f); // Destroy after 1 second
                AudioManager.Instance.PlayEggBrokenSound();
            }
            // If the item collides with the player
            else if (collision.CompareTag("Player"))
            {
                if(itemData.Heal)
                {
                    gameManager.IncreaseChances();
                    AudioManager.Instance.PlayEggBonusSound();
                }
                else
                {
                    if(itemData.Bad)
                    {
                        gameManager.IncreaseScore(itemData.scoreValue,0); // alter score but not count
                        AudioManager.Instance.PlayEggBrokenSound();
                    }
                    else
                    {
                        gameManager.IncreaseScore(itemData.scoreValue,1); // alter Score and count
                        AudioManager.Instance.PlayEggCollectedSound();
                    }
                }   
                DestroyAll(); // Destroy the item
            }
            
        }
    }

    private void DestroyAll()
    {
        Destroy(gameObject);
    }
}
