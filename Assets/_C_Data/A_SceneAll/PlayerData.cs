using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーデータ
/// ScriptableObjectとしてアセット化して管理する
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptables/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("プレイヤーステータス")]
    public int Level;
    public int Exp;
    public (int Infiltration, int EncounterCount) EncounterCount; //単純なint 型から異世界潜入 / 敵との遭遇回数 に変更

    [Header("能力値")]
    public int Hp;
    public int MaxHp;
    public int BaseAttack;
    public int BaseDefense;
    public int BaseAgility;
    public int BaseEvadeRate;
    public int BaseCriticalRate;
    public int BaseCriticalScale;
    public float BaseHealScale;
    public int BaseTensionUpRate;

    [Header("装備品")]
    public int EquipmentCrystal;
    public int EquipmentJewel;

    [Header("所持品")]
    public List<string> ItemsAccessory = new List<string>();
    public List<string> ItemsUsable = new List<string>();
    
    [Header("スキル")]
    public (string SkillName, int SkillLevel)[] SkillArray; //戦闘システムに適用していない

    [Header("感情レベル")]
    public int[] EmotionLevelArray = new []{1, 1, 1, 1, 1};

    [Header("スマホ機能")]
    public List<string> Smartphone = new List<string>();
}