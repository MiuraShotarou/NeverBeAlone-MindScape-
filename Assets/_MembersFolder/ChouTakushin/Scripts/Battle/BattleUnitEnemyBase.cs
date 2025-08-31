using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleUnitEnemyBase : BattleUnitBase
{
    [SerializeField] protected GameObject _playerGo;
    /// <summary>
    /// テスト用：敵攻撃
    /// </summary>
    public abstract void DoAttack1(BattleUnitBase targetGo);

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
