using UnityEngine;

public interface IInputHandler
{
    bool isInputDown { get; }
    Vector2 inputPosition { get; }
    
}
