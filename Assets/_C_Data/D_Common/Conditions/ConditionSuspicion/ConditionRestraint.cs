using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>束縛（猜疑）次のターンまで行動不能</summary>
public class ConditionRestraint : ConditionBase
{

    private Emotion _emotion = Emotion.Suspicion;
    [SerializeField] private int _damage = default;

    /// <summary>不安（猜疑）自身に猜疑属性継続ダメージ</summary>
    public ConditionRestraint()
    {
        _condition = Condition.Restraint;
        _name = "束縛";
        _type = ConditionActivationType.OnTurnStart;
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

        _activeTurns -= 1;
        CommonUtils.LogDebugLine(this, "ActivateConditionEffect()", _name + "が発動しました");
        var battleLoopHandler = GameObject.FindObjectOfType<BattleLoopHandler>();

        if (battleLoopHandler != null)
        {
            battleLoopHandler.BattleState = BattleState.TurnEnd;
        }

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
