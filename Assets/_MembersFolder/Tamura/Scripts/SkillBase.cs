using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    /// <summary>
    /// �X�L���̊��N���X
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
    /// �X�L���̔������ʂ���������B
    /// </summary>
    public abstract void SkillAct();
}
