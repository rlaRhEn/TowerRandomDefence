using UnityEngine;

public class InputManager 
{
    Transform container;
#if UNITY_ANDROID && !UNITY_EDITOR
    IInputHandler inputHandler = new TouchInput();
#else
    IInputHandler inputHandler = new MouseInput();
#endif
    public InputManager(Transform container)
    {
        this.container = container;
    }
    public bool isTouchDown => inputHandler.isInputDown;
    public Vector2 touchPosition => inputHandler.inputPosition;
}
