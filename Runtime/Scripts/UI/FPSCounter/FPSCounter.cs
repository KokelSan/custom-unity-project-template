using TMPro;
using UnityEngine;

// Inspired by the Viking Village Demo FPS counter
public class FPSCounter : BaseBehaviour
{
    public TMP_Text FPSText;
    public float FPSMeasurePeriod = 0.5f;
    
    private int _fpsAccumulator = 0;
    private float _fpsNextPeriod = 0;
    private int _currentFps;

    protected override void EventHandlerRegister()
    {
        FPSCounterHandlerData.OnSetFPSCountVisibility += SetTextVisibility;
    }

    protected override void EventHandlerUnRegister()
    {
        FPSCounterHandlerData.OnSetFPSCountVisibility -= SetTextVisibility;
    }

    private void Start()
    {
        _fpsNextPeriod = Time.realtimeSinceStartup + FPSMeasurePeriod;
    }

    private void Update()
    {
        // measure average frames per second
        _fpsAccumulator++;
        if (Time.realtimeSinceStartup > _fpsNextPeriod)
        {
            _currentFps = (int) (_fpsAccumulator/FPSMeasurePeriod);
            _fpsAccumulator = 0;
            _fpsNextPeriod += FPSMeasurePeriod;
            FPSText.text = $"{_currentFps} FPS";
        }
    }

    private void SetTextVisibility(bool isVisible)
    {
        FPSText.alpha = isVisible ? 1 : 0;
    }
}