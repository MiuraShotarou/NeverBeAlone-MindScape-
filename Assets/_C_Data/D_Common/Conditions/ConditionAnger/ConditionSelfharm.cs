using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�{�葮���p���_���[�W</summary>
public class ConditionSelfharm : ConditionBase
{

    private Emotion _emotion = Emotion.Anger;
    private int _damage = default;
    private int _targetHp = default;

    /// <summary>��Ԉُ�̃N���X�ƃr�b�g�t���O��R�Â���</summary>
    public ConditionSelfharm(Condition condition) : base(condition)
    {
        //���N���X�Œ�`�ς݁B���������Ȃ���OK
    }

    public override void ApplyCondition()
    {
        ActiveTurns = Random.Range(1, 3);
        _targetHp = Target.Hp;
    }

    public override void ReapplyCondition()
    {
        ActiveTurns = Random.Range(1, 3);
    }

    public override void ActivateConditionEffect()
    {
        if (ActiveTurns == 0) return;

        _targetHp -= _damage;
        ActiveTurns -= 1;
    }

    public override void RemoveCondition()
    {
        ActiveTurns = 0;
    }
}
