using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public abstract class ConditionBase : ScriptableObject
{
    protected Condition _condition;
    /// <summary>�p���^�[����</summary>
    protected int _activeTurns = 1;
    protected ConditionActivationType _type;
    protected BattleUnitBase _target = default;

    /// <summary>�R���X�g���N�^�B��Ԉُ�̃N���X�ƃr�b�g�t���O��R�Â���</summary>
    public ConditionBase(Condition condition)
    {
        _condition = condition;
    }

    /// <summary>��Ԉُ��t�^�B�K�v�ȃp�����[�^�[�擾��</summary>
    public abstract void ApplyCondition();

    /// <summary>������Ԉُ��t�^���ꂽ�Ƃ�</summary>
    public abstract void ReapplyCondition();

    /// <summary>��Ԉُ�̌��ʂ���������u�Ԃ̏���</summary>
    public abstract void ActivateConditionEffect();

    /// <summary>��Ԉُ������</summary>
    public abstract void RemoveCondition();

    /// <summary>�^�[�Q�b�g�ɏ�Ԉُ��t�^���ăr�b�g�t���O�ɒǉ�</summary>
    /// <param name="targetCondition"></param>
    public void ApplyConditionToTarget(BattleUnitBase target)
    {
        _target = target;
        Condition targetCondition = target.ConditionFlag;

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

    /// <summary>�^�[�Q�b�g�̏�Ԃ�߂��ăr�b�g�t���O����폜</summary>

    public void RemoveConditionFromTarget(BattleUnitBase target)
    {
        Condition targetCondition = target.ConditionFlag;
        RemoveCondition();
        targetCondition &= ~_condition;
        Debug.Log(_condition + "��Ԃ���������܂���");
    }
}
