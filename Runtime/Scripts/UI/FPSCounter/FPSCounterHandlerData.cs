using System;

public static class FPSCounterHandlerData
{
    public static void SetFPSCountVisibility(bool isVisible) => OnSetFPSCountVisibility?.Invoke(isVisible);
    public static event Action<bool> OnSetFPSCountVisibility;
}