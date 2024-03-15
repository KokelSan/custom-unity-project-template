using UnityEngine;

/// <summary>
/// A base class for all managers needed in the game
/// </summary>
public abstract class Manager : MonoBehaviour
{
    private void Awake()
    {
        EventHandlerRegister();
    }

    private void OnDestroy()
    {
        EventHandlerUnRegister();
    }
    
    protected virtual void EventHandlerRegister(){}
    protected virtual void EventHandlerUnRegister(){}
    public virtual void Initialize(){}
}
