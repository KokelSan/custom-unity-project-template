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

[Serializable]
public class UIEvent
{
    public UIEventType EventType;
    
    [HideIf(nameof(EventType), UIEventType.None)]
    [HideIf(nameof(EventType), UIEventType.CloseMenu)]
    [HideIf(nameof(EventType), UIEventType.ExitGame)]
    public bool HideMenuOnEvent;

    [ShowIf(nameof(HideMenuOnEvent))]
    public bool HideInstant;
    
    // ShowOptionsMenu parameters
    [ShowIf(nameof(EventType), UIEventType.ShowOptionsMenu)]
    [ShowIf(nameof(HideMenuOnEvent))]
    public bool ShowBackWhenOptionsMenuHidden;
    
    [ShowIf(nameof(ShowBackWhenOptionsMenuHidden))]
    public bool ShowBackInstant;
    
    // LoadScene parameters
    [ShowIf(nameof(EventType), UIEventType.LoadScene)]
    public SceneLoadingData SceneLoadingData;
}