using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitPlayer : BattleUnitBase
{
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

    /// <summary>
    /// テスト用攻撃メソッド
    /// </summary>
    public void TestAttack()
    {
        _loopHandler.PlayerOneMoreFlg = false;
        _animator.Play("Attack1");
    }
    
    public void TestOneMoreAttack()
    {
        _loopHandler.PlayerOneMoreFlg = true;
        _animator.Play("Attack1");
    }

    public void OnAttackAnimationEnd()
    {
        _actionTarget.GetComponent<BattleUnitBase>().OnAttacked(_attack);
        // if (_battleLoopHandler.PlayerOneMoreFlg)
        // {
        //     _battleLoopHandler.BattleState = BattleState.TurnStart;
        // }
        // else
        // {
        //     _battleLoopHandler.BattleState = BattleState.TurnEnd;
        // }
    }

    public void SetActionTarget(GameObject target)
    {
        _actionTarget = target;
        ExecuteAction();
    }

    public void ExecuteAction()
    {
        _loopHandler.BattleState = BattleState.Busy;
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

    public override void TakeDamage(int damage)
    {
        _damageText.gameObject.SetActive(true);
        _damageText.text = damage.ToString();
        // 対象のscreenPosition
        Vector3 targetScreenPos = _mainCamera.WorldToScreenPoint(transform.position);
        Vector2 uiPos;
        // 対象のscreenPositionをUIのanchoredPositionに変換する
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _UICanvas.transform as RectTransform,
            targetScreenPos,
            null,   // Overlayの場合はnullでOK
            out uiPos
        );
        _damageText.rectTransform.anchoredPosition = uiPos;
        _damageText.gameObject.GetComponent<Animator>().Play("ShowDamage");
        _hp -= damage;
        Debug.Log(_unitName + "がダメージを受ける：" + damage + "。　残りHP：" + _hp);
    }
}
