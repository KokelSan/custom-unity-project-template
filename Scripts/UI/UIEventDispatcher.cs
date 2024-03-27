using System;
using System.Collections.Generic;
using KokelSan.CustomAttributes;
using UnityEngine;

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

public static class UIEventDispatcher
{
        public static void TriggerEvents(List<UIEvent> uiEvents, UIButtonMenu caller, object value = null)
        {
                foreach (var uiEvent in uiEvents)
                {
                        TriggerEvent(uiEvent,caller,value);
                }
        }
        
        private static void TriggerEvent(UIEvent uiEvent, UIButtonMenu caller, object value)
        {
                if (uiEvent.HideMenuOnEvent)
                {
                        caller.PlayHideAnimation();
                }
                
                switch (uiEvent.EventType)
                {
                        case UIEventType.StartGame:
                                GameManagerHandlerData.StartGame();
                                break;
                        
                        case UIEventType.PauseGame:
                                GameManagerHandlerData.PauseGame();
                                break;
                        
                        case UIEventType.ResumeGame:
                                GameManagerHandlerData.ResumeGame();
                                break;
                        
                        case UIEventType.StopGame:
                                GameManagerHandlerData.StopGame();
                                break;
                        
                        case UIEventType.ExitGame:
                                GameManagerHandlerData.ExitGame();
                                break;
                        
                        case UIEventType.ShowOptionsMenu:
                                Action onGoBack = uiEvent.ShowBackWhenOptionsMenuHidden ? () => caller.PlayShowAnimation() : null;
                                UIMenuServiceHandlerData.ShowOptionsMenu(onGoBack);
                                break;
                        
                        case UIEventType.CloseMenu:
                                caller.HideMenu();
                                break;
                        
                        case UIEventType.LoadScene:
                                SceneLoadingServiceHandlerData.LoadScene(uiEvent.SceneLoadingData);
                                break;
                       
                        default:
                                Debug.LogWarning($"UI Event {uiEvent.EventType} is not implemented yet, trigger aborted.");
                                break;
                }
        }
}