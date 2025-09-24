using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary></summary>
public class ConditionName : ConditionBase
{

    private Emotion _emotion = Emotion.Anger;

    /// <summary>状態異常のクラスとビットフラグを紐づける</summary>
    public ConditionName(Condition condition) : base(condition)
    {
        //基底クラスで定義済み。何も書かなくてOK
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
