using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelectHandler : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private BattleHandler _battleHandler;
    [SerializeField] private BattleUnitPlayer _player;
    void Start()
    {
        _mainCamera = Camera.main;
    }
    void Update() {
        
        if (Input.GetMouseButtonDown(0) && _battleHandler.BattleState == BattleState.WaitForTargetSelect) {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                GameObject clickedObj = hit.collider.gameObject;
                Debug.Log("Clicked On: " + clickedObj.name);
                // クリック対象が敵の場合のみ、対象選択を確定する
                if (clickedObj.GetComponent<BattleUnitBase>() is BattleUnitEnemyBase)
                {
                    _player.SetActionTarget(clickedObj);
                    _battleHandler.BattleState = BattleState.Busy;
                    _battleHandler.DeactiveTargetSelectText();
                }
            }
        }
    }
}
