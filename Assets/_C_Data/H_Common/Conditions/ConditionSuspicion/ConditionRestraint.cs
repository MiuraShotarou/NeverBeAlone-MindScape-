using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�����i�ȋ^�j���̃^�[���܂ōs���s�\</summary>
public class ConditionRestraint : ConditionBase
{

    private Emotion _emotion = Emotion.Suspicion;
    [SerializeField] private int _damage = default;

    /// <summary>�s���i�ȋ^�j���g���ȋ^�����p���_���[�W</summary>
    public ConditionRestraint()
    {
        _condition = Condition.Restraint;
        _name = "����";
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
        CommonUtils.LogDebugLine(this, "ActivateConditionEffect()", _name + "���������܂���");
        var battleLoopHandler = FindObjectOfType<BattleLoopHandler>();

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
