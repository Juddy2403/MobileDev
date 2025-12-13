using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class FoodItem : MonoBehaviour, IInteractable
{
    public event System.Action<FoodItem> OnPickedUp;
    public List<ItemData> ItemList => _itemList;
    private List<ItemData> _itemList = new List<ItemData>();

    public void AddFoodItem(ItemData itemData)
    {
        if (!itemData) return;
        ItemList.Add(itemData);
        for(int i = 0; i < itemData.Sprites.Count; i++)
        {
            GameObject itemPart = new GameObject(itemData.name);
            itemPart.transform.SetParent(this.transform, false);
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
        OnPickedUp?.Invoke(this);
    }

    public void OnTouchEnd(FoodItem foodItem)
    {
        // One at a time
        if (foodItem.ItemList.Count is > 1 or 0) return;
        // No duplicates
        if(ItemList.Any(item => item == foodItem.ItemList[0])) return;
        // Items are ready to be combined
        if (!foodItem.ItemList[0].IsReadyToBeCombined) { return; }
        if(ItemList.Any(item => !item.IsReadyToBeCombined)) return;

        foreach (var item in foodItem.ItemList)
        {
            AddFoodItem(item);
        }
        Destroy(foodItem.gameObject);
    }

    public void CookItem()
    {
        if (ItemList.Count is > 1 or 0) return;
        ItemData nextCookingStage = ItemList[0].NextCookingStage;
        if (!nextCookingStage) return;
        Destroy(transform.GetChild(0).gameObject);
        ItemList.Clear();
        AddFoodItem(nextCookingStage);
    }
    
    IEnumerator Cooking()
    {
        while (ItemList[0]?.NextCookingStage)
        {
            yield return new WaitForSeconds(ItemList[0].CookTime);
            CookItem();
        }
    }

    public void StartCooking()
    {
        StartCoroutine(Cooking());
    }
    
    public void StopCooking()
    {
        StopCoroutine(Cooking());
    }
}
