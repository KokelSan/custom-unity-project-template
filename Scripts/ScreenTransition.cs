using UnityEngine;

public enum TransitionType
{
    None = 0,
    Fade,
}

public class ScreenTransition : MonoBehaviour
{
    private const string BootTrigger = "Boot";
    private const string ShowTrigger = "Show";
    private const string HideTrigger = "Hide";
    
    public TransitionType TransitionType;
    public float TransitionSpeed = 1;
    
    [HideInInspector] public bool IsAnimating = false;
    private bool _isShown = false;
    private Animator _animator;

    public void Initialize()
    {
        if(!TryGetComponent(out _animator))
        {
            Debug.LogWarning($"Transition '{TransitionType} has no Animator component'");
        }
    }

    /// <summary>
    /// Show the transition to hide the screen
    /// </summary>
    public void Show(bool isBoot)
    {
        if(_animator == null || IsAnimating || _isShown) return;
        _animator.speed = TransitionSpeed;
        _isShown = true;
        _animator.SetTrigger(isBoot ? BootTrigger : ShowTrigger);
    }

    /// <summary>
    /// Hide the transition to show the screen
    /// </summary>
    public void Hide()
    {
        if(_animator == null || IsAnimating || !_isShown) return;
        _animator.speed = TransitionSpeed;
        _isShown = false;
        _animator.SetTrigger(HideTrigger);
    }
}