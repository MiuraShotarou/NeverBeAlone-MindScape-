using System;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// NewInputSystemから公式に沿ったやり方で入力情報を受け取っているクラス
/// </summary>
public class InputActionMonoBehaviour : InputActionFieldData
{
    public static InputActionMonoBehaviour _Instance { get; private set; }
    void OnEnable()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else if (_Instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        _MoveInputRaw = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _LookInputRaw = context.ReadValue<Vector2>();
    }
}
/// <summary>
/// InputAcitonMonoBehaviourで受け取った情報を保持するためだけのクラス
/// </summary>
public class InputActionFieldData : MonoBehaviour
{
    public Vector2 _MoveInputRaw { get; protected set; }
    public Vector2 _MoveInput { get; protected set; }
    public Vector2 _LookInputRaw { get; protected set; }
    public Vector2 _LookInput { get; protected set;}
}

