using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : MonoBehaviour, IInteractable
{
    [SerializeField] private float _cookingTime = 2f;

    // Track cooking coroutines per food item
    private Dictionary<FoodItem, Coroutine> _activeCooking = new();

    public void OnTouchStart() { }

    public void OnTouchEnd(FoodItem foodItem)
    {
        // Only allow single-item food stacks
        if (foodItem.ItemList.Count != 1) return;

        if (foodItem.ItemList[0]?.NextCookingStage == null) return;

        // Prevent starting twice
        if (_activeCooking.ContainsKey(foodItem)) return;

        foodItem.OnPickedUp += () => OnFoodPickedUp(foodItem);

        Coroutine cookRoutine = StartCoroutine(CookItem(foodItem));
        _activeCooking.Add(foodItem, cookRoutine);
    }

    IEnumerator CookItem(FoodItem foodItem)
    {
        while (foodItem.ItemList[0]?.NextCookingStage)
        {
            yield return new WaitForSeconds(_cookingTime);
            foodItem.CookItem();
        }

        Cleanup(foodItem);
    }

    private void OnFoodPickedUp(FoodItem foodItem)
    {
        if (_activeCooking.TryGetValue(foodItem, out var routine))
        {
            StopCoroutine(routine);
            Cleanup(foodItem);
        }
    }

    private void Cleanup(FoodItem foodItem)
    {
        foodItem.OnPickedUp -= () => OnFoodPickedUp(foodItem);
        _activeCooking.Remove(foodItem);
    }
}