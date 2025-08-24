using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private int _level;
    private int _exp;
    private int _hp;
    private int _maxhp;
    private int _tension;
    private int _maxtension;
    private float _baseAttack;
    private int _baseDefense;
    private int _agility; //敏捷性
    private int _baseEvadeRate;
    private int _baseCriticalRate;
    private int _baseCriticalDamageRate;
    private int _baseFluctuationRate;
    private int _baseTensionUpRate;
    private int _movement; // 行動力
    //private int 気力？

    private int _equipmentCrystal; // 装備品：結晶
    private int _equipmentJewel; // 装備品：宝石

    private List<string> _itemsAccessory; // 所持物：アクセサリ
    private List<string> _itemsUsable; // 所持物：使用アイテム

    private List<string> _skills; // スキル

    private List<string> _smartphone; // スマホ機能
}
