using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIBaseElement : BaseBehaviour
{
    [Header("UI Base Element")]
    public CanvasGroup CanvasGroup;
    
    protected bool IsVisible;
    
    protected override void Initialize()
    {
        if (CanvasGroup == null && !TryGetComponent(out CanvasGroup))
        {
            Debug.LogWarning($"UIMenu named '{name}' (id = {GetInstanceID()}) has no CanvasGroup component'");
        }
    }

    private void SetCanvasGroupProperties(float alpha, bool interactableAndBlocksRaycasts)
    {
        if (CanvasGroup == null) return;
        CanvasGroup.alpha = alpha;
        CanvasGroup.interactable = CanvasGroup.blocksRaycasts = interactableAndBlocksRaycasts;
    }
    
    public void Enable()
    {
        IsVisible = true;
        SetCanvasGroupProperties(1, true);
    }

    public void Disable()
    {
        IsVisible = false;
        SetCanvasGroupProperties(0, false);
    }
}
