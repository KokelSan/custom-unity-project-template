using System;
using System.Collections.Generic;
using UnityEngine;

public static class UIEventDispatcher
{
        public static void TriggerEvents(List<UIEvent> uiEvents, UIButtonMenu caller)
        {
                foreach (var uiEvent in uiEvents)
                {
                        if (uiEvent.HideMenuOnEvent)
                        {
                                switch (uiEvent.EventExecutionOrder)
                                {
                                        case EventExecutionOrder.HideThenPerformAction:
                                                caller.PlayHideAnimation(() => TriggerEvent(uiEvent,caller));
                                                break;
                                        
                                        case EventExecutionOrder.PerformActionThenHide:
                                                TriggerEvent(uiEvent,caller);
                                                caller.PlayHideAnimation();
                                                break;
                                }
                                
                                continue;
                        }
                        TriggerEvent(uiEvent,caller);
                }
        }
        
        private static void TriggerEvent(UIEvent uiEvent, UIButtonMenu caller)
        {
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
                                UIMenuManagerHandlerData.ShowOptionsMenu(onGoBack);
                                break;
                        
                        case UIEventType.CloseMenu:
                                caller.HideMenu();
                                break;
                        
                        case UIEventType.LoadScene:
                                SceneLoadingService.LoadScene(uiEvent.SceneLoadingData);
                                break;
                       
                        default:
                                Debug.LogWarning($"UI Event {uiEvent.EventType} is not implemented yet, trigger aborted.");
                                break;
                }
        }
}