using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SkillBase���p��
/// </summary>
public abstract class ConditionBase
{
    protected Condition _condition;
    public Condition ConditionName
    {
        get { return _condition; }
        set { _condition = value; }
    }

    /// <summary>��Ԉُ�̌���</summary>
    public abstract void ApplyCondition();

    /// <summary>������Ԉُ��t�^���ꂽ�Ƃ�</summary>
    public abstract void ReapplyCondition();

    /// <summary>��Ԉُ�̉���</summary>
    public abstract void RemoveCondition();

    /// <summary>�^�[�Q�b�g�̃r�b�g�t���O�ɒǉ�</summary>
    /// <param name="targetCondition"></param>
    public void ApplyConditionToTarget(Condition targetCondition)
    {
        if ((targetCondition & _condition) == _condition)
        {
            ReapplyCondition();
        }
        else
        {
            ApplyCondition();
        }
        targetCondition |= _condition;
        Debug.Log(_condition + "��ԂɂȂ�܂���");
    }

    /// <summary>�^�[�Q�b�g�̃r�b�g�t���O����폜</summary>
    
    public void RemoveConditionFromTarget(Condition targetCondition)
    {
        RemoveCondition();
        targetCondition &= ~_condition;
        Debug.Log(_condition + "��Ԃ���������܂���");
    }
}
