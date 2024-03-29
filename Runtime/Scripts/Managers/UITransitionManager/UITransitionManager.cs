﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class UITransitionManager : BaseBehaviour
{
    public UISceneTransitionsTableSO TransitionsTableSO;
    public List<UITransition> Transitions;

    private Dictionary<TransitionType, UITransition> _transitionsDict = new Dictionary<TransitionType, UITransition>();
    private UITransition _pendingTransition;
    
    protected override void EventHandlerRegister()
    {
        UITransitionManagerHandlerData.OnShowSceneTransition += ShowSceneTransition;
        UITransitionManagerHandlerData.OnShowTransition += ShowTransition;
        UITransitionManagerHandlerData.OnPlayFullTransition += PlayFullTransition;
        UITransitionManagerHandlerData.OnHideTransition += HideTransition;
    }

    protected override void EventHandlerUnRegister()
    {
        UITransitionManagerHandlerData.OnShowSceneTransition -= ShowSceneTransition;
        UITransitionManagerHandlerData.OnShowTransition -= ShowTransition;
        UITransitionManagerHandlerData.OnPlayFullTransition -= PlayFullTransition;
        UITransitionManagerHandlerData.OnHideTransition -= HideTransition;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        _transitionsDict.Clear();
        foreach (UITransition transition in Transitions)
        {
            if (transition == null)
            {
                Debug.LogWarning($"An element of the 'Transitions' list is null");
                continue;
            }
            
            if (!_transitionsDict.TryAdd(transition.TransitionType, transition))
            {
                Debug.LogWarning($"Transition with type {transition.TransitionType} has multiple instance, only the first one will be used.");
            }
        }
    }

    private void ShowSceneTransition(int fromScene, int toScene, Action onAnimationCompleted)
    {
        TransitionType transitionType = UISceneTransitionsTableUtils.GetTransition(TransitionsTableSO, fromScene, toScene);
        ShowTransition(transitionType, onAnimationCompleted);
    }

    private void ShowTransition(TransitionType transitionType, Action onAnimationCompleted)
    {
        if (_pendingTransition != null)
        {
            Debug.LogWarning($"Trying to show {transitionType} transition while {_pendingTransition.TransitionType} is pending");
            return;
        }
        
        if (_transitionsDict.TryGetValue(transitionType, out UITransition transition))
        {
            transition.PlayShowAnimation(onAnimationCompleted);
            _pendingTransition = transition;
            return;
        }
        Debug.LogWarning("Trying to show an unregistered transition");
    }

    private void PlayFullTransition(TransitionType transitionType, Action onTransitionCompleted)
    {
        ShowTransition(transitionType, OnAnimationCompleted);
        void OnAnimationCompleted ()
        {
            HideTransition(onTransitionCompleted);
        }
    }
    
    private void HideTransition(Action onAnimationCompleted)
    {
        if (_pendingTransition != null)
        {
            _pendingTransition.PlayHideAnimation(OnAnimationCompleted);
            void OnAnimationCompleted()
            {
                onAnimationCompleted?.Invoke();   
                _pendingTransition = null;
            }
            return;
        }
        Debug.LogWarning("Trying to hide a not-shown transition");
    }
}