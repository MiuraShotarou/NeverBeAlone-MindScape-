using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�L���̋��L�p�����[�^
/// </summary>
[CreateAssetMenu(menuName = "Scriptables/Create SkillData")]
public class SkillData : ScriptableObject
{
    [Header("�X�L����")]
    public string skillName;
    [Header("�X�L�����ʐ���")]
    public string skillDescription;
    [Header("���C�x�R�X�g")]
    public int sanityCost;
    [Header("������")]
    public int accuracy;
    [Header("�U���{��")]
    public int atkMult;
    [Header("�h��{��")]
    public int defMult;
    [Header("�ǉ����ʊm��")]
    public int effectProbability;
    [Header("�X�e�[�^�X�㏸�{��")]
    public float statusBoost;
    [Header("�X�e�[�^�X���~�{��")]
    public float statusDecrease;
}
