﻿using System.Collections.Generic;

public static class UISceneTransitionsTableUtils
{
        public static ScreenTransitionType GetTransition(UISceneTransitionsTableSO table, int from, int to)
        {
                if (TryGetSpecificTransition(table.SpecificTransitions, from, to, out ScreenTransitionType transitionType))
                {
                        return transitionType;
                }
                
                if (TryGetGlobalTransition(table.GlobalFromTransitions, from, out transitionType))
                {
                        return transitionType;
                }
                
                if (TryGetGlobalTransition(table.GlobalToTransitions, to, out transitionType))
                {
                        return transitionType;
                }

                return table.DefaultTransition;
        }
        
        private static bool TryGetSpecificTransition(List<SpecificTransition> transitions, int from, int to, out ScreenTransitionType transitionType)
        {
                foreach (SpecificTransition transition in transitions)
                {
                        if (transition.FromSceneIndex == from && transition.ToSceneIndex == to)
                        {
                                transitionType = transition.TransitionType;
                                return true;
                        }
                }
                transitionType = default;
                return false;
        }

        private static bool TryGetGlobalTransition(List<GlobalTransition> transitions, int sceneIndex, out ScreenTransitionType transitionType)
        {
                foreach (GlobalTransition transition in transitions)
                {
                        if (transition.SceneIndex == sceneIndex)
                        {
                                transitionType = transition.TransitionType;
                                return true;
                        }
                }
                transitionType = default;
                return false;
        }
}