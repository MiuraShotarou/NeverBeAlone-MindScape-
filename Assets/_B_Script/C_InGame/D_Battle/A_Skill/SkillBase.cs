using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// スキルの基底クラス。使用者のステータスに影響しない値を格納する。
/// </summary>
public abstract class SkillBase : ScriptableObject
{
    public abstract string ClientName { get;} //ユーザー用のスキル名
    public abstract string ClientDescription { get;} //ユーザー用の説明文
    public abstract Emotion Emotion { get;} //スキルが持つ感情
    // public abstract AttackRange AttackRange(int level); // 攻撃範囲（未実装）
    public abstract TensionRank NeedTension(int level); //スキルの使用に必要なテンションランク
    public abstract int AttackCount(int level); //攻撃回数
    public abstract ConditionActivationType ConditionActivationType { get;} //必要ないかもしれないけど一応
    public abstract SkillEffectBase SkillEffectBase { get;} //ステータス変動効果
    public abstract Action SpecialEffect { get;} //スキル固有の独特な効果
}
/// <summary>
/// ステータスに影響を与える数値をInterface経由で取得するためのクラス。
/// </summary>
public abstract class SkillEffectBase
{
    public abstract string Name { get;} //アドレスとして利用するスキル名。Enumに変更したい。
}