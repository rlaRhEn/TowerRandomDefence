using UnityEngine;

public class TouchInput : IInputHandler
{
    bool IInputHandler.isInputDown => Input.GetTouch(0).phase == TouchPhase.Began;

    Vector2 IInputHandler.inputPosition => Input.GetTouch(0).position;
}
