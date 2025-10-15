using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptables/Create EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("コード内での敵の名前")]
    public string Name;
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
    [Header("スキル")]
    public (string SkillName, int SkillLevel)[] SkillArray;
    [Header("ユーザー用の敵の名前")] 
    public string ClientName;
    
    [Header("敵の感情")]
    public Emotion Emotion;
}