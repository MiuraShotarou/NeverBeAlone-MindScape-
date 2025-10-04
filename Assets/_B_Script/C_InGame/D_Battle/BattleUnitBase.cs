using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;
using Random = UnityEngine.Random;

public abstract class BattleUnitBase : MonoBehaviour
{
    [Header("Editor項目")]
    [SerializeField] private ObjectManager _objects;
    [SerializeField] public Image Icon;
    [SerializeField] public Text DamageText;
    [SerializeField] public Canvas UICanvas;
    protected BattleLoopHandler _loopHandler = default;
    protected Camera _mainCamera = default;
    protected Animator _animator = default;

    [Space(20)]
    [SerializeField] public string _unitName;
    [Header("基礎値")]
    [SerializeField, Tooltip("最大正気度")] public int MaxHpBase;
    [SerializeField, Tooltip("攻撃力")] public int AttackBase;
    [SerializeField, Tooltip("攻撃力倍率")] public float AttackScaleBase;
    [SerializeField, Tooltip("弱点攻撃力倍率")] public float AttackWeakpointScaleBase;
    [SerializeField, Tooltip("耐性攻撃力倍率")] public float AttackResistanceScaleBase;
    [SerializeField, Tooltip("防御力")] public int DefenseBase;
    [SerializeField, Tooltip("防御力倍率")] public float DefenseScaleBase;
    [SerializeField, Tooltip("耐性防御力倍率")] public float DefenseWeakpointScaleBase;
    [SerializeField, Tooltip("弱点防御力倍率")] public float DefenseResistanceScaleBase;
    [SerializeField, Tooltip("敏捷性")] public float DexBase;
    [SerializeField, Tooltip("回避率")] public float EvadeRateBase;
    [SerializeField, Tooltip("クリティカルヒットレート")] public float CriticalRateBase;
    [SerializeField, Tooltip("クリティカルダメージレート")] public float CriticalScaleBase;
    [SerializeField, Tooltip("回復力")] public float RegenerateBase;
    [SerializeField, Tooltip("テンションメータ最大値")] public float MaxTensionBase;
    [SerializeField, Tooltip("テンションゲージ上昇率")] public float TentionUpRateBase;
    [SerializeField, Tooltip("テンション減少率")] public float TentionDownRateBase;

    [Space(20)]
    [SerializeField, Tooltip("最大正気度")] public int MaxHp;
    [SerializeField, Tooltip("現在正気度")] public int Hp;
    [SerializeField, Tooltip("付与する経験値")] public int ExpReward = 10; // publicなので不安
    [SerializeField, Tooltip("総経験値")] public int ExpAmmount = 0; // publicなので不安
    [SerializeField, Tooltip("レベル")] public int Level = 1; // publicなので不安
    [SerializeField, Tooltip("テンションメータ最大値")] public float MaxTension;
    [SerializeField, Tooltip("テンション")] public int Tension;
    [SerializeField, Tooltip("テンションレベル")] public int TensionLevel;
    [SerializeField, Tooltip("攻撃力加算値")] public float AttackMod;
    [SerializeField, Tooltip("攻撃力倍率加算値")] public float AttackScaleMod;
    [SerializeField, Tooltip("防御力加算値")] public float DefenseMod;
    [SerializeField, Tooltip("防御力倍率加算値")] public float DefenseScaleMod;
    [SerializeField, Tooltip("敏捷性")] public int Dex;
    [SerializeField, Tooltip("回避率")] public int EvadeRate;
    [SerializeField, Tooltip("死亡フラグ")] public bool IsDead;
    [SerializeField, Tooltip("感情状態")] public Dictionary<Emotion, EmotionBase> Emotions;
    [SerializeField, Tooltip("現在感情")] public EmotionBase CurrentEmotion = new EmotionVoid(1);
    [SerializeField, Tooltip("所持バフ")] public List<SkillEffectBase> SkillEffects = new List<SkillEffectBase>();
    [SerializeField, Tooltip("状態異常フラグ")] public Condition ConditionFlag = Condition.None;
    [SerializeField, Tooltip("感情レベル")] public int[] EmotionLevels = {1, 1, 1, 1, 1};


    public delegate void JudgeSurvival();
    protected void Start()
    {
        OnStart();
    }

