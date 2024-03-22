using System;

public static class UILoadingScreenHandlerData
{
    public static void UpdateLoadingProgress(float value) => OnLoadingProgressUpdate?.Invoke(value);
    public static event Action<float> OnLoadingProgressUpdate;
    
    public static void WaitForInput(Action onScreenClickedAction) => OnWaitForInput?.Invoke(onScreenClickedAction);
    public static event Action<Action> OnWaitForInput;
    
    public static bool ShouldWaitForInput() => OnShouldWaitForInput?.Invoke() ?? false;
    public static event Func<bool> OnShouldWaitForInput;
}