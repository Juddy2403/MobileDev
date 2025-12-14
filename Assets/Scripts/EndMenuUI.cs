using UnityEngine;
using UnityEngine.UIElements;

public class EndMenuUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _uiDocument.rootVisualElement.Q<Button>("ReplayButton").clicked += () => { GameManager.Instance.StartGame(); };
        _uiDocument.rootVisualElement.Q<Button>("AdButton").clicked += () => { /* play ad*/ };
        _uiDocument.rootVisualElement.Q<Label>("ScoreLabel").text = "Score: " + GameManager.Instance.CurrentScore.ToString("000000");
        _uiDocument.rootVisualElement.Q<Label>("HighScoreLabel").text = "Score: " + GameManager.Instance.HighScore.ToString("000000");
    }

}
