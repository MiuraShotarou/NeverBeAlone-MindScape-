using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用。BattleUnitにアタッチして付与したい状態異常を選んでから実行する
/// </summary>
public class TestConditionController : MonoBehaviour
{
    [SerializeField] private Condition _condition;
    BattleUnitBase _battleUnit;

    private void Start()
    {
        _battleUnit = GetComponent<BattleUnitBase>();
        _battleUnit.ConditionFlag = _condition;

        foreach (Condition condition in Enum.GetValues(typeof(Condition)))
        {
            if (_battleUnit.ConditionFlag.HasFlag(condition) && condition != Condition.None)
            {
                ConditionBase conditionbase = ConditionDatabase.Database[condition];
                conditionbase.ApplyConditionToTarget(_battleUnit);
            }
        }
    }
}
