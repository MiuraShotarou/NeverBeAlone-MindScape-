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
	        _loopHandler.PlayerOneMoreFlg = skillName.Contains("OneMore");
	        SkillBase skill = GetSkill(skillName);
	        TestAttack();
	    }
	
	    /// <summary>
	    /// テスト用攻撃メソッド。TestOneMoreAttack()と統合した。
	    /// </summary>
	    private void TestAttack()
	    {
	        _loopHandler.BattleState = BattleState.Busy; //削除するのはOKかもしれない
	        _animator.Play("Attack1");
	        // 攻撃範囲の変動 → 敵を選択する段階から反映させていないといけない
	        // スキルの使用条件 → スキルレベル、スキルごとに存在する使用条件をUIの段階で決めておく必要がある
	        // ステータスの変動 → スキルによるステータスの変動は、状態異常とは別で数値で管理する(SkillEffect)。
	        // 状態異常の付与（ランダム要素あり）
	        // 攻撃倍率の変動
	        // スキルによる条件判定（弱点かどうか、敵・自身のバフ・デバフの数、攻撃後のOneMoreフラグ、敵の数カウント）
	        // 攻撃回数の反映
	        // スキルが持っている属性の反映
	        // 感情の切り替え
	        // 囮の考慮
	        Attack();
	    }
	
	    public void Attack()
	    {
	        float finalAttack = CalcFinalAttack();
	        var targetBattleUnitBase = _actionTarget.GetComponent<BattleUnitBase>();
	        targetBattleUnitBase.OnAttacked(finalAttack, CurrentEmotion.Emotion);
	    }
	
	    /// <summary>
	    /// 攻撃ボーナスを算出
	    /// </summary>
	    /// <returns></returns>
	    protected override float CalcAttackMod()
	    {
	        float mod = base.CalcAttackMod();
	        // 感情ボーナスを適用
	        IAttackModifier emotionModifier = CurrentEmotion as IAttackModifier;
	        if (emotionModifier != null)
	        {
	            mod = emotionModifier.ModifyAttack(EmotionLevels[(int)CurrentEmotion.Emotion], mod);
	        }
	        
	        return mod;
	    }
	
	    /// <summary>
	    /// 攻撃倍率ボーナスを算出
	    /// </summary>
	    /// <returns></returns>
	    protected override float CalcAttackScaleMod()
	    {
	        float mod = base.CalcAttackScaleMod();

	        IAttackScaleModifier emotionModifier = CurrentEmotion as IAttackScaleModifier;
	        if (emotionModifier != null)
	        {
	            mod = emotionModifier.ModifyAttackScale(EmotionLevels[(int)CurrentEmotion.Emotion], mod);
	        }
	        
	        // TODO 感情と状態が一致した時のボーナスを適用【未実装】（+ 1.4）弱点攻撃倍率にしか影響しない
	
	        // TODO テンションボーナスを適用【未実装】（楽しい +1.1, 覚醒 +1.2）弱点攻撃倍率にしか影響しない
	        // 弱点ヒット・耐性ヒットの判定をどこで取るかが大事だ
	        // 弱点を突かれたときの挙動 （テンションが減少、ダメージが変動）
	        
	        return mod;
	    }
	
	    /// <summary>
	    /// 防御力ボーナスを算出する
	    /// </summary>
	    /// <returns></returns>
	    protected override float CalcDefenseMod()
	    {
	        float mod = base.CalcDefenseMod();
	        // 感情ボーナスを適用
	        IDefenseModifier emotionModifier = CurrentEmotion as IDefenseModifier;
	        if (emotionModifier != null)
	        {
	            mod = emotionModifier.ModifyDefense(EmotionLevels[(int)CurrentEmotion.Emotion], mod);
	        }
	
	        // TODO テンションボーナスを適用【未実装】

	        return mod;
	    }
	
	    /// <summary>
	    /// 防御力倍率を算出
	    /// </summary>
	    /// <returns></returns>
	    protected override float CalcDefenseScaleMod()
	    {
	        float mod = base.CalcDefenseScaleMod();
	        // 感情ボーナスを適用
	        IDefenseScaleModifier emotionModifier = CurrentEmotion as IDefenseScaleModifier;
	        if (emotionModifier != null)
	        {
	            mod = emotionModifier.ModifyDefenseScale(EmotionLevels[(int)CurrentEmotion.Emotion], mod);
	        }
	        
	        // TODO テンションボーナスを適用【未実装】（楽しい +0.05f, 覚醒 +0.1f）弱点耐性倍率にしか影響しない
	
	        return mod;
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