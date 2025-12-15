using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WantedItem
{
    HotDog,
    Fries,
    Drink,
    Burger
}

[System.Serializable]
public class WantedFoodPair
{
    public WantedItem itemType;
    public List<ItemData> itemData;
}

public class Customer : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Sprite> _charSprites;
    [SerializeField] private List<WantedFoodPair> _foodItems;
    [SerializeField] private FoodItem _requestedFoodItem;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Image _waitingBar;
    [SerializeField] private float _waitingTime = 15f;
    private float _timeElapsed = 0f;
    public Vector2? Target { get; set; } = null;
    public event System.Action<Customer> OnLeaving;
    public event System.Action OnCustomerHappy;
    public event System.Action OnCustomerOk;
    public event System.Action OnCustomerUnhappy;
    private bool _left = false;

    void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = _charSprites[Random.Range(0, _charSprites.Count)];

        int itemCount = System.Enum.GetValues(typeof(WantedItem)).Length;
        WantedItem randomItem = (WantedItem)Random.Range(0, itemCount - 1);
        if (randomItem == WantedItem.HotDog && GameManager.Instance.ActiveStandIdx == 1) randomItem = WantedItem.Burger;
        foreach (var item in _foodItems)
        {
            if (item.itemType != randomItem) continue;
            if (randomItem is WantedItem.HotDog or WantedItem.Burger)
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
            else _requestedFoodItem.AddFoodItem(item.itemData[0]);

            break;
        }
    }

    void Update()
    {
        UpdateWaitingBar();
        MoveTowardsTarget();
    }

    private void UpdateWaitingBar()
    {
        if (_left || Target.HasValue) return;
        _timeElapsed += Time.deltaTime;
        _waitingBar.fillAmount = (_waitingTime - _timeElapsed) / _waitingTime;
        if (_timeElapsed >= _waitingTime)
        {
            Leave();
            OnCustomerUnhappy?.Invoke();
        }
    }

    private void MoveTowardsTarget()
    {
        if (!Target.HasValue) return;
        transform.position = Vector2.MoveTowards(
            transform.position,
            Target.Value,
            _speed * Time.deltaTime
        );
        if (Vector2.Distance(transform.position, Target.Value) <= 0.01f)
        {
            Target = null;
            if (_left) Destroy(gameObject);
        }
    }

    public void OnTouchStart()
    {
    }

    public void OnTouchEnd(FoodItem foodItem)
    {
        if (_left) return;
        int itemsGotRight = 0;
        foreach (var item in _requestedFoodItem.ItemList)
        {
            if (foodItem.ItemList.Contains(item)) itemsGotRight++;
        }

        if (itemsGotRight == _requestedFoodItem.ItemList.Count &&
            _requestedFoodItem.ItemList.Count == foodItem.ItemList.Count)
        {
            if (_timeElapsed * 3f / 2f >= _waitingTime) OnCustomerOk?.Invoke();
            else OnCustomerHappy?.Invoke();
        }
        else if (itemsGotRight >= 2)
        {
            OnCustomerOk?.Invoke();
        }
        else
        {
            OnCustomerUnhappy?.Invoke();
        }

        Leave();
        Destroy(foodItem.gameObject);
    }

    private void Leave()
    {
        OnLeaving?.Invoke(this);
        Target = transform.position + Vector3.left * 15f;
        _left = true;
    }
}