using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary></summary>
public class ConditionName : ConditionBase
{

    private Emotion _emotion = Emotion.Anger;

    /// <summary>��Ԉُ�̃N���X�ƃr�b�g�t���O��R�Â���</summary>
    public ConditionName(Condition condition) : base(condition)
    {
        //���N���X�Œ�`�ς݁B���������Ȃ���OK
    }

    public override void ApplyCondition()
    {
        ActiveTurns = Random.Range(1, 3);
    }

    public override void ReapplyCondition()
    {
        ActiveTurns = Random.Range(1, 3);
    }

    public override void ActivateConditionEffect()
    {
        if (ActiveTurns == 0) return;


        ActiveTurns -= 1;
    }

    public override void RemoveCondition()
    {
        ActiveTurns = 0;
    }
}
