using UnityEngine;

public interface IInteractable
{
    void OnTouchStart();
    void OnTouchEnd(FoodItem foodItem);
}
