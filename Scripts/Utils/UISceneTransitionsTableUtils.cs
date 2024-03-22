using System.Collections.Generic;
using UnityEngine;

public static class UISceneTransitionsTableUtils
{
        public static ScreenTransitionType GetTransition(UISceneTransitionsTableSO table, int from, int to)
        {
                // specific transition
                if (TryGetSpecificTransition(table.SpecificTransitions, from, to, out ScreenTransitionType transitionType))
                {
                        Debug.Log($"Transition from {from} to {to} is {transitionType}");
                        return transitionType;
                }
                
                // global FROM transition
                if (TryGetGlobalTransition(table.GlobalFromTransitions, from, out transitionType))
                {
                        Debug.Log($"Transition from {from} is {transitionType}");
                        return transitionType;
                }
                
                //  global TO transition
                if (TryGetGlobalTransition(table.GlobalToTransitions, to, out transitionType))
                {
                        Debug.Log($"Transition to {to} is {transitionType}");
                        return transitionType;
                }

                Debug.Log($"Default transition ({table.DefaultTransition}) applied from {from} to {to}");
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