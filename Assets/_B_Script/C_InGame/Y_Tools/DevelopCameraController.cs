using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// 開発用のカメラ移動を行うクラス
/// </summary>
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
    private Vector2 _moveInput;

    void Update()
    {
        _moveDirection.y = NewInput.GetAxisRaw("Vertical");
        _moveDirection.x = NewInput.GetAxisRaw("Horizontal");
        // Debug.Log(_moveDirection);
        if (_moveDirection.x != 0 || _moveDirection.y != 0)
        {
            transform.position += transform.forward * _moveDirection.y * _moveSpeed * Time.deltaTime;
            transform.position += transform.right * _moveDirection.x * _moveSpeed * Time.deltaTime;
            if (_isDontFly)
            {
                transform.position = new Vector3(transform.position.x, _clampPosY, transform.position.z);
            }
        }

        _rotateDelta.x = NewInput.GetAxis("Mouse X");
        _rotateDelta.y = NewInput.GetAxis("Mouse Y");
        // Vector2 _rotateDelta = Mouse.current.delta.ReadValue(); // 前フレームとの差分
        int ajdustY = -1;
        if (_rotateDelta.x != 0 || _rotateDelta.y != 0)
        {
            Vector3 updateRot = transform.rotation.eulerAngles;
            updateRot.y += _rotateDelta.x * Time.deltaTime * _rotSpeed;
            updateRot.x += ajdustY * (_rotateDelta.y * Time.deltaTime * _rotSpeed);
            // updateRot.x = Mathf.Clamp(updateRot.x,  ajdustY * _clampRotX, _clampRotX);
            gameObject.transform.rotation = Quaternion.Euler(updateRot);
        }
    }
}
