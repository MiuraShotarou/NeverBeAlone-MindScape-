using System;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// NewInputSystemから公式に沿ったやり方で入力情報を受取るクラス
/// </summary>
public class InputAcitonMonoBehaviour : InputAcitonFieldData
{
    // private static InputAcitonMonoBehaviour _instance;
    // public static InputAcitonMonoBehaviour _Instance{
    //     get
    //     {
    //         if (_instance == null)
    //             _instance = new GameObject().AddComponent<InputAcitonMonoBehaviour>();
    //         return _instance;
    //     }
    // }
    protected void OnMove(InputAction.CallbackContext context)
    {
        _MoveInput = context.ReadValue<Vector2>();
    }
    protected void OnLook(InputAction.CallbackContext context)
    {
        _LookInput = context.ReadValue<Vector2>();
    }
}
/// <summary>
/// InputAcitonMonoBehaviourで受け取った情報を保持するだけのクラス
/// </summary>
public class InputAcitonFieldData : MonoBehaviour
{
    public Vector2 _MoveInput{get; set;}
    public Vector2 _LookInput{get; set;}
}

