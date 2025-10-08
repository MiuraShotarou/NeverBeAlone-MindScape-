using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitEnemyB : BattleUnitEnemyBase
{
    public override void DoAttack1(BattleUnitBase playerUnit)
    {
        SetTargetObject(playerUnit.gameObject);
        _animator.Play("Attack");
    }
    
    public void OnAttackAnimationEnd()
    {
        float finalAttack = CalcFinalAttack();
        _actionTarget.GetComponent<BattleUnitBase>().OnAttacked(finalAttack, CurrentEmotion.Emotion);
    }
}