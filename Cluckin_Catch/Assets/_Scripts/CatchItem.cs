using UnityEngine;

[CreateAssetMenu(fileName = "NewCatchItem", menuName = "Catch Item")]
public class CatchItem : ScriptableObject
{
    public string itemName;
    public float fallSpeed = 2f;
    public int scoreValue = 1;
    public Sprite objectSprite;
    public Sprite brokenSprite;
    public int SpawnChance=1; // 1 means 1 in 1 chance, 7 means 1 in 7 chance, 10 means 1 in 10, so on..
    public bool Bonus = false;
    public int Mult=1;
}
