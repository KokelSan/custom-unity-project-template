using System.Collections.Generic;

public static class UISceneTransitionsTableUtils
{
        public static ScreenTransitionType GetTransition(UISceneTransitionsTableSO table, int from = -1, int to = -1)
        {
                // specific transition
                if (from >= 0 && to >= 0)
                {
                        if (TryGetSpecificTransition(table.SpecificTransitions, from, to, out ScreenTransitionType transitionType))
                        {
                                return transitionType;
                        }
                }
                
                // global FROM transition
                else if (from >= 0)
                {
                        if (TryGetGlobalTransition(table.GlobalFromTransitions, from, out ScreenTransitionType transitionType))
                        {
                                return transitionType;
                        }
                }
                
                //  global TO transition
                else
                {
                        if (TryGetGlobalTransition(table.GlobalToTransitions, to, out ScreenTransitionType transitionType))
                        {
                                return transitionType;
                        }
                }

                return table.DefaultTransition;
        }
        
        private static bool TryGetSpecificTransition(List<SpecificTransition> transitions, int from, int to, out ScreenTransitionType transitionType)
        {
                SpecificTransition transition = transitions.Find(transition => transition.FromSceneIndex == from && transition.ToSceneIndex == to);
                if (transition.Equals(default))
                {
                        transitionType = default;
                        return false;
                }
                
                transitionType = transition.TransitionType;
                return true;
        }

        private static bool TryGetGlobalTransition(List<GlobalTransition> transitions, int sceneIndex, out ScreenTransitionType transitionType)
        {
                GlobalTransition transition = transitions.Find(transition => transition.SceneIndex == sceneIndex);
                if (transition.Equals(default))
                {
                        transitionType = default;
                        return false;
                }
                
                transitionType = transition.TransitionType;
                return true;
        }
}