    /// <summary>
    /// Startメソッドが実行する処理
    /// </summary>
    protected virtual void OnStart()
    {
        _mainCamera = Camera.main;
        _loopHandler = _objects.BattleLoopHandler;
        _animator = gameObject.GetComponent<Animator>();
        _objects.BattleEventController.DoJudgeSurvival += DoJudgeSurvival;

        //Emotion EnumとEmotionクラスの紐づけ。いらないかも
        //Emotions.Add(Emotion.Void, new EmotionVoid(EmotionLevels[(int)Emotion.Void]));
        //Emotions.Add(Emotion.Anger, new EmotionAnger(EmotionLevels[(int)Emotion.Anger]));
        //Emotions.Add(Emotion.Grudge, new EmotionGrudge(EmotionLevels[(int)Emotion.Grudge]));
        //Emotions.Add(Emotion.Hatred, new EmotionHatred(EmotionLevels[(int)Emotion.Hatred]));
        //Emotions.Add(Emotion.Suspicion, new EmotionSuspicion(EmotionLevels[(int)Emotion.Suspicion]));
    }
    /// <summary>
    /// 被ダメージ処理
    /// </summary>
    /// <param name="damage">ダメージ値</param>
    public abstract void TakeDamage(int damage);

    public virtual void OnDeath()
    {
        Debug.Log(_unitName + "の死亡時効果発動.");
    }

    /// <summary>
    /// 死亡判定
    /// </summary>
    protected bool IsUnitDead()
    {
        return Hp <= 0;
    }
    /// <summary>
    /// 死亡判定を行う
    /// 死亡となった場合、その効果を実行する
    /// </summary>
    protected virtual void DoJudgeSurvival()
    {
        if (IsUnitDead())
        {
            IsDead = true;
            // Common.LogDebugLine(this, "DoJudgeSurvival()", _unitName + "死亡");
            _animator.Play("Death");
            OnDeath();
            _objects.BattleEventController.DoJudgeSurvival -= DoJudgeSurvival;
        }
        // else
        // {
        //     CommonUtils.LogDebugLine(this, "DoJudgeSurvival()", _unitName + "生存");
        // }
    }
    /// <summary>
    /// 攻撃を受ける時の処理
    /// </summary>
    public virtual void OnAttacked(float damage, Emotion emotion)
    {
        TakeDamage(CalculateFinalDamageTaken(damage, emotion));
    }

    protected virtual int CalculateFinalDamageTaken(float damage, Emotion emotion)
    {
        float finalDefense = CalcFinalDefense();

        if (emotion == CurrentEmotion.WeakEmotion)
        {
            finalDefense *= DefenseWeakpointScaleBase;
            CommonUtils.LogDebugLine(this, "CulculateFinalDamageTaken()", "弱点, 防御" + DefenseWeakpointScaleBase + "倍, 防御力：" + finalDefense);
        }
        else if (emotion == CurrentEmotion.ResistantEmotion)
        {
            finalDefense *= DefenseResistanceScaleBase;
            CommonUtils.LogDebugLine(this, "CulculateFinalDamageTaken()", "耐性, 防御" + DefenseResistanceScaleBase + "倍, 防御力：" + finalDefense);
        }

        float damageBlurScale = Random.Range(0.95f, 1.05f);
        int finalDamageTaken = Mathf.RoundToInt((damage - finalDefense) * damageBlurScale);
        if (finalDamageTaken < 0)
        {
            finalDamageTaken = 0;
        }
        CommonUtils.LogDebugLine(this, "CalculateFinalDamageTaken()", "ダメージブレ係数：" + damageBlurScale + ", 最終ダメージ：" + finalDamageTaken);
        return finalDamageTaken;
    }

    // /// <summary>
    // /// 感情切り替え時の処理
    // /// 現在の感情ボーナスを解除して、新しい感情ボーナスを適用する
    // /// </summary>
    // /// <param name="emotion"></param>
    // protected virtual void OnSwitchEmotion(EmotionBase emotion)
    // {
    //     CurrentEmotion.RemoveBuff(this);
    //     CurrentEmotion = emotion;
    //     CurrentEmotion.ApplyBuff(this);
    // }
    //
    // /// <summary>
    // /// Buff/Debuffを獲得した時の処理
    // /// </summary>
    // /// <param name="effect"></param>
    // public void OnGainEffect(SkillEffectBase effect)
    // {
    //     effect.ApplyEffect(this, null);
    //     SkillEffects.Add(effect);
    // }
    //
    // /// <summary>
    // /// Buff/Debuffが解除された時の処理
    // /// </summary>
    // /// <param name="effect"></param>
    // public void OnRemoveEffect(SkillEffectBase effect)
    // {
    //     effect.RemoveEffect(this, null);
    //     SkillEffects.Remove(effect);
    // }

