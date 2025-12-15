using System;
using System.Collections.Generic;
using Dan.Main;
using Dan.Models;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardManager : Singleton<LeaderboardManager>
{
    private UIDocument _uiDocument;
    private TextField _usernameField;
    private Label _usernameLabel;
    private Button _leaderboardBtn;

    private void Start()
    {
        LoadEntries();
        _uiDocument = GetComponent<UIDocument>();
        _usernameField = _uiDocument.rootVisualElement.Q<TextField>("UsernameInput");
        _usernameLabel = _uiDocument.rootVisualElement.Q<Label>("UsernameLabel");
        _leaderboardBtn = _uiDocument.rootVisualElement.Q<Button>("EnterLeaderboardButton");
        if (GameManager.Instance.Username != "")
        {
            _usernameLabel.text = "Username: " + GameManager.Instance.Username;
            _usernameField.visible = false;
            _leaderboardBtn.visible = false;
            UploadEntry(GameManager.Instance.Username, GameManager.Instance.HighScore);
        }
        else
        {
            _leaderboardBtn.clicked += OnEnterLeaderboard;
            _usernameLabel.visible = false;
        }

        _uiDocument.rootVisualElement.Q<Label>("HighScoreLabel").text =
            "High Score: " + GameManager.Instance.HighScore.ToString("000000");
        _uiDocument.rootVisualElement.Q<Button>("BackButton").clicked += OnBackButton;
    }

    private void OnBackButton()
    {
        WidgetSwitcher widgetSwitcher = FindFirstObjectByType<WidgetSwitcher>();
        if(!widgetSwitcher) return;
        widgetSwitcher.SwitchTo(0);
    }

    private void OnEnterLeaderboard()
    {
        var username = _usernameField.text;
        if (string.IsNullOrEmpty(username)) return;
        
        _leaderboardBtn.visible = false;
        _leaderboardBtn.clicked -= OnEnterLeaderboard;
        
        GameManager.Instance.Username = username;
        _usernameLabel.text = "Username: " + username;
        _usernameLabel.visible = true;
        _usernameField.visible = false;
        UploadEntry(username, GameManager.Instance.HighScore);
    }

    private void LoadEntries()
    {
        Leaderboards.GameLeaderboard.GetEntries(entries =>
        {
            GroupBox groupBox = _uiDocument.rootVisualElement.Q<GroupBox>("LeaderboardGroupBox");

            List<Label> labels = groupBox.Query<Label>().ToList();

            foreach (var t in labels)
                t.text = "Username: 000000";

            var length = Mathf.Min(labels.Count, entries.Length);
            for (int i = 0; i < length; i++)
                labels[i].text = $"{entries[i].Username}: {entries[i].Score:000000}";
        });
    }

    private void UploadEntry(string username, int score)
    {
        Leaderboards.GameLeaderboard.UploadNewEntry(username, score, isSuccessful =>
        {
            if (isSuccessful) LoadEntries();
        });
    }
}