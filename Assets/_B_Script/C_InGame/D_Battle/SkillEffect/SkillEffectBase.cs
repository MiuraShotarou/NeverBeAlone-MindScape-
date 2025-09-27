using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillEffectBase : ScriptableObject
{
    [SerializeField]public string EffectName;
    [SerializeField]public ConditionActivationType ApplyType;
    // public abstract void ApplyEffect(BattleUnitBase unit, Action action);
    // public abstract void RemoveEffect(BattleUnitBase unit, Action action);
}