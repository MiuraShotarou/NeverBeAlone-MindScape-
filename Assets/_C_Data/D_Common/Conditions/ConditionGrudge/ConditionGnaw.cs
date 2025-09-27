using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>心蝕（怨念）自身に怨念属性継続ダメージ</summary>
public class ConditionGnaw : ConditionBase
{

    private Emotion _emotion = Emotion.Grudge;
    [SerializeField] private int _damage = default;

    /// <summary>心蝕（怨念）自身に怨念属性継続ダメージ</summary>
    public ConditionGnaw(Condition condition) : base(condition)
    {
        _name = "心蝕";
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
