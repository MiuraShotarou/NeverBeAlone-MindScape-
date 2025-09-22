using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SkillBase���p��
/// </summary>
public class ConditionModifier
{
    public void AddCondition(Condition targetCondition, Condition condition)
    {
        targetCondition |= condition;
        Debug.Log(condition + "��ԂɂȂ�܂���");
    }

    public void RemoveCondition(Condition targetCondition, Condition condition)
    {
        targetCondition &= ~condition;
        Debug.Log(condition + "��Ԃ���������܂���");
    }
}