    /// <summary>
    /// 最終攻撃力を算出
    /// </summary>
    /// <returns></returns>
    public float CalcFinalAttack()
    {
        float valueMod = CalcAttackMod();
        float scaleMod = CalcAttackScaleMod();
        float finalAttack = (AttackBase + valueMod) * (AttackScaleBase + scaleMod);
        CommonUtils.LogDebugLine(this, "CalcFinalAttack()", "攻撃側：" + _unitName);
        CommonUtils.LogDebugLine(this, "CalcFinalAttack()",
            "基礎攻撃力：" + AttackBase
                     + ", 攻撃ボーナス：" + valueMod
                     + ", 基礎攻撃力倍率：" + AttackScaleBase
                     + ", 攻撃力倍率ボーナス：" + scaleMod
                     + ", 最終攻撃力：" + finalAttack);
        return finalAttack;
    }
    /// <summary>
    /// 最終防御力を算出
    /// </summary>
    /// <returns></returns>
    public float CalcFinalDefense()
    {
        float valueMod = CalcDefenseMod();
        float scaleMod = CalcDefenseScaleMod();
        float finalDefense = (DefenseBase + valueMod) * (DefenseScaleBase + scaleMod);
        CommonUtils.LogDebugLine(this, "CalcFinalDefense()", "防御側：" + _unitName);
        CommonUtils.LogDebugLine(this, "CalcFinalDefense()",
            "基礎防御力：" + DefenseBase
                     + ", 防御ボーナス：" + valueMod
                     + ", 基礎防御力倍率：" + DefenseScaleBase
                     + ", 防御力倍率ボーナス：" + scaleMod
                     + ", 最終防御力：" + finalDefense);
        return finalDefense;
    }
    /// <summary>
    /// 攻撃ボーナスを算出
    /// </summary>
    /// <returns></returns>
    public float CalcAttackMod()
    {
        float mod = 0;
        // 感情ボーナスを適用
        IAttackModifier emotionModifier = CurrentEmotion as IAttackModifier;
        if (emotionModifier != null)
        {
            mod = emotionModifier.ModifyAttack(mod);
        }

        // TODO テンションボーナスを適用【未実装】

        // 所持バフの効果を適用
        foreach (SkillEffectBase effect in SkillEffects)
        {
            IAttackModifier modifier = effect as IAttackModifier;
            if (modifier != null)
            {
                mod = modifier.ModifyAttack(mod);
            }
        }
        return mod;
    }
    /// <summary>
    /// 攻撃倍率ボーナスを算出
    /// </summary>
    /// <returns></returns>
    public float CalcAttackScaleMod()
    {
        float mod = 0;
        // 感情ボーナスを適用
        IAttackScaleModifier emotionModifier = CurrentEmotion as IAttackScaleModifier;
        if (emotionModifier != null)
        {
            mod = emotionModifier.ModifyAttackScale(mod);
        }

        // TODO テンションボーナスを適用【未実装】

        // 所持バフの効果を適用
        foreach (SkillEffectBase effect in SkillEffects)
        {
            IAttackScaleModifier modifier = effect as IAttackScaleModifier;
            if (modifier != null)
            {
                mod = modifier.ModifyAttackScale(mod);
            }
        }
        return mod;
    }

    /// <summary>
    /// 防御力ボーナスを算出する
    /// </summary>
    /// <returns></returns>
    public float CalcDefenseMod()
    {
        float mod = 0;
        // 感情ボーナスを適用
        IDefenseModifier emotionModifier = CurrentEmotion as IDefenseModifier;
        if (emotionModifier != null)
        {
            mod = emotionModifier.ModifyDefense(mod);
        }

        // TODO テンションボーナスを適用【未実装】

        // 所持バフの効果を適用
        foreach (SkillEffectBase effect in SkillEffects)
        {
            IDefenseModifier modifier = effect as IDefenseModifier;
            if (modifier != null)
            {
                mod = modifier.ModifyDefense(mod);
            }
        }
        return mod;
    }
    /// <summary>
    /// 防御力倍率を算出
    /// </summary>
    /// <returns></returns>
    public float CalcDefenseScaleMod()
    {
        float mod = 0;
        // 感情ボーナスを適用
        IDefenseScaleModifier emotionModifier = CurrentEmotion as IDefenseScaleModifier;
        if (emotionModifier != null)
        {
            mod = emotionModifier.ModifyDefenseScale(mod);
        }

        // TODO テンションボーナスを適用【未実装】

        // 所持バフの効果を適用
        foreach (SkillEffectBase effect in SkillEffects)
        {
            IDefenseScaleModifier modifier = effect as IDefenseScaleModifier;
            if (modifier != null)
            {
                mod = modifier.ModifyDefenseScale(mod);
            }
        }
        return mod;
    }
}