using TMPro;
using UnityEngine;

public class MoneyManager : Singleton<MoneyManager>
{
    [SerializeField] private TextMeshProUGUI _cashText;
    [SerializeField] private int _onCustomerHappyIncome = 10;
    [SerializeField] private int _onCustomerOkIncome = 5;
    private int _money = 0;

    public void OnCustomerHappy() => AddMoney(_onCustomerHappyIncome);
    public void OnCustomerOk() => AddMoney(_onCustomerOkIncome);
    
    private void AddMoney(int amount)
    {
        _money += amount;
        _cashText.text = _money.ToString() + " $";
    }
}
