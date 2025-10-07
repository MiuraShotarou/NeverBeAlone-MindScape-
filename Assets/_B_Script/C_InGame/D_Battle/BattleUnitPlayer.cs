using UnityEngine;
using UnityEngine.Rendering;

public class BattleUnitPlayer : BattleUnitBase
{
    private TensionRank _tensionRank;
    [SerializeField, Tooltip("テンションメータ最大値")] public int MaxTensionBase;
    [SerializeField, Tooltip("テンションゲージ上昇率")] public float TentionUpRateBase;
    [SerializeField, Tooltip("テンション減少率")] public float TentionDownRateBase;
    // [SerializeField, Tooltip("テンション弱点倍率")] public float TensionUpAttackScale; //仕様書から削除する可能性あり
    [SerializeField, Tooltip("テンションメータ最大値")] public int MaxTension;
    [SerializeField, Tooltip("テンション")] public int Tension;
    [SerializeField, Tooltip("テンションランク")]public TensionRank TensionRank{get => _tensionRank; set{ if(value != _tensionRank){SetTensionDownRate();}_tensionRank = value;}}
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
    /// ここのメソッド内にターン確定後のメソッドをすべて羅列したい。
    /// </summary>
    public void DecidePlayerMove(string skillName ,GameObject target)
    {
        SetTargetObject(target);
        _loopHandler.PlayerOneMoreFlg = skillName.Contains("OneMore");
        _skill = GetSkill(skillName);
        // 状態と一致した感情スキルを使用した場合、テンションが上昇する。(+弱点倍率 +1.4f の処理はダメージ計算の時に呼ぶ)
        if (_skill.Emotion == CurrentEmotion.Emotion)
        {
            float matchScale = 0.4f;
            Tension = ModifyPlayerTension(Tension, matchScale);
        }
	    // 攻撃範囲の変動 → 敵を選択する段階から反映させていないといけない
	    // スキルの使用条件 → 敵を選択する段階から反映させていないといけない
	    // ステータスの変動 → スキルによるステータスの変動は、状態異常とは別で数値で管理する(SkillEffect)。この後の処理に登場する
	    // 状態異常の付与（ランダム要素あり）
	    // 攻撃倍率の変動 
	    // スキルによる条件判定（弱点かどうか、敵・自身のバフ・デバフの数、攻撃後のOneMoreフラグ、敵の数カウント）
	    // 攻撃回数の反映
	    // スキルが持っている属性の反映
	    // 感情の切り替え
	    // 囮の考慮
        TestAttack();
    }

    /// <summary>
    /// テスト用攻撃メソッド。TestOneMoreAttack()と統合した。
    /// </summary>
    private void TestAttack()
    {
        _loopHandler.BattleState = BattleState.Busy; //削除するのはOKかもしれない
        _animator.Play("Attack1");
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
        // 感情ステータスボーナスを適用
        IAttackScaleModifier emotionModifier = CurrentEmotion as IAttackScaleModifier;
        if (emotionModifier != null)
        {
            mod = emotionModifier.ModifyAttackScale(EmotionLevels[(int)CurrentEmotion.Emotion], mod);
        }
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
    
    /// <summary>
    /// 弱点相性判定とmod加算
    /// </summary>
    /// <param name="mod"></param>
    /// <param name="skillEmotion"></param>
    /// <param name="targetEmotion"></param>
    /// <returns></returns>
    protected override float ModifyEmotionAffinityAttackScale(float mod, Emotion skillEmotion, EmotionBase targetEmotion)
    {
        if (skillEmotion == targetEmotion.WeakEmotion)
        {
            mod = ModifyWeekAttackScale(mod);
            // mod = ModifyTensionBonusWeekAttackScale(mod); //主人公のみ
        }
        else if (skillEmotion == targetEmotion.ResistantEmotion)
        {
            mod = ModifyResistAttackScale(mod);
        }
        return mod;
    }

    public void KillDrain() => Hp += Mathf.RoundToInt(MaxHp * RegenerateBase);
    
    private void SetTensionDownRate()
    {
        switch (TensionRank)
        {
            case TensionRank.Calm:
                TentionDownRateBase = GameDataManager.Instance.CalmTensionDownRate;
                // TensionUpAttackScale = GameDataManager.Instance.CalmWeekAttackScale;
                break;
            case TensionRank.Excited:
                TentionDownRateBase = GameDataManager.Instance.ExcitedTensionDownRate;
                // TensionUpAttackScale = GameDataManager.Instance.ExcitedWeekAttackScale;
                break;
            case TensionRank.Fun:
                TentionDownRateBase = GameDataManager.Instance.FunTensionDownRate;
                // TensionUpAttackScale = GameDataManager.Instance.FunWeekAttackScale;
                break;
            case TensionRank.Awakening:
                TentionDownRateBase = GameDataManager.Instance.AwakeningTensionDownRate;
                // TensionUpAttackScale = GameDataManager.Instance.AwakeningWeekAttackScale;
                break;
        }
    }

    // private float ModifyTensionBonusWeekAttackScale(float mod) => mod + TensionUpAttackScale;
    private int ModifyPlayerTension(int current, float scale) => Mathf.RoundToInt(current + MaxTensionBase * scale * TentionUpRateBase); //弱点を突かれた時はscaleをマイナスにする
}
