using UnityEngine;
using UnityEngine.UIElements;

public class EndMenuUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _adButton;
    bool _playedAd = false;
    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _uiDocument.rootVisualElement.Q<Button>("ReplayButton").clicked += () => { GameManager.Instance.MainMenu(); };
        _adButton = _uiDocument.rootVisualElement.Q<Button>("AdButton");
        if (_playedAd) _adButton.visible = false;
        else _adButton.clicked += OnAdButtonClicked;
        _uiDocument.rootVisualElement.Q<Label>("ScoreLabel").text = "Score: " + GameManager.Instance.CurrentScore.ToString("000000");
        _uiDocument.rootVisualElement.Q<Label>("HighScoreLabel").text = "High Score: " + GameManager.Instance.HighScore.ToString("000000");
    }

    void OnAdButtonClicked()
    {
        InterstitialAd.Instance.ShowAd();
        _playedAd = true;
        _adButton.clicked -= OnAdButtonClicked;
    }

}
