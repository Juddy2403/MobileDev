using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float _faultsBeforeLose = 3f;
    [SerializeField] private float _faultForUnhappy = 1f;
    [SerializeField] private float _faultForOk = 0.33f;

    private float _currentScore;
    private float _highScore;

    public float CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            if (_currentScore > _highScore) _highScore = _currentScore;
        }
    }

    public float HighScore => _highScore;

    public UnityEvent OnGameOver;
    private float _fault;

    public void OnCustomerOk() => TakeFault(_faultForOk);
    public void OnCustomerUnhappy() => TakeFault(_faultForUnhappy);

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void TakeFault(float amount)
    {
        _fault -= amount;
        if (!(_fault <= 0)) return;
        OnGameOver?.Invoke();
        CurrentScore = ScoreManager.Instance.Score;
        SceneManager.LoadScene("GameOverScene");
    }

    public void StartGame()
    {
        _fault = _faultsBeforeLose;
        SceneManager.LoadScene("MainScene");
    }
}
