using UnityEngine;

public abstract class BaseBehaviour : MonoBehaviour
{
    protected virtual void EventHandlerRegister() {}
    protected virtual void EventHandlerUnRegister() {}
    protected virtual void Initialize() {}

    private void Awake()
    {
        EventHandlerRegister();
        Initialize();
    }

    private void OnDestroy()
    {
        EventHandlerUnRegister();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}