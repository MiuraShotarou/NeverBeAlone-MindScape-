using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopCameraController : MonoBehaviour
{
    [SerializeField] private bool _isDontFly;
    private Vector3 _moveDirection;
    private Vector2 _rotateDirection;
    private int _moveSpeed = 5;
    private int _rotSpeed = 100;
    private int _clampPosY = 1;
    private int _clampRotX = 90;
    void Update()
    {
        _moveDirection.y = Input.GetAxis("Vertical");
        _moveDirection.x = Input.GetAxis("Horizontal");
        if (_moveDirection.x != 0  || _moveDirection.y != 0)
        {
            transform.position += transform.forward * _moveDirection.y * _moveSpeed * Time.deltaTime;
            transform.position += transform.right * _moveDirection.x * _moveSpeed * Time.deltaTime;
            if (_isDontFly)
            {
                transform.position =  new Vector3(transform.position.x, _clampPosY, transform.position.z);
            }
        }
        _rotateDirection.x = Input.GetAxis("Mouse X");
        _rotateDirection.y = Input.GetAxis("Mouse Y");
        int ajdustY = -1;
        if (_rotateDirection.x != 0  || _rotateDirection.y != 0)
        {
            Vector3 updateRot = transform.rotation.eulerAngles;
            updateRot.y += _rotateDirection.x * Time.deltaTime * _rotSpeed;
            updateRot.x += ajdustY * (_rotateDirection.y * Time.deltaTime * _rotSpeed);
            // updateRot.x = Mathf.Clamp(updateRot.x,  ajdustY * _clampRotX, _clampRotX);
            gameObject.transform.rotation = Quaternion.Euler(updateRot);
        }
    }
}
