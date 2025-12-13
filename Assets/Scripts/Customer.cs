using System.Collections.Generic;
using UnityEngine;

public enum WantedItem
{
    HotDog,
    Fries,
    Drink
}
[System.Serializable]
public class WantedFoodPair
{
    public WantedItem itemType;
    public List<ItemData> itemData;
}
public class Customer : MonoBehaviour
{

    [SerializeField] private List<WantedFoodPair> _foodItems;
    [SerializeField] private FoodItem _requestedFoodItem;

    void Start()
    {
        int itemCount = System.Enum.GetValues(typeof(WantedItem)).Length;
        WantedItem randomItem = (WantedItem) Random.Range(0, itemCount);
        foreach (var item in _foodItems)
        {
            if (item.itemType != randomItem) continue;
            if (randomItem == WantedItem.HotDog)
            {
                for (int i = 0; i < item.itemData.Count; i++)
                {
                    if (i < 2) _requestedFoodItem.AddFoodItem(item.itemData[i]);
                    else
                    {
                        bool randomChance = Random.Range(0, 100) < 50;
                        if (randomChance) _requestedFoodItem.AddFoodItem(item.itemData[i]);
                    }
                }
            }
            else
            {
                _requestedFoodItem.AddFoodItem(item.itemData[0]);
            }
            break;
        }
        
    }

}