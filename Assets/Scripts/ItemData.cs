using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Food/ItemData")]
public class ItemData : ScriptableObject
{
    public List<Sprite> Sprites;
    public List<int> SortingOrder;
}
