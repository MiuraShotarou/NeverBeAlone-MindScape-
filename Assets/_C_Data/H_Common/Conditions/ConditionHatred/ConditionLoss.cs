using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>喪失（嫌悪）自身に嫌悪属性継続ダメージ</summary>
public class ConditionLoss : ConditionBase
{

    private Emotion _emotion = Emotion.Hatred;
    [SerializeField] private int _damage = default;

    /// <summary>喪失（嫌悪）自身に嫌悪属性継続ダメージ</summary>
    public ConditionLoss()
    {
        _condition = Condition.Loss;
        _name = "喪失";
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
