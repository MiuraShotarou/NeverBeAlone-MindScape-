using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e�X�g�p�BBattleUnit�ɃA�^�b�`���ĕt�^��������Ԉُ��I��ł�����s����
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
