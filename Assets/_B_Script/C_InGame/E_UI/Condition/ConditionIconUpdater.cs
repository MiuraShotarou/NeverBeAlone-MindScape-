using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

/// <summary>
/// 状態アイコンを更新するコンポーネント
/// 「==UIManager==」にアタッチする予想
/// </summary>
public class ConditionIconUpdater : MonoBehaviour
{
    [SerializeField] private ObjectManager _objectManager;
    [SerializeField] private ConditionIconData _conditionIconData;
    private BattleUnitPlayer _player;

    private void Start()
    {
        _player = _objectManager.PlayerUnits[0];
    }

    /// <summary>
    /// 状態アイコン表示を更新する
    /// </summary>
    public void UpdateConditionIconUI()
    {
        Condition playerCondition = _player.ConditionFlag;
        foreach (var e in _conditionIconData.ConditionIconDictionary)
        {
            // 判定：「&」演算子で全てのビットをAND演算を行う
            // 結果が0以外なら、プレイヤーが持つ状態が判定対象を含むこととなる
            if ((playerCondition & e.Key) != 0)
            {
                e.Value.SetActive(true);
            }
            else
            {
                e.Value.SetActive(false);
            }
        }
    }
}