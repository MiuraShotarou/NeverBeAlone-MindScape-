using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// NewInputSystem を 旧InputSystem感覚で使えるようにした static Class
/// </summary>
public static class NewInput
{
    public static void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>(); //stringの引数で取得出来るようにする
    }

    public static float GetAxisRaw(string axisName) //stringを引数に持つ
    {
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
}