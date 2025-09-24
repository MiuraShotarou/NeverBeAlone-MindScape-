using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// NewInputSystemを旧InputManagerの感覚で使えるようにしたstatic class
/// </summary>
public static class NewInput
{
    /// <summary>
    /// スムージングをかければGetAxisになる
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    public static float GetAxis(string axisName) //stringを引数に持つ
    {
        switch (axisName)
        {
            case "Horizontal":
                return InputActionMonoBehaviour._Instance._MoveInputRaw.x;
            case "Vertical":
                return InputActionMonoBehaviour._Instance._MoveInputRaw.y;
            case "Mouse X":
                return InputActionMonoBehaviour._Instance._LookInputRaw.x;
            case "Mouse Y":
                return InputActionMonoBehaviour._Instance._LookInputRaw.y;
        }
        return 0;
        if (Keyboard.current.wKey.isPressed && Keyboard.current.sKey.isPressed)
        {
            return 0;
        }
        else if (Keyboard.current.wKey.isPressed)
        {
            return 1;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    public static float GetAxisRaw(string axisName)
    {
        switch (axisName)
        {
            case "Horizontal":
                return InputActionMonoBehaviour._Instance._MoveInputRaw.x;
            case "Vertical":
                return InputActionMonoBehaviour._Instance._MoveInputRaw.y;
            case "Mouse X":
                return InputActionMonoBehaviour._Instance._LookInputRaw.x;
            case "Mouse Y":
                return InputActionMonoBehaviour._Instance._LookInputRaw.y;
        }
        return 0;
    }
}