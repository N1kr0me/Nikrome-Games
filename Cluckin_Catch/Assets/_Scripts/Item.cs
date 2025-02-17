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
        else
        {
            DestroyAll();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(Collided)
        {
            DestroyAll();
            return;
        }
        else
        {
            // If the item collides with the player
            if (collision.CompareTag("Player"))
            {
                gameManager.IncreaseScore(itemData.scoreValue); // Increase score
                DestroyAll(); // Destroy the item
            }
            // If the item collides with the ground
            else if (collision.CompareTag("Ground"))
            {
                gameManager.DecreaseChances(); // Decrease chances
                DestroyAll(); // Destroy the item
            }
        }
    }

    private void DestroyAll()
    {
        Destroy(gameObject);
    }
}
