using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>怒り属性継続ダメージ</summary>
public class ConditionSelfharm : ConditionBase
{

    private Emotion _emotion = Emotion.Anger;
    private int _damage = default;
    private int _targetHp = default;

    /// <summary>状態異常のクラスとビットフラグを紐づける</summary>
    public ConditionSelfharm(Condition condition) : base(condition)
    {
        //基底クラスで定義済み。何も書かなくてOK
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
