using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SkillBaseが継承
/// </summary>
public abstract class ConditionBase
{
    protected Condition _condition;
    public Condition ConditionName
    {
        get { return _condition; }
        set { _condition = value; }
    }

    /// <summary>状態異常の効果</summary>
    public abstract void ApplyCondition();

    /// <summary>同じ状態異常を付与されたとき</summary>
    public abstract void ReapplyCondition();

    /// <summary>状態異常の解除</summary>
    public abstract void RemoveCondition();

    /// <summary>ターゲットのビットフラグに追加</summary>
    /// <param name="targetCondition"></param>
    public void ApplyConditionToTarget(Condition targetCondition)
    {
        if ((targetCondition & _condition) == _condition)
        {
            ReapplyCondition();
        }
        else
        {
            ApplyCondition();
        }
        targetCondition |= _condition;
        Debug.Log(_condition + "状態になりました");
    }

    /// <summary>ターゲットのビットフラグから削除</summary>
    
    public void RemoveConditionFromTarget(Condition targetCondition)
    {
        RemoveCondition();
        targetCondition &= ~_condition;
        Debug.Log(_condition + "状態が解除されました");
    }
}
