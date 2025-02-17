using UnityEngine;

[CreateAssetMenu(fileName = "NewCatchItem", menuName = "Catch Item")]
public class CatchItem : ScriptableObject
{
    public float fallSpeed = 2f;
    public int scoreValue = 1;
    public Sprite objectSprite;
}
