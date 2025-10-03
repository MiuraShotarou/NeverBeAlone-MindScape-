using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // ← New Input System

public class MouseSelectHandler : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private ObjectManager _objects;
    private BattleLoopHandler _battleLoopHandler;
    private BattleUIController _uiController;
    private BattleUnitPlayer _player;

    void Start()
    {
        _mainCamera = Camera.main;
        _uiController = _objects.UIController;
        _battleLoopHandler = _objects.BattleLoopHandler;
        _player = _objects.PlayerUnits[0];
    }

    void Update()
    {
        // 早期リターン
        if (_battleLoopHandler.BattleState != BattleState.WaitForTargetSelect)
            return;

        Vector2 screenPos;
        bool pressed = false; // タップ検知用フラグ

        // --- PC用 ---
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            screenPos = Mouse.current.position.ReadValue();
            pressed = true;
        }
        // --- スマホ用 ---
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            screenPos = Touchscreen.current.primaryTouch.position.ReadValue();
            pressed = true;
        }
        else
        {
            return;
        }

        if (pressed)
        {
            Ray ray = _mainCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject clickedObj = hit.collider.gameObject;
                BattleUnitBase clickedUnit = clickedObj.GetComponent<BattleUnitBase>();

                if (clickedUnit is BattleUnitEnemyBase && !clickedUnit.IsDead)
                {
                    _player.SetActionTarget(clickedObj);
                    _battleLoopHandler.BattleState = BattleState.Busy;
                    _uiController.DeactivateTargetSelectText();
                }
            }
        }
    }
}
