using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuUi : MonoBehaviour
{
    private UIDocument _uiDocument;

    void Start()
    {
        _uiDocument = GetComponent<UIDocument>();
        _uiDocument.rootVisualElement.Q<Button>("PlayButton").clicked += () => { GameManager.Instance.StartGame(); };
        _uiDocument.rootVisualElement.Q<Button>("LeaderboardButton").clicked += OnLeaderboardButton;
        _uiDocument.rootVisualElement.Q<Label>("MoneyLabel").text = GameManager.Instance.TotalMoney.ToString() + " $";
        var boughtBurgerStand = GameManager.Instance.BoughtBurgerStand;
        if (boughtBurgerStand)
        {
            _uiDocument.rootVisualElement.Q<GroupBox>("BuyStandGroup").visible = false;
        _uiDocument.rootVisualElement.Q<ToggleButtonGroup>("StandToggle").value.ResetAllOptions();
        }
        else
        {
            _uiDocument.rootVisualElement.Q<ToggleButtonGroup>("StandToggle").visible = false;
            _uiDocument.rootVisualElement.Q<Button>("BuyBurgerBtn").clicked += OnBuyBurger;
        }
    }

    private void OnBuyBurger()
    {
        if (GameManager.Instance.TotalMoney < 10) return;
        GameManager.Instance.TotalMoney -= 10;
        _uiDocument.rootVisualElement.Q<Label>("MoneyLabel").text = GameManager.Instance.TotalMoney.ToString() + " $";
        GameManager.Instance.BoughtBurgerStand = true;
        _uiDocument.rootVisualElement.Q<GroupBox>("BuyStandGroup").visible = false;
        _uiDocument.rootVisualElement.Q<ToggleButtonGroup>("StandToggle").visible = true;
        _uiDocument.rootVisualElement.Q<ToggleButtonGroup>("StandToggle").value.ResetAllOptions();
       
    }

    public int GetActiveStand()
    {
        var toggleValue = _uiDocument.rootVisualElement.Q<ToggleButtonGroup>("StandToggle").value;
        var activeOptions = toggleValue.GetActiveOptions(stackalloc int[toggleValue.length]);
        Debug.Log(activeOptions[0]);
        return activeOptions[0];
    }

    private void OnLeaderboardButton()
    {
        WidgetSwitcher widgetSwitcher = FindFirstObjectByType<WidgetSwitcher>();
        if (!widgetSwitcher) return;
        widgetSwitcher.SwitchTo(1);
    }
}