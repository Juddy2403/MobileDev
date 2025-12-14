using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : MonoBehaviour, IInteractable
{
    public void OnTouchStart() { }

    public void OnTouchEnd(FoodItem foodItem)
    {
        // Only allow single-item food stacks
        if (foodItem.ItemList.Count != 1) return;
        if (foodItem.ItemList[0]?.NextCookingStage == null) return;

        foodItem.OnPickedUp += OnFoodPickedUp;

        foodItem.StartCooking();
    }

    private void OnFoodPickedUp(FoodItem foodItem)
    {
        foodItem.StopCooking();
        foodItem.OnPickedUp -= OnFoodPickedUp;
    }

}