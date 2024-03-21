using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIBaseElement : BaseBehaviour
{
    [Header("UI Base Element")]
    public CanvasGroup CanvasGroup;
    public bool HideOnStart;
    
    protected bool IsVisible;
    
    protected override void Initialize()
    {
        if (CanvasGroup == null && !TryGetComponent(out CanvasGroup))
        {
            Debug.LogWarning($"UIMenu named '{name}' (id = {GetInstanceID()}) has no CanvasGroup component'");
            return;
        }
        
        if (HideOnStart)
        {
            Hide();
        }
    }

    private void SetCanvasGroupProperties(float alpha, bool interactableAndBlocksRaycasts)
    {
        if (CanvasGroup == null) return;
        CanvasGroup.alpha = alpha;
        CanvasGroup.interactable = CanvasGroup.blocksRaycasts = interactableAndBlocksRaycasts;
    }
    
    public void Show()
    {
        IsVisible = true;
        SetCanvasGroupProperties(1, true);
    }

    public void Hide()
    {
        IsVisible = false;
        SetCanvasGroupProperties(0, false);
    }
}
