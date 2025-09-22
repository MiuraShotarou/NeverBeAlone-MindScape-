using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ConditionModifier
{
    /// <summary>
    /// スキルの基底クラス
    /// </summary>

    [SerializeField] public SkillData skillData;
    //public Player player;
    //public Enemy enemy;
    void Start()
    {
        //player = FindObjectOfType<Player>();
        //enemy = FindObjectOfType<Enemy>();
    }
    /// <summary>
    /// スキルの発動効果を実装する。
    /// </summary>
    public abstract void SkillAct();
}