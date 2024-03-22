using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpecificTransition
{
        public int FromSceneIndex;
        public int ToSceneIndex;
        public ScreenTransitionType TransitionType;
}

[Serializable]
public struct GlobalTransition
{
        public int SceneIndex;
        public ScreenTransitionType TransitionType;
}

[CreateAssetMenu(menuName = "Project Template/New UI Scene Transitions Table",  fileName = "UISceneTransitionsTableSO")]
public class UISceneTransitionsTableSO : ScriptableObject
{
        public ScreenTransitionType DefaultTransition;
        public List<SpecificTransition> SpecificTransitions;
        public List<GlobalTransition> GlobalFromTransitions;
        public List<GlobalTransition> GlobalToTransitions;
}