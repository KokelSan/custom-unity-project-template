using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputService : Service
{
    #region Mouse & Keyboard
    
    public void OnMove(InputValue input)
    {
        Vector2 value = input.Get<Vector2>();
        InputServiceHandlerData.Move(value);
    }
    
    public void OnLook(InputValue input)
    {
        Vector2 value = input.Get<Vector2>();
        InputServiceHandlerData.Look(value);
    }
    
    public void OnRightClick()
    {
        Vector2 position = Pointer.current.position.value;
        InputServiceHandlerData.RightClick(position);
    }
    
    public void OnEscape()
    {
        InputServiceHandlerData.Escape();
    }
    
    public void OnSpace()
    {
        InputServiceHandlerData.Space();
    }
    
    #endregion

    #region Touchscreen
    
    public void OnTap()
    {
        Vector2 position = Touchscreen.current.primaryTouch.position.value;
        InputServiceHandlerData.Tap(position);
    }
    
    #endregion

    #region Common

    public void OnPointerMove(InputValue input)
    {
        Vector2 value = input.Get<Vector2>();
        InputServiceHandlerData.PointerMove(value);
    }
    
    public void OnClick()
    {
        Vector2 position = Pointer.current.position.value;
        InputServiceHandlerData.Click(position);
    }

    #endregion
    
    /////////

    public void OnTest1()
    {
        GameManagerManagerHandlerData.StopGame();
        SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(1, screenTransitionType: ScreenTransitionType.Fade));
    }
    
    public void OnTest2()
    {
        GameManagerManagerHandlerData.StartGame();
        SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(2, screenTransitionType: ScreenTransitionType.Fade));
    }
    
    public void OnTest3()
    {
        GameManagerManagerHandlerData.StartGame();
        SceneLoadingServiceHandlerData.LoadScene(new SceneLoadingParameters(3, screenTransitionType: ScreenTransitionType.Fade));
    }
}