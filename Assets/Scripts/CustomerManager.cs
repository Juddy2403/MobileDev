using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : Singleton<CustomerManager>
{
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private float _spaceBetween = 1f;
    [SerializeField] private int _maxCustomers = 3;
    [SerializeField] private Transform _customerStartPosition;
    [SerializeField] private Transform _customerEndPosition;
    private List<GameObject> _customers;

    private void Start()
    {
        _customers = new List<GameObject>();
        AddCustomer();
    }

    private void AddCustomer()
    {
        var newCustomer = Instantiate(_customerPrefab, _customerStartPosition.position, Quaternion.identity);
        var customer = newCustomer.GetComponent<Customer>();
        customer.OnLeaving += UpdateCustomersTargets;
        customer.Target = _customerEndPosition.position + Vector3.right * _spaceBetween * _customers.Count;
        _customers.Add(newCustomer);
        customer.OnCustomerHappy += OnCustomerHappy;
        customer.OnCustomerOk += OnCustomerOk;
        if (_customers.Count < _maxCustomers) Invoke(nameof(AddCustomer), _spawnInterval);
    }

    private void OnCustomerOk()
    {
        MoneyManager.Instance.OnCustomerOk();
    }

    private void OnCustomerHappy()
    {
        MoneyManager.Instance.OnCustomerHappy();
    }

    private void UpdateCustomersTargets(Customer obj)
    {
        obj.OnLeaving -= UpdateCustomersTargets;
        Customer customer = obj;
        customer.OnCustomerHappy -= OnCustomerHappy;
        customer.OnCustomerOk -= OnCustomerOk;
        _customers.Remove(obj.gameObject);
        
        for(int i = 0; i < _customers.Count; i++)
            _customers[i].GetComponent<Customer>().Target = _customerEndPosition.position + Vector3.right * _spaceBetween * i;
        
        CancelInvoke(nameof(AddCustomer));
        if (_customers.Count == 0) AddCustomer();
        else Invoke(nameof(AddCustomer), _spawnInterval);
    }
}