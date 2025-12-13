using UnityEngine;

public class FoodItem : MonoBehaviour, IInteractable
{
    public ItemData ItemData { get; set; }
    
    void Start()
    {
        if (!ItemData) return;
        for(int i = 0; i < ItemData.Sprites.Count; i++)
        {
            GameObject itemPart = new GameObject("Part_" + i);
            itemPart.transform.SetParent(this.transform);
            SpriteRenderer sr = itemPart.AddComponent<SpriteRenderer>();
            sr.sprite = ItemData.Sprites[i];
            sr.sortingOrder = ItemData.SortingOrder[i];
            itemPart.transform.localPosition = Vector3.zero;
        }
    }

    private void StopDragging()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.PointerMove -= OnMove;
        InputManager.Instance.PointerUp -= StopDragging;
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
}
