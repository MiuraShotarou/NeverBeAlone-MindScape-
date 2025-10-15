using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Diagnostics;
//using static System.Net.Mime.MediaTypeNames;
using Random = UnityEngine.Random;

public abstract class BattleUnitBase : MonoBehaviour
{
	[Header("Editor項目")] [SerializeField] private ObjectManager _objects;
	[SerializeField] public Image Icon;
	[SerializeField] public Text DamageText;
	[SerializeField] public Canvas UICanvas;
	protected BattleLoopHandler _loopHandler = default;
	protected Camera _mainCamera = default;
	protected Animator _animator = default;
    protected GameObject _actionTarget = null;

	[Space(20)] [SerializeField] public string _unitName;

	[SerializeField, Tooltip("攻撃力")] public int AttackBase;
	[SerializeField, Tooltip("攻撃力倍率")] public float AttackScaleBase;
	[SerializeField, Tooltip("防御力")] public int DefenseBase;
	[SerializeField, Tooltip("防御力倍率")] public float DefenseScaleBase;
	[SerializeField, Tooltip("敏捷性")] public int AgilityBase;
	[SerializeField, Tooltip("回避率")] public float EvadeRateBase;
	[SerializeField, Tooltip("クリティカルヒットレート")]
	public float CriticalRateBase;
	[SerializeField, Tooltip("クリティカルダメージレート")]
	public float CriticalScaleBase;
	[SerializeField, Tooltip("回復力")] public float HealScaleBase;
	
	[Space(20)] [SerializeField, Tooltip("最大正気度")]
	public int MaxHp;
    [SerializeField, Tooltip("現在正気度")] public int Hp;
    [SerializeField, Tooltip("エンカウント回数")] public (int Infiltration, int EncounterCount) Progress;
	[SerializeField, Tooltip("死亡フラグ")] public bool IsDead;
	[SerializeField, Tooltip("現在感情")] public EmotionBase CurrentEmotion = new EmotionVoid(1);
	[SerializeField, Tooltip("スキルエフェクト")] public List<SkillEffectBase> SkillEffects = new List<SkillEffectBase>();
	[SerializeField, Tooltip("状態異常フラグ")] public Condition ConditionFlag = Condition.None;
	[SerializeField, Tooltip("感情レベル")] public int[] EmotionLevels = { 1, 1, 1, 1, 1 };
	[HideInInspector, Tooltip("スキル")] public Dictionary<string, int> SkillDict = new Dictionary<string, int>();
	[SerializeField, Tooltip("使用スキル")] protected SkillBase _skill;

	public delegate void JudgeSurvival();

	protected void Start()
	{
		Debug.Log($"{gameObject.name}, {Hp}");
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
		//EmotionDict.Add(Emotion.Void, new EmotionVoid(EmotionLevels[(int)Emotion.Void]));
		//EmotionDict.Add(Emotion.Anger, new EmotionAnger(EmotionLevels[(int)Emotion.Anger]));
		//EmotionDict.Add(Emotion.Grudge, new EmotionGrudge(EmotionLevels[(int)Emotion.Grudge]));
		//EmotionDict.Add(Emotion.Hatred, new EmotionHatred(EmotionLevels[(int)Emotion.Hatred]));
		//EmotionDict.Add(Emotion.Suspicion, new EmotionSuspicion(EmotionLevels[(int)Emotion.Suspicion]));
	}

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

	/// <summary>
	/// 被ダメージ処理
	/// </summary>
	/// <param name="damage">ダメージ値</param>
	public abstract void TakeDamage(int damage);

