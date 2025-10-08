using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleUnitEnemyA : BattleUnitEnemyBase
{
    //�e�X�g�p
    protected override void Awake()
    {
        base.Awake();
        CurrentEmotion = new EmotionHatred(1);
    }

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