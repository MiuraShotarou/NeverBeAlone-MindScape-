using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitEnemyB : BattleUnitEnemyBase
{
    private BattleUnitBase _actionTarget;

    public override void DoAttack1(BattleUnitBase targetGo)
    {
        _actionTarget = targetGo;
        _animator.Play("Attack");
    }
    
    public void OnAttackAnimationEnd()
    {
        _actionTarget.OnAttacked(_attack);
    }
}
