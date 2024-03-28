using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SpecificTransition
{
        public int FromSceneIndex;
        public int ToSceneIndex;
        public TransitionType TransitionType;
}

[Serializable]
public struct GlobalTransition
{
        public int SceneIndex;
        public TransitionType TransitionType;
}

[CreateAssetMenu(menuName = "Project Template/New UI Scene Transitions Table",  fileName = "UISceneTransitionsTableSO")]
public class UISceneTransitionsTableSO : ScriptableObject
{
        public TransitionType DefaultTransition;
        public List<SpecificTransition> SpecificTransitions;
        public List<GlobalTransition> GlobalFromTransitions;
        public List<GlobalTransition> GlobalToTransitions;
}