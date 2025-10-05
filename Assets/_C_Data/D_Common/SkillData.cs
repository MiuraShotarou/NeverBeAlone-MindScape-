using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スキルの共有パラメータ
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Create SkillData")]
public class SkillData : ScriptableObject
{
    [Header("スキル名")] //アドレスとして利用する
    public string skillName;
    [Header("スキル効果説明")]
    public string skillDescription; //UI用の文字列
    [Header("正気度コスト")]
    public int sanityCost; //
    [Header("命中率")]
    public int accuracy;
    [Header("攻撃倍率")]
    public int atkMult;
    [Header("防御倍率")]
    public int defMult;
    [Header("追加効果確率")]
    public int effectProbability;
    [Header("ステータス上昇倍率")]
    public float statusBoost;
    [Header("ステータス下降倍率")]
    public float statusDecrease;
}