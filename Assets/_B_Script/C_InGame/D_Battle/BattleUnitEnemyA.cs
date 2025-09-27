using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUnitEnemyA : BattleUnitEnemyBase
{
    private BattleUnitBase _actionTarget;

    public override void DoAttack1(BattleUnitBase targetGo)
    {
        _actionTarget = targetGo;
        _animator.Play("Attack");
    }
    
    public void OnAttackAnimationEnd()
    {
        float finalAttack = CalcFinalAttack();
        _actionTarget.GetComponent<BattleUnitBase>().OnAttacked(finalAttack, CurrentEmotion.Emotion);
    }
}