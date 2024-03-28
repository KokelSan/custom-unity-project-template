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
        
    [ShowIf(nameof(EventType), UIEventType.ShowOptionsMenu)]
    public bool ShowBackWhenOptionsMenuHidden;

    [ShowIf(nameof(EventType), UIEventType.LoadScene)]
    public SceneLoadingData SceneLoadingData;
        
    [HideIf(nameof(EventType), UIEventType.None)]
    [HideIf(nameof(EventType), UIEventType.CloseMenu)]
    [HideIf(nameof(EventType), UIEventType.ExitGame)]
    public bool HideMenuOnEvent;
}