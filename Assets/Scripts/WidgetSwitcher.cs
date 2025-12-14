using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WidgetSwitcher : MonoBehaviour
{
    [SerializeField] private List<UIDocument> _widgets;
    [SerializeField] private int _currentIndex = 0;
    void Start()
    {
        foreach (var widget in _widgets)
        {
            widget.rootVisualElement.style.display = DisplayStyle.None;
        }
        _widgets[_currentIndex].rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void SwitchTo(int index)
    {
        _widgets[_currentIndex].rootVisualElement.style.display = DisplayStyle.None;
        _currentIndex = index;
        _widgets[_currentIndex].rootVisualElement.style.display = DisplayStyle.Flex;
    }
}
