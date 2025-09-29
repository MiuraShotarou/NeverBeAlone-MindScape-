using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // ← 追加

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
        // --- New Input System ---
        // Input.GetMouseButtonDown(0) → Mouse.current.leftButton.wasPressedThisFrame
        // Input.mousePosition → Mouse.current.position.ReadValue()
        if (Mouse.current.leftButton.wasPressedThisFrame &&
            _battleLoopHandler.BattleState == BattleState.WaitForTargetSelect)
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()); // ← New Input
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObj = hit.collider.gameObject;
                BattleUnitBase clickedUnit = clickedObj.GetComponent<BattleUnitBase>();

                // クリック対象が敵の場合のみ、対象選択を確定する
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