	/// <summary>
	/// 行っていること
	/// 弱点・耐性の判断と計算
	/// 乱数の追加
	/// </summary>
	protected virtual int CalculateFinalDamageTaken(float damage, Emotion emotion)
	{
		float finalDefense = CalcFinalDefense();

		if (emotion == CurrentEmotion.WeakEmotion)
		{
			CommonUtils.LogDebugLine(this, "CulculateFinalDamageTaken()",
				"弱点, 防御力：" + finalDefense);
		}
		else if (emotion == CurrentEmotion.ResistantEmotion)
		{
			CommonUtils.LogDebugLine(this, "CulculateFinalDamageTaken()",
				"耐性, 防御力：" + finalDefense);
		}

		float damageBlurScale = Random.Range(0.95f, 1.05f);
		int finalDamageTaken = Mathf.RoundToInt((damage - finalDefense) * damageBlurScale);
		if (finalDamageTaken < 0)
		{
			finalDamageTaken = 0;
		}

		CommonUtils.LogDebugLine(this, "CalculateFinalDamageTaken()",
			"ダメージブレ係数：" + damageBlurScale + ", 最終ダメージ：" + finalDamageTaken);
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

	protected SkillBase GetSkill(string skillKey) => _objects.SkillBaseDict[skillKey];
	protected void SetTargetObject(GameObject target) => _actionTarget = target;

	/// <summary>	
	/// 最終攻撃力を算出
	/// </summary>
	/// <returns></returns>
	protected virtual float CalcFinalAttack()
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
	protected virtual float CalcFinalDefense()
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
	protected virtual float CalcAttackMod()
	{
		float mod = 0;
		// スキルエフェクトの効果を適用
		foreach (SkillEffectBase effect in SkillEffects)
		{
			IAttackModifier modifier = effect as IAttackModifier;
			if (modifier != null)
			{
				mod = modifier.ModifyAttack(SkillDict[effect.Name], mod);
			}
		}
		return mod;
	}

	/// <summary>
	/// 攻撃倍率ボーナスを算出
	/// </summary>
	/// <returns></returns>
	protected virtual float CalcAttackScaleMod()
	{
		float mod = 0;
		// 所持バフの効果を適用
		foreach (SkillEffectBase effect in SkillEffects)
		{
			IAttackScaleModifier modifier = effect as IAttackScaleModifier;
			if (modifier != null)
			{
				mod = modifier.ModifyAttackScale(SkillDict[effect.Name], mod);
			}
		}
		// 弱点・耐性の効果を適用
		mod = ModifyEmotionAffinityAttackScale(mod, _skill.Emotion, _actionTarget.GetComponent<BattleUnitBase>().CurrentEmotion);
        // 使用者の感情とスキルの感情が一致していた場合の効果を適用
		mod = ModifyEmotionMatchAttackScale(mod, _skill.Emotion, _actionTarget.GetComponent<BattleUnitBase>().CurrentEmotion);
		return mod;
	}

	/// <summary>
	/// 防御力ボーナスを算出する
	/// </summary>
	/// <returns></returns>
	protected virtual float CalcDefenseMod()
	{
		float mod = 0;
		// 所持バフの効果を適用
		foreach (SkillEffectBase effect in SkillEffects)
		{
			IDefenseModifier modifier = effect as IDefenseModifier;
			if (modifier != null)
			{
				mod = modifier.ModifyDefense(SkillDict[effect.Name], mod);
			}
		}
		return mod;
	}

	/// <summary>
	/// 防御力倍率を算出
	/// </summary>
	/// <returns></returns>
	protected virtual float CalcDefenseScaleMod()
	{
		float mod = 0;
		// 所持バフの効果を適用
		foreach (SkillEffectBase effect in SkillEffects)
		{
			IDefenseScaleModifier modifier = effect as IDefenseScaleModifier;
			if (modifier != null)
			{
				mod = modifier.ModifyDefenseScale(SkillDict[effect.Name], mod);
			}
		}
		return mod;
	}

	/// <summary>状態異常の発動タイミングで呼び出す /// </summary>
	public void OnConditionActivate(ConditionActivationType timing)
	{
		foreach (Condition condition in Enum.GetValues(typeof(Condition)))
		{
			if (ConditionFlag.HasFlag(condition) && condition != Condition.None)
			{
				ConditionBase conditionbase = ConditionDatabase.Database[condition];
				if (conditionbase.Type == timing)
				{
					conditionbase.ActivateConditionEffect();
				}
			}
		}
	}
	/// <summary>
	/// 弱点相性判定とmod加算
	/// </summary>
	/// <param name="mod"></param>
	/// <param name="skillEmotion"></param>
	/// <param name="targetEmotion"></param>
	/// <returns></returns>
	protected virtual float ModifyEmotionAffinityAttackScale(float mod, Emotion skillEmotion, EmotionBase targetEmotion)
	{
		if (skillEmotion == targetEmotion.WeakEmotion)
		{
			mod = ModifyWeekAttackScale(mod);
		}
		else if (skillEmotion == targetEmotion.ResistantEmotion)
		{
			mod = ModifyResistAttackScale(mod);
		}
		return mod;
	}
	/// <summary>
	/// 使用者の感情マッチ判定とmod加算
	/// </summary>
	/// <param name="mod"></param>
	/// <param name="skillEmotion"></param>
	/// <param name="targetEmotion"></param>
	/// <returns></returns>
	protected virtual float ModifyEmotionMatchAttackScale(float mod, Emotion skillEmotion, EmotionBase targetEmotion)
	{
		if (skillEmotion == CurrentEmotion.Emotion)
		{
			if (skillEmotion == targetEmotion.WeakEmotion)
			{
				mod = ModifyEmotionMatchWeekAttackScale(mod);
			}
		}
		return mod;
	}
	protected virtual float ModifyWeekAttackScale(float attackScale) => attackScale + GameDataManager.Instance.WeekAttackScale; //mod + 0.1f
	protected virtual float ModifyResistAttackScale(float attackScale) => attackScale + GameDataManager.Instance.ResistAttacktScale;  //mod + -0.2f
	protected virtual float ModifyEmotionMatchWeekAttackScale(float attackScale) => attackScale + GameDataManager.Instance.EmotionMatchScale; //mod + 0.4f
}