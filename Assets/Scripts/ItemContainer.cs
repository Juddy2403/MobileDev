using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemContainer : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _containedItemPrefab;
    [SerializeField] private ItemData _containedItemData;
    private BoxCollider2D _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void OnTouchStart()
    {
        if (!_containedItemPrefab) return;
        var worldPos = InputManager.Instance.WorldPosition;
        var item = Instantiate(_containedItemPrefab, worldPos, Quaternion.identity);
        var foodItem = item.GetComponent<FoodItem>();
        foodItem.AddFoodItem(_containedItemData);
        foodItem.OnTouchStart();
    }

    public void OnTouchEnd(FoodItem foodItem)
    {
        Destroy(foodItem.gameObject);
    }
}