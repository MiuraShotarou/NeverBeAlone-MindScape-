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

    //テスト用
    private void Awake()
    {
        CurrentEmotion = new EmotionAnger(1);
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
        float finalAttack = CalcFinalAttack();
        var targetBattleUnitBase = _actionTarget.GetComponent<BattleUnitBase>();
        targetBattleUnitBase.OnAttacked(finalAttack, CurrentEmotion.Emotion);

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
        DamageText.gameObject.SetActive(true);
        DamageText.text = damage.ToString("0");
        // 対象のscreenPosition
        Vector3 targetScreenPos = _mainCamera.WorldToScreenPoint(transform.position);
        Vector2 uiPos;
        // 対象のscreenPositionをUIのanchoredPositionに変換する
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UICanvas.transform as RectTransform,
            targetScreenPos,
            null,   // Overlayの場合はnullでOK
            out uiPos
        );
        DamageText.rectTransform.anchoredPosition = uiPos;
        DamageText.gameObject.GetComponent<Animator>().Play("ShowDamage");
        Hp -= damage;
        Debug.Log(_unitName + "がダメージを受ける：" + damage + "。　残りHP：" + Hp);
    }
}