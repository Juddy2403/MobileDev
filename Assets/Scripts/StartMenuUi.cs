using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUi : MonoBehaviour
{
    private UIDocument _uiDocument;
    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _uiDocument.rootVisualElement.Q<Button>("PlayButton").clicked += () => { GameManager.Instance.StartGame(); };
        _uiDocument.rootVisualElement.Q<Label>("MoneyLabel").text = GameManager.Instance.TotalMoney.ToString() + " $";
    }
    
}
