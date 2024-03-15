using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class BaseUIMenu : MonoBehaviour
{
    public CanvasGroup CanvasGroup;

    protected virtual void EventHandlerRegister(){}
    protected virtual void EventHandlerUnRegister(){}
    
    private void Awake()
    {
        EventHandlerRegister();
    }

    private void OnDestroy()
    {
        EventHandlerUnRegister();
    }

    private void Start()
    {
        if (CanvasGroup == null && !TryGetComponent(out CanvasGroup))
        {
            Debug.LogWarning($"UIMenu named '{name}' (id = {GetInstanceID()}) has no CanvasGroup component'");
        }
    }

    private void SetCanvasGroupProperties(float alpha, bool intaractableAndBlocksRaycasts)
    {
        if (CanvasGroup == null) return;
        CanvasGroup.alpha = alpha;
        CanvasGroup.interactable = CanvasGroup.blocksRaycasts = intaractableAndBlocksRaycasts;
    }
    
    public void Show()
    {
        SetCanvasGroupProperties(1, true);
    }

    public void Hide()
    {
        SetCanvasGroupProperties(0, false);
    }
}
