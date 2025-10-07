using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/Create SkillOneMore")]
public sealed class SkillOneMore : SkillBase
{
    public override string ClientName => "もう一回行動する";
    public override string ClientDescription => "もう一度俺のターン。";
    public override Emotion Emotion => Emotion.Anger;
    // public abstract AttackRange AttackRange(int level); // 攻撃範囲（未実装）
    public override TensionRunk NeedTension(int level) => TensionRunk.Calm;
    public override int AttackCount(int level) => 1; //攻撃回数
    public override ConditionActivationType ConditionActivationType => ConditionActivationType.Always; //必要ないかもしれないけど一応
    public override SkillEffectBase SkillEffectBase => new OneMoreEffect();
    public override Action SpecialEffect => () => {};
}
/// <summary>
/// 状態異常（Condition）とは別に、スキルが保有しているステータス変動効果を内蔵しているスクリプト。
/// 継承先のクラスからどのインターフェイスを実装しているか確認することで、変動するステータスの種類を判断することが出来る。
/// スキルレベルによって変動する可能性のある値をInterface経由で実装する。
/// </summary>
public sealed class OneMoreEffect : SkillEffectBase
{
    public override string Name => "OneMore";
}