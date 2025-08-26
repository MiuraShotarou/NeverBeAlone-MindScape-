using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitPlayer : BattleUnitBase
{
    private Animator _animator;
    private GameObject _actionTarget = null;
    private AttackType _attackType;

    public GameObject ActionTarget
    {
        get => _actionTarget;
        set => _actionTarget = value;
    }

    enum AttackType
    {
        Normal,
        OneMore
    } 
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    /// <summary>
    /// テスト用攻撃メソッド
    /// </summary>
    public void TestAttack()
    {
        _battleHandler.PlayerOneMoreFlg = false;
        _animator.Play("Attack1");
    }
    
    public void TestOneMoreAttack()
    {
        _battleHandler.PlayerOneMoreFlg = true;
        _animator.Play("Attack1");
    }

    public void OnAttackAnimationEnd()
    {
        Debug.Log(_actionTarget.GetComponent<BattleUnitBase>()._unitName + "に" + _attack + "ダメージ");
        if (_battleHandler.PlayerOneMoreFlg)
        {
            _battleHandler.BattleState = BattleState.TurnStart;
        }
        else
        {
            _battleHandler.BattleState = BattleState.TurnEnd;
        }
    }

    public void SetActionTarget(GameObject target)
    {
        _actionTarget = target;
        ExecuteAction();
    }

    public void ExecuteAction()
    {
        _battleHandler.BattleState = BattleState.Busy;
        switch (_attackType)
        {
            case AttackType.Normal:
                TestAttack();
                break;
            case AttackType.OneMore:
                TestOneMoreAttack();
                break;
            default:
                break;
        }
    }

    public void SetAttackType(string type)
    {
        switch (type)
        {
            case "Normal":
                _attackType = AttackType.Normal;
                break;
            case "OneMore":
                _attackType = AttackType.OneMore;
                break;
            default:
                break;
        }
    }
}
