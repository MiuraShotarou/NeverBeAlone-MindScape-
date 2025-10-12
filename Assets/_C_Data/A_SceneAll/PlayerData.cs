using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーデータ
/// ScriptableObjectとしてアセット化して管理する
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptables/Create PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("基本ステータス")]
    public int Level;
    public int Exp;
    public int Hp;
    public int MaxHp;
    public int Tension;
    public int MaxTension;
    public (int Infiltration, int EncounterCount) EncounterCount; //単純なint 型から異世界潜入 / 敵との遭遇回数 に変更

    [Header("能力値")]
    public float BaseAttack;
    public int BaseDefense;
    public int Agility;
    public int BaseEvadeRate;
    public int BaseCriticalRate;
    public int BaseCriticalDamageRate;
    public int BaseFluctuationRate;
    public int BaseTensionUpRate;
    public int Movement;

    [Header("装備品")]
    public int EquipmentCrystal;
    public int EquipmentJewel;

    [Header("所持品")]
    public List<string> ItemsAccessory = new List<string>();
    public List<string> ItemsUsable = new List<string>();
    
    [Header("スキル")]
    public Dictionary<string, int> SkillDict = new Dictionary<string, int>(); //戦闘システムに適用していない

    [Header("感情レベル")]
    public int[] EmotionLevelArray = new []{1, 1, 1, 1, 1};

    [Header("スマホ機能")]
    public List<string> Smartphone = new List<string>();
}