using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // ← New Input System

public class MouseSelectHandler : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private ObjectManager _objects;
    [SerializeField] private Skill_UI _skillUI;
    private BattleLoopHandler _battleLoopHandler;
    private BattleUIController _uiController;

    void Awake()
    {
        _mainCamera = Camera.main;
        _uiController = _objects.UIController;
        _battleLoopHandler = _objects.BattleLoopHandler;
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
                    _skillUI.SetTargetObject(clickedObj); //コードは汚いが、ターゲットオブジェクトを渡さなくてはならないため
                    _uiController.DeactivateTargetSelectText(); //Deactive → OnDisable → Playerの行動処理
                    _battleLoopHandler.BattleState = BattleState.Busy;
                }
            }
        }
    }
}
