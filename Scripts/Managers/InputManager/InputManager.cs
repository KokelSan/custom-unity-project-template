using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Manager
{
    #region Mouse & Keyboard
    
    public void OnMove(InputValue input)
    {
        Vector2 value = input.Get<Vector2>();
        InputManagerHandlerData.Move(value);
    }
    
    public void OnLook(InputValue input)
    {
        Vector2 value = input.Get<Vector2>();
        InputManagerHandlerData.Look(value);
    }
    
    public void OnRightClick()
    {
        Vector2 position = Pointer.current.position.value;
        InputManagerHandlerData.RightClick(position);
    }
    
    public void OnEscape()
    {
        InputManagerHandlerData.Escape();
    }
    
    public void OnSpace()
    {
        InputManagerHandlerData.Space();
    }
    
    #endregion

    #region Touchscreen
    
    public void OnTap()
    {
        Vector2 position = Touchscreen.current.primaryTouch.position.value;
        InputManagerHandlerData.Tap(position);
    }
    
    #endregion

    #region Common

    public void OnPointerMove(InputValue input)
    {
        Vector2 value = input.Get<Vector2>();
        InputManagerHandlerData.PointerMove(value);
    }
    
    public void OnClick()
    {
        Vector2 position = Pointer.current.position.value;
        InputManagerHandlerData.Click(position);
    }

    #endregion
}