using UnityEngine;

public class MouseInput : IInputHandler
{
    bool IInputHandler.isInputDown => Input.GetButtonDown("Fire1");

    Vector2 IInputHandler.inputPosition => Input.mousePosition;
}
