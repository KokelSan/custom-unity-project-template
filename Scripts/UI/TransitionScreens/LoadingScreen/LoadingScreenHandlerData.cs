using System;

public static class LoadingScreenHandlerData
{
    public static void UpdateLoadingProgress(float onTransitionCompleted) => OnLoadingProgressUpdate?.Invoke(onTransitionCompleted);
    public static event Action<float> OnLoadingProgressUpdate;
}