using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�^�����_���[�W�~0.1��</summary>
public class ConditionDrain : ConditionBase
{

    private Emotion _emotion = Emotion.Anger;
    private float _hpRestoreRate = 0.1f;
    private int _targetHp = default;

    /// <summary>��Ԉُ�̃N���X�ƃr�b�g�t���O��R�Â���</summary>
    public ConditionDrain(Condition condition) : base(condition)
    {
        //���N���X�Œ�`�ς݁B���������Ȃ���OK
    }

    public override void ApplyCondition()
    {
        _targetHp = Target.Hp;
    }

    public override void ReapplyCondition()
    {
        ActiveTurns = Random.Range(1, 3);
    }

    public override void ActivateConditionEffect()
    {
        //�^�����_���[�W
        int damage = 0;
        _targetHp += (int)(damage * _hpRestoreRate);
    }

    public override void RemoveCondition()
    {
        ActiveTurns = 0;
    }
}
