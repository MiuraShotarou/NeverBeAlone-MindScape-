using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnitEnemyBase : BattleUnitBase
{
    [SerializeField, Header("所持スキル")] private SkillBase[] SkillArrays;
    /// <summary>
    /// テスト用：敵攻撃
    /// </summary>
    public abstract void DoAttack1(BattleUnitBase targetGo);

    /// <summary>
    /// テスト用：所持スキル
    /// </summary>
    /// <param name="damage"></param>
    protected virtual void Awake()
    {
        if (SkillArrays[0] != null)
        {
            _skill = SkillArrays[0];
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