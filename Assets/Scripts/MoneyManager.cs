using TMPro;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private int _onCustomerHappyIncome = 10;
    [SerializeField] private int _onCustomerOkIncome = 5;
    private int _money = 0;
    public int Money => _money;
    private TextMeshProUGUI _cashText;

    public void OnCustomerHappy() => AddMoney(_onCustomerHappyIncome);
    public void OnCustomerOk() => AddMoney(_onCustomerOkIncome);
    
    private void Start()
    {
        _cashText = GameObject.Find("MoneyText").GetComponent<TextMeshProUGUI>();
    }
    
    private void AddMoney(int amount)
    {
        _money += amount;
        _cashText.text = _money.ToString() + " $";
    }
}
