using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class UIButton
{
    public Button Button;
    public List<UIEvent> Events;
}

public class UIButtonMenu : UIAnimatedElement
{
    [Header("Button Menu Elements")]
    public List<UIButton> Buttons;

    private List<UnityEvent> _events = new List<UnityEvent>();
    
    protected override void EventHandlerRegister()
    {
        foreach (var uiButton in Buttons)
        {
            uiButton.Button.onClick.AddListener(() => UIEventDispatcher.TriggerEvents(uiButton.Events, this));
            _events.Add(uiButton.Button.onClick);
        }
    }
    
    protected override void EventHandlerUnRegister()
    {
        foreach (var unityEvent in _events)
        {
            unityEvent.RemoveAllListeners();
        }
        _events.Clear();
    }

    public virtual void HideMenu()
    {
        PlayHideAnimation();
    }
}