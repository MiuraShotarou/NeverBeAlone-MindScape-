using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitEnemyB : BattleUnitEnemyBase
{
    private Animator _animator;
    private GameObject _actionTarget;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void DoAttack1(GameObject targetGo)
    {
        _actionTarget = targetGo;
        _animator.Play("Attack");
    }
    
    public void OnAttackAnimationEnd()
    {
        _actionTarget.GetComponent<BattleUnitBase>().TakeDamage(_attack);
        _battleHandler.BattleState = BattleState.TurnEnd;
    }
}
