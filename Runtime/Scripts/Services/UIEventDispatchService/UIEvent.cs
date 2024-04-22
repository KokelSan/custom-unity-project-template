using System;
using KokelSan.CustomAttributes;

public enum UIEventType
{
    None,
    StartGame,
    PauseGame,
    ResumeGame,
    StopGame,
    ExitGame,
    ShowOptionsMenu,
    CloseMenu,
    LoadScene
}

public enum EventExecutionOrder
{
    HideThenPerformAction,
    PerformActionThenHide,
}

[Serializable]
public class UIEvent
{
    public UIEventType EventType;
    
    [HideIf(nameof(EventType), UIEventType.None)]
    [HideIf(nameof(EventType), UIEventType.CloseMenu)]
    [HideIf(nameof(EventType), UIEventType.ExitGame)]
    public bool HideMenuOnEvent;

    [ShowIf(nameof(HideMenuOnEvent))]
    public EventExecutionOrder EventExecutionOrder;
    
    // ShowOptionsMenu parameters
    [ShowIf(nameof(EventType), UIEventType.ShowOptionsMenu)]
    [ShowIf(nameof(HideMenuOnEvent))]
    public bool ShowBackWhenOptionsMenuHidden;
    
    // LoadScene parameters
    [ShowIf(nameof(EventType), UIEventType.LoadScene)]
    public SceneLoadingData SceneLoadingData;
}