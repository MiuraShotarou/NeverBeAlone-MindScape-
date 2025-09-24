using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DevelopCameraController : MonoBehaviour
{
    [SerializeField] private bool _isDontFly;
    private Vector3 _moveDirection;
    private Vector2 _rotateDelta;
    private Vector2 _rotateDirection;
    private int _moveSpeed = 5;
    private float _rotSpeed = 40f;
    private int _clampPosY = 1;
    private int _clampRotX = 90;
    //
    private Vector2 _moveInput;
    // public void On2DComposite(InputValue value) //ここの戻り値を調べるところから
    // {
    //     // V / H どちらも取得している
    //     _moveInput = value.Get<Vector2>();
    //     Debug.Log("InputAction" + _moveInput);
    // }
    void Start()
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    protected void OnMove(InputAction.CallbackContext context) //
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    protected void OnLook(InputAction.CallbackContext context)
    {
        _rotateDelta = context.ReadValue<Vector2>();
    }

    void Update()
    {
        // _moveDirection.y = _moveInput.y;
        // _moveDirection.x = _moveInput.x;
        
        _moveDirection.y = NewInput.GetAxis("Vertical");
        // _moveDirection.y = Input.GetAxis("Vertical");
        // _moveDirection.x = Input.GetAxis("Horizontal");
        // _moveDirection.y = Keyboard.current.wKey.isPressed ? 1 : 0;
        // _moveDirection.y = Keyboard.current.sKey.isPressed ? -1 : 0;
        // _moveDirection.x = Keyboard.current.aKey.isPressed ? 1 : 0;
        // _moveDirection.x = Keyboard.current.dKey.isPressed ? -1 : 0;
        //要はdirectionが取れればそれで良い
        if (_moveInput.x != 0 || _moveInput.y != 0)
        {
            // Debug.Log("Move");
            transform.position += transform.forward * _moveDirection.y * _moveSpeed * Time.deltaTime;
            transform.position += transform.right * _moveDirection.x * _moveSpeed * Time.deltaTime;
            if (_isDontFly)
            {
                transform.position = new Vector3(transform.position.x, _clampPosY, transform.position.z);
            }
        }
        // Vector2 _rotateDelta = Mouse.current.delta.ReadValue(); // 前フレームとの差分
        int ajdustY = -1;
        if (_rotateDelta.x != 0  || _rotateDelta.y != 0)
        {
            Vector3 updateRot = transform.rotation.eulerAngles;
            updateRot.y += _rotateDelta.x * Time.deltaTime * _rotSpeed;
            updateRot.x += ajdustY * (_rotateDelta.y * Time.deltaTime * _rotSpeed);
            // updateRot.x = Mathf.Clamp(updateRot.x,  ajdustY * _clampRotX, _clampRotX);
            gameObject.transform.rotation = Quaternion.Euler(updateRot);
        }
    }
}
