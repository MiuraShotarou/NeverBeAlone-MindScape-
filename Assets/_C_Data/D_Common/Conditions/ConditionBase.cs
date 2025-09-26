using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ConditionBase : ScriptableObject
{
    protected Condition _condition;
    /// <summary>継続ターン数</summary>
    protected int _activeTurns = 1;
    protected ConditionActivationType _type;
    protected BattleUnitBase _target = default;

    /// <summary>コンストラクタ。状態異常のクラスとビットフラグを紐づける</summary>
    public ConditionBase(Condition condition)
    {
        _condition = condition;
    }

    /// <summary>状態異常を付与。必要なパラメーター取得等</summary>
    public abstract void ApplyCondition();

    /// <summary>同じ状態異常を付与されたとき</summary>
    public abstract void ReapplyCondition();

    /// <summary>状態異常の効果が発動する瞬間の処理</summary>
    public abstract void ActivateConditionEffect();

    /// <summary>状態異常を解除</summary>
    public abstract void RemoveCondition();

    /// <summary>ターゲットに状態異常を付与してビットフラグに追加</summary>
    /// <param name="targetCondition"></param>
    public void ApplyConditionToTarget(BattleUnitBase target)
    {
        _target = target;
        Condition targetCondition = target.ConditionFlag;

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

    /// <summary>ターゲットの状態を戻してビットフラグから削除</summary>

    public void RemoveConditionFromTarget(BattleUnitBase target)
    {
        Condition targetCondition = target.ConditionFlag;
        RemoveCondition();
        targetCondition &= ~_condition;
        Debug.Log(_condition + "状態が解除されました");
    }
}
