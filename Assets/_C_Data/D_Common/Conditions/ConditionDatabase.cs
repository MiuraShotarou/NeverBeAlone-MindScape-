using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ó‘ÔˆÙí‚ğÀ‘•‚µ‚½‚ç‚±‚±‚É’Ç‰Á
/// </summary>
public class ConditionDatabase
{
    public static Dictionary<Condition, ConditionBase> Database { get; private set; } = new Dictionary<Condition, ConditionBase>
    {
        {Condition.Selfharm, new ConditionSelfharm()},

    };

    
}
