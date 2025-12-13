using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemContainer : MonoBehaviour
{
    [SerializeField] private GameObject _containedItemPrefab;
    [SerializeField] private ItemData _containedItemData;
    private BoxCollider2D _collider;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        InputManager.Instance.PointerDown += OnPointerDown;
    }

    private void OnDisable()
    {
        if(InputManager.Instance != null) InputManager.Instance.PointerDown -= OnPointerDown;
    }

    private void OnPointerDown()
    {
        var worldPos = InputManager.Instance.WorldPosition;
        if (!_collider.OverlapPoint(worldPos)) return;
        Debug.Log("ItemContainer: OnPointerDown");
        var item = Instantiate(_containedItemPrefab, worldPos, Quaternion.identity);
        var foodItem = item.GetComponent<FoodItem>();
        foodItem.ItemData = _containedItemData;
        foodItem.StartDragging();
    }

}