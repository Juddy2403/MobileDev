using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ScoreManager : Singleton<ScoreManager>
{
    [SerializeField] private int _onCustomerHappyScore = 100;
    [SerializeField] private int _onCustomerOkScore = 25;
    [SerializeField] private int _onCustomerUpsetScore = -15;
    public int Score { get; private set; } = 0;
    private TextMeshProUGUI _scoreText;

    public void OnCustomerHappy() => AddScore(_onCustomerHappyScore);
    public void OnCustomerOk() => AddScore(_onCustomerOkScore);
    public void OnCustomerUnhappy() => AddScore(_onCustomerUpsetScore);

    private void Start()
    {
        _scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
    }
    
    private void AddScore(int amount)
    {
        Score += amount;
        _scoreText.text = Score.ToString();
    }
}
