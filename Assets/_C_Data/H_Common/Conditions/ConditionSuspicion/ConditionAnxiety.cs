using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�s���i�ȋ^�j���g���ȋ^�����p���_���[�W</summary>
public class ConditionAnxiety : ConditionBase
{

    private Emotion _emotion = Emotion.Suspicion;
    [SerializeField] private int _damage = default;

    /// <summary>�s���i�ȋ^�j���g���ȋ^�����p���_���[�W</summary>
    public ConditionAnxiety()
    {
        _condition = Condition.Anxiety;
        _name = "�s��";
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
        CommonUtils.LogDebugLine(this, "ActivateConditionEffect()", _name + "���������܂���");

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
