using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SkillBase‚ªŒp³
/// </summary>
public class ConditionModifier
{
    public void AddCondition(Condition targetCondition, Condition condition)
    {
        targetCondition |= condition;
        Debug.Log(condition + "ó‘Ô‚É‚È‚è‚Ü‚µ‚½");
    }

    public void RemoveCondition(Condition targetCondition, Condition condition)
    {
        targetCondition &= ~condition;
        Debug.Log(condition + "ó‘Ô‚ª‰ğœ‚³‚ê‚Ü‚µ‚½");
    }
}
