using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Scriptables/Create Omoiokomete")]
public sealed class Omoiokomete : SkillBase
{
    public override string SkillName => "Omoiokomete";
    public override string SkillDescription => "敵一体に強力な一撃を与える。";
    public override Emotion Emotion => Emotion.Anger;
    public override ConditionActivationType ConditionActivationType => ConditionActivationType.Always; //必要ないかもしれないけど一応
    public override SkillEffectBase SkillEffectBase => new OmoiokometeEffect();
    // public abstract AttackRange AttackRange(int level); // 攻撃範囲（未実装）
    public override Tension NeedTension(int level) => Tension.Calm;
    public override int AttackCount(int level) => 1; //攻撃回数
}
/// <summary>
/// 状態異常（Condition）とは別に、スキルが保有しているステータス変動効果を内蔵しているスクリプト。
/// 継承先のクラスからどのインターフェイスを実装しているか確認することで、変動するステータスの種類を判断することが出来る。
/// スキルレベルによって変動する可能性のある値をInterface経由で実装する。
/// </summary>
public sealed class OmoiokometeEffect : SkillEffectBase, IAttackScaleModifier
{
    public float ModifyAttackScale(int skillLevel, float attackScale)
    {
        switch(skillLevel)
        {
            case < 7:
                return attackScale + 1.124f;
            case default:
                return attackScale;
        }
    }
}