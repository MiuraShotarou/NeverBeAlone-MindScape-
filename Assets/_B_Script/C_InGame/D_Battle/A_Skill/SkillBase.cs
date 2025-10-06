using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// スキルの共有パラメータ。 SkillEffectをどう扱っていくのか　→　前提 SkillEffectはスキルごとに実装を持つ必要がある → 実装を書けるのはスクリプトの中のみ
/// SkillEffectを排除したとして、インターフェイスのみの実装だとステータス以外の機能を実装することができない
/// そのターンのみ発動する「スキルとしての効果」を別で実装する必要がある
/// </summary>
public abstract class SkillBase : ScriptableObject//,INeedTensionModifier 
{
    //アドレスとして利用する。最適化を狙うならenumにしても良い
    public abstract string SkillName { get;}
    public abstract string SkillDescription { get;} //UI用の文字列
    public abstract Tension NeedTension { get;} //スキル使用に必要なテンション。スキルのレベルによって変動するため引数にintを持ったメソッドにする必要がある。
    public abstract SkillEffect SkillEffect { get;} //ステータス変動効果
}