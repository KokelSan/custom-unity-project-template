using System;
using UnityEngine;

public static class InputManagerHandlerData
{
    #region Mouse & Keyboard
    
    public static void Move(Vector2 value) => OnMove?.Invoke(value);
    public static event Action<Vector2> OnMove;
    
    public static void Look(Vector2 value) => OnLook?.Invoke(value);
    public static event Action<Vector2> OnLook;
    
    public static void RightClick(Vector2 position) => OnRightClick?.Invoke(position);
    public static event Action<Vector2> OnRightClick;
    
    public static void Escape() => OnEscape?.Invoke();
    public static event Action OnEscape;
    
    public static void Space() => OnSpace?.Invoke();
    public static event Action OnSpace;
    
    #endregion

    #region Touchscreen

    public static void Tap(Vector2 position) => OnTap?.Invoke(position);
    public static event Action<Vector2> OnTap;

    #endregion
    
    #region Common

    public static void PointerMove(Vector2 newPosition) => OnPointerMove?.Invoke(newPosition);
    public static event Action<Vector2> OnPointerMove;
    
    public static void Click(Vector2 position) => OnClick?.Invoke(position);
    public static event Action<Vector2> OnClick;

    #endregion
}