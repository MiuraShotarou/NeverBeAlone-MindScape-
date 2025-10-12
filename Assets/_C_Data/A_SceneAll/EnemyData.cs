using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptables/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("コード内での敵の名前")]
    public string Name;
    [Header("能力値")]
    public int BaseAttack;
    public int BaseDefense;
    public int Agility;
    public int BaseEvadeRate;
    public int BaseCriticalRate;
    public int BaseCriticalDamageRate;
    public int BaseFluctuationRate; //不要
    public int Movement;
    [Header("敵の基本ステータス")]
    public int Hp;
    public int MaxHp;
    [Header("ユーザー用の敵の名前")] 
    public string ClientName;

    
    [Header("敵の感情")]
    public Emotion Emotion;
    
    [Header("スキル")]
    public Dictionary<string, int> SkillDict = new Dictionary<string, int>();
}