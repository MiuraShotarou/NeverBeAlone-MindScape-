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
    public int level;
    public int exp;
    public int hp;
    public int maxHp;
    public int tension;
    public int maxTension;

    [Header("能力値")]
    public float baseAttack;
    public int baseDefense;
    public int agility;
    public int baseEvadeRate;
    public int baseCriticalRate;
    public int baseCriticalDamageRate;
    public int baseFluctuationRate;
    public int baseTensionUpRate;
    public int movement;

    [Header("装備品")]
    public int equipmentCrystal;
    public int equipmentJewel;

    [Header("所持品")]
    public List<string> itemsAccessory = new List<string>();
    public List<string> itemsUsable = new List<string>();

    [Header("スキル")]
    public Dictionary<string, int> skillDict = new Dictionary<string, int>();

    [Header("スマホ機能")]
    public List<string> smartphone = new List<string>();
}
