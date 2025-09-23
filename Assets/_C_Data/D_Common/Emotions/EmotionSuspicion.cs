using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionSuspicion : EmotionBase, IAttackModifier, IDexModifier, IEvadeRateModifier
{
    public EmotionSuspicion(int level)
    {
        _name = "猜疑";
        _emotion = Emotion.Suspicion;
        _level = level;
    }
    public float ModifyAttack(float attack)
    {
        float mod = 2 * _level;
        return attack + mod;
    }
    public float ModifyDex(float dex)
    {
        float mod = 2 * _level;
        return dex + mod;
    }
    public float ModifyEvadeRate(float evadeRate)
    {
        float mod = 0.1f * _level;
        return evadeRate + mod;
    }
}