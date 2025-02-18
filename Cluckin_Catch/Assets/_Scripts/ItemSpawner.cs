using UnityEngine;
using System.Collections;
public class ItemSpawner : MonoBehaviour
{   
    public GameObject[] itemPrefab; // Egg prefab
    public float spawnRate = 1f; // Start spawn time
    private float rate;
    private GameManager gameManager; // Reference to GameManager
    public GameObject ItemParent;

    void Start()
    {
        gameManager = GameManager.Instance; // Get the GameManager instance
        rate=spawnRate;
        StartCoroutine(SpawnItem()); // Spawn at regular TIME intervals
    }

    IEnumerator SpawnItem()
    {
        while (gameManager.GetFlag())
        {
            yield return new WaitForSeconds(rate); //wait for "rate" seconds
            
            // Random spawn position within the rectangle's width
            float randomX = Random.Range(transform.position.x-(transform.localScale.x/2),transform.position.x+(transform.localScale.x/2));
            Vector2 spawnPos = new Vector2(randomX, transform.position.y+2f);

            GameObject itemToSpawn = itemPrefab[Random.Range(0,itemPrefab.Length)]; // Default to egg spawn
            itemToSpawn.transform.localScale = Vector3.one;
            

            // Instantiate the item and make it as a child of the spawner
            GameObject item = Instantiate(itemToSpawn, spawnPos, Quaternion.identity,ItemParent.transform);

            // Adjust spawn time after each egg caught (reduce spawn time)
            rate = Mathf.Max(spawnRate*0.5f, spawnRate * (1f - 0.01f*gameManager.GetCount())); // Reduce by 0.01% every item caught
        }
    }
}
