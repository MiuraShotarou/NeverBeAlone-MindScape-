using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>自傷（怒り）自身に怒り属性継続ダメージ</summary>
public class ConditionSelfharm : ConditionBase
{
    private Emotion _emotion = Emotion.Anger;
    [SerializeField] private int _damage = 20;

    /// <summary>自傷（怒り）自身に怒り属性継続ダメージ</summary>
    public ConditionSelfharm()
    {
        _condition = Condition.Selfharm;
        _name = "自傷";
        _type = ConditionActivationType.OnTurnEnd;
    }

    public override void ApplyCondition()
    {
        _activeTurns = Random.Range(1, 4);
    }

    public override void ReapplyCondition()
    {
        _activeTurns += Random.Range(1, 3);
    }

    public override void ActivateConditionEffect()
    {
        if (_activeTurns == 0) return;

        _target.GetComponent<BattleUnitBase>().OnAttacked(_damage, _emotion);
        _activeTurns -= 1;
        CommonUtils.LogDebugLine(this, "ActivateConditionEffect()", _name + "が発動しました");

        if (_activeTurns == 0)
        {
            RemoveConditionFromTarget(_target);
        }
    }

    public override void RemoveCondition()
    {
        _activeTurns = 0;
    }
}
