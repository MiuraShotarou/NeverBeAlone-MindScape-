using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>与えたダメージ×0.1回復</summary>
public class ConditionDrain : ConditionBase
{

    private Emotion _emotion = Emotion.Anger;
    private float _hpRestoreRate = 0.1f;
    private int _targetHp = default;

    /// <summary>状態異常のクラスとビットフラグを紐づける</summary>
    public ConditionDrain(Condition condition) : base(condition)
    {
        //基底クラスで定義済み。何も書かなくてOK
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
        //与えたダメージ
        int damage = 0;
        _targetHp += (int)(damage * _hpRestoreRate);
    }

    public override void RemoveCondition()
    {
        ActiveTurns = 0;
    }
}
