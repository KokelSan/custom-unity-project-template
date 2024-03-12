using UnityEngine;

public class EventDebugger : Manager
{
    public bool DebugSceneLoading;
    public bool DebugInputs;
    
    protected override void EventHandlerRegister()
    {
        if (DebugSceneLoading)
        {
            SceneLoadingManagerHandlerData.OnSceneLoaded += index => Debug.Log($"SceneLoadingManager: Scene {index} successfully loaded");
            SceneLoadingManagerHandlerData.OnSceneUnLoaded += index => Debug.Log($"SceneLoadingManager: Scene {index} successfully unloaded");
        }

        if (DebugInputs)
        {
            InputManagerHandlerData.OnMove += _ => Debug.Log($"Moving");
            InputManagerHandlerData.OnLook += _ => Debug.Log($"Looking");
            InputManagerHandlerData.OnRightClick += vector2 => Debug.Log($"Right click at {vector2}");
            InputManagerHandlerData.OnEscape += () => Debug.Log($"Escape pressed");
            InputManagerHandlerData.OnSpace += () => Debug.Log($"Space pressed");
            InputManagerHandlerData.OnTap += vector2 => Debug.Log($"Tap at {vector2}");
            InputManagerHandlerData.OnPointerMove += _ => Debug.Log($"Pointer moving");
            InputManagerHandlerData.OnClick += vector2 => Debug.Log($"Click at {vector2}");
        }
    }
}