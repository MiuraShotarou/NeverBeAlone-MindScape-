using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// NewInputSystem を 旧InputSystem感覚で使えるようにした static Class
/// </summary>
public static class NewInput
{
    private static InputAcitonMonoBehaviour _inputAcitonMonoBehaviourInstance;
    private static InputAcitonMonoBehaviour _InputAcitonMonoBehaviourInstance => _inputAcitonMonoBehaviourInstance ??= new GameObject().AddComponent<InputAcitonMonoBehaviour>();
    public static float GetAxis(string axisName)
    {
        switch (axisName)
        {
            case "Horizontal":
                return _InputAcitonMonoBehaviourInstance._MoveInput.y;
            case "Vertical":
                return _InputAcitonMonoBehaviourInstance._MoveInput.x;
        }
        return 0;
    }
    // public Vector2 OnMove(InputAction.CallbackContext context)
    // {
    //     Vector2 moveInput = context.ReadValue<Vector2>(); //stringの引数で取得出来るようにする
    //     return moveInput;
    // }
    // public void OnLook(InputAction.CallbackContext context)
    // {
    //     Vector2 lookInput = context.ReadValue<Vector2>();
    // }

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