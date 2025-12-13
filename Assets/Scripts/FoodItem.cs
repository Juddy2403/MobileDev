using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour, IInteractable
{
    private List<ItemData> _itemList = new List<ItemData>();
    private int _partCounter = 0;

    public void AddFoodItem(ItemData itemData)
    {
        if (!itemData) return;
        _itemList.Add(itemData);
        for(int i = 0; i < itemData.Sprites.Count; i++)
        {
            GameObject itemPart = new GameObject("Part_" + _partCounter++);
            itemPart.transform.SetParent(this.transform);
            SpriteRenderer sr = itemPart.AddComponent<SpriteRenderer>();
            sr.sprite = itemData.Sprites[i];
            sr.sortingOrder = itemData.SortingOrder[i];
            itemPart.transform.localPosition = Vector3.zero;
        }
    }

    private void StopDragging()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.PointerMove -= OnMove;
        InputManager.Instance.PointerUp -= StopDragging;
        // Interact with the object under when letting go
        var interactable = InputManager.Instance.GetInteractableUnderObject(gameObject);
        interactable?.OnTouchEnd(this);
    }

    private void OnMove(Vector2 obj)
    {
        transform.position = obj;
    }

    public void OnTouchStart()
    {
        InputManager.Instance.PointerMove += OnMove;
        InputManager.Instance.PointerUp += StopDragging;
    }

    public void OnTouchEnd(FoodItem foodItem)
    {
        // try to combine
    }
}
