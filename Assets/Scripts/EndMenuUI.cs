using UnityEngine;
using UnityEngine.UIElements;

public class EndMenuUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        _uiDocument.rootVisualElement.Q<Button>("ReplayButton").clicked += () => { GameManager.Instance.MainMenu(); };
        _uiDocument.rootVisualElement.Q<Button>("AdButton").clicked += () => { InterstitialAd.Instance.ShowAd(); };
        _uiDocument.rootVisualElement.Q<Label>("ScoreLabel").text = "Score: " + GameManager.Instance.CurrentScore.ToString("000000");
        _uiDocument.rootVisualElement.Q<Label>("HighScoreLabel").text = "High Score: " + GameManager.Instance.HighScore.ToString("000000");
    }

}
