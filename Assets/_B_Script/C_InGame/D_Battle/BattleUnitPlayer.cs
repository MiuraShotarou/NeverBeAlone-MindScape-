using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUnitPlayer : BattleUnitBase
{
    private GameObject _actionTarget = null;
    private string _decideSkillKey = "" ;
    private bool _isOneMoreAttack;

    public GameObject ActionTarget
    {
        get => _actionTarget;
        set => _actionTarget = value;
    }

    // //テスト用
    // private void Awake()
    // {
    //     CurrentEmotion = new EmotionAnger(1);
    // }

    /// <summary>
    /// Skill_UI.csからOnDisableで呼び出される。Playerの行動が確定する。
    /// ここのメソッド内にターン確定後のメソッドをすべて羅列したい
    /// </summary>
    public void DecidePlayerMove(string skillName ,GameObject target)
    {
        _actionTarget = target;
        _decideSkillKey = skillName;
        TestAttack();
    }

    /// <summary>
    /// テスト用攻撃メソッド。TestOneMoreAttack()と統合した。
    /// </summary>
    private void TestAttack()
    {
        _loopHandler.BattleState = BattleState.Busy; //削除するのはOKかもしれない
        _loopHandler.PlayerOneMoreFlg = _decideSkillKey.Contains("OneMore")? true : false;
        _animator.Play("Attack1");
        OnAttackAnimationEnd();
    }

    public void OnAttackAnimationEnd()
    {
        float finalAttack = CalcFinalAttack();
        var targetBattleUnitBase = _actionTarget.GetComponent<BattleUnitBase>();
        targetBattleUnitBase.OnAttacked(finalAttack, CurrentEmotion.Emotion);
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