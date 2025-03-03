using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;
public class ItemSpawner : MonoBehaviour
{   
    public GameObject[] itemPrefab; // Egg prefab
    public float spawnRate = 1f; // Start spawn time
    private float rate;
    private GameManager gameManager; // Reference to GameManager
    public GameObject ItemParent;

    public TMP_Text CountdownText;
    public GameObject Countdown;
    public GameObject HUD; 

    void Start()
    {
        gameManager = GameManager.Instance; // Get the GameManager instance
        rate=spawnRate;
        StartCoroutine(StartCountdown()); // Spawn at regular TIME intervals
    }

    IEnumerator StartCountdown()
    {
        for(int i=3; i>0;i--)
        {
            CountdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        CountdownText.text ="Start!";
        yield return new WaitForSeconds(1f);

        Countdown.SetActive(false);
        HUD.SetActive(true);
        StartCoroutine(SpawnItem());
    }

    IEnumerator SpawnItem()
    {
        while (gameManager.GetFlag())
        {
            yield return new WaitForSeconds(rate); //wait for "rate" seconds
            
            GameObject itemToSpawn = GetRandomItem();
            if(itemToSpawn == null) continue;
            
            // Random spawn position within the rectangle's width
            float randomX = Random.Range(transform.position.x-(transform.localScale.x/2),transform.position.x+(transform.localScale.x/2));
            Vector2 spawnPos = new Vector2(randomX, transform.position.y);

            // Instantiate the item and make it as a child of the spawner
            GameObject item = Instantiate(itemToSpawn, spawnPos, Quaternion.identity,ItemParent.transform);
            itemToSpawn.transform.localScale = Vector3.one;

            // Adjust spawn time after each egg caught (reduce spawn time)
            rate = Mathf.Max(spawnRate*0.25f, spawnRate * (1f - 0.1f*gameManager.GetCount())); // Reduce by 0.01% every item caught
        }
    }

    private GameObject GetRandomItem()
    {
        GameObject guaranteedItem = null;
        var possibleItems = new System.Collections.Generic.List<GameObject>();

        foreach (var prefab in itemPrefab)
        {
            Item itemScript = prefab.GetComponent<Item>();
            if(itemScript == null || itemScript.itemData == null) continue;

            int chance = itemScript.itemData.SpawnChance;

            if(chance ==1)
            {
                guaranteedItem = prefab;
            }
            else if (Random.Range(1,chance+1)==1)
            {
                possibleItems.Add(prefab);
            }
        }

        if (possibleItems.Count>0)
        {
            return possibleItems[Random.Range(0,possibleItems.Count)];
        }
        return guaranteedItem;

    }
}
