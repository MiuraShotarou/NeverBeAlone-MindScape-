using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԉُ�����������炱���ɒǉ�
/// </summary>
public class ConditionDatabase
{
    public static Dictionary<Condition, ConditionBase> Database { get; private set; } = new Dictionary<Condition, ConditionBase>
    {
        {Condition.Selfharm, new ConditionSelfharm()},

    };

    
}
