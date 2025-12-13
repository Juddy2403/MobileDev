using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : MonoBehaviour, IInteractable
{
    [SerializeField] private float _cookingTime = 2f;
    private FoodItem _foodItem;
    public void OnTouchStart()
    {
        
    }

    public void OnTouchEnd(FoodItem foodItem)
    {
        if (foodItem.ItemList.Count is > 1 or 0) return;
        if (foodItem.ItemList[0]?.NextCookingStage == null) return;
        _foodItem = foodItem;
        _foodItem.OnPickedUp += OnActivePickedUp;
        StartCoroutine(nameof(CookItem));
    }

    IEnumerator CookItem()
    {
        while (_foodItem.ItemList[0]?.NextCookingStage != null)
        {
            yield return new WaitForSeconds(_cookingTime);
            _foodItem.CookItem();
        }
    }
    
    private void OnActivePickedUp()
    {
        StopCoroutine(nameof(CookItem));
    }
    
}
