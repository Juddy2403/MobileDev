using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float _faultsBeforeLose = 3f;
    [SerializeField] private float _faultForUnhappy = 1f;
    [SerializeField] private float _faultForOk = 0.33f;

    private int _currentScore;
    private int _highScore;
    private int _totalMoney;
    private EndMenuUI _endMenuUI;

    public int TotalMoney
    {
        get => _totalMoney;
        set
        {
            _totalMoney = value;
            PlayerPrefs.SetInt("TotalMoney", _totalMoney);
        }
    }
    
    public int CurrentScore
    {
        get => _currentScore;
        set
        {
            _currentScore = value;
            if (_currentScore > _highScore)
            {
                _highScore = _currentScore;
                PlayerPrefs.SetInt("HighScore", HighScore);
            }
        }
    }

    public int HighScore => _highScore;

    public UnityEvent OnGameOver;
    private float _fault;

    public void OnCustomerOk() => TakeFault(_faultForOk);
    public void OnCustomerUnhappy() => TakeFault(_faultForUnhappy);

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _highScore = PlayerPrefs.GetInt("HighScore", 0);
        TotalMoney = PlayerPrefs.GetInt("TotalMoney", 0);
    }

    private void TakeFault(float amount)
    {
        _fault -= amount;
        if (!(_fault <= 0)) return;
        OnGameOver?.Invoke();
        CurrentScore = ScoreManager.Instance.Score;
        TotalMoney += MoneyManager.Instance.Money;
        _endMenuUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
        InterstitialAd.Instance.OnAdCompleted.AddListener(ContinueGame);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        _fault = _faultsBeforeLose;
        SceneManager.LoadScene("MainScene");
        StartCoroutine(FindEndUI());
    }

    private void ContinueGame()
    {
        Time.timeScale = 1f;
        // Money will be added back when the game is lost and not continued
        TotalMoney -= MoneyManager.Instance.Money;
        _fault = _faultsBeforeLose / 2f;
        _endMenuUI?.gameObject.SetActive(false);
        InterstitialAd.Instance.OnAdCompleted.RemoveListener(ContinueGame);
    }

    private IEnumerator FindEndUI()
    {
        yield return null;
        yield return null;
        _endMenuUI = FindFirstObjectByType<EndMenuUI>();
        if (!_endMenuUI)
        {
            Debug.LogError("EndMenuUI not found in the scene.");
        }
        else
        {
            _endMenuUI.gameObject.SetActive(false);
        }
    }
}