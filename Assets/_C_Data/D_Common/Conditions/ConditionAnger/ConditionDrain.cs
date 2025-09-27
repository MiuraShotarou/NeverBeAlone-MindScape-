using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>ドレイン（怒り）与えたダメージ * 0.1 回復する</summary>
public class ConditionDrain : ConditionBase
{

    private Emotion _emotion = Emotion.Anger;
    [SerializeField] private int _damageDealt = 10;
    [SerializeField] private float _hpRestoreScale = 0.1f;

    /// <summary>ドレイン（怒り）与えたダメージ * 0.1 回復する</summary>
    public ConditionDrain(Condition condition) : base(condition)
    {
        _name = "ドレイン";
        _type = ConditionActivationType.OnAttackExecute;
    }

    public override void ApplyCondition()
    {
        
    }

    public override void ReapplyCondition()
    {
        
    }

    public override void ActivateConditionEffect()
    {
        // TODO 自分が敵に与えたダメージを持ってくる
        //_damageDealt = 
        int restoringHp = Mathf.RoundToInt(_damageDealt * _hpRestoreScale);
        restoringHp *= -1;
        _target.GetComponent<BattleUnitBase>().TakeDamage(restoringHp);
        CommonUtils.LogDebugLine(this, "ActivateConditionEffect()", _name + "が発動しました");
    }

    public override void RemoveCondition()
    {
        
    }
}
