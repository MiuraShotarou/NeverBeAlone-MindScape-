using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// スキルの基底クラス。使用者のステータスに影響しない値を格納する。
/// </summary>
public abstract class SkillBase : ScriptableObject
{
    //アドレスとして利用する。最適化を狙うならenumにしても良い
    public abstract string SkillName { get;}
    public abstract string SkillDescription { get;} //UI用の文字列
    public abstract Emotion Emotion { get;} //スキルが持つ感情
    // public abstract AttackRange AttackRange(int level); // 攻撃範囲（未実装）
    public abstract Tension NeedTension(int level); //スキルの使用に必要なテンションランク
    public abstract int AttackCount(int level); //攻撃回数
    public abstract ConditionActivationType ConditionActivationType { get;} //必要ないかもしれないけど一応
    public abstract SkillEffectBase SkillEffectBase { get;} //ステータス変動効果
}
/// <summary>
/// ステータスに影響を与える数値をInterface経由で取得するためのクラス。
/// </summary>
public abstract class SkillEffectBase
{

}