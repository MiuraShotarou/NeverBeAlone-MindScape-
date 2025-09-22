using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionAnger : EmotionBase, IAttackModifier, ICriticalRateModifier
{
    public EmotionAnger(int level)
    {
        _name = "怒り";
        _emotion = Emotion.Anger;
        _level = level;
    }

    public float ModifyAttack(float attack)
    {
        float mod = 5 * _level;
        return attack + mod;
    }

    public float ModifyCriticalRate(float criticalRate)
    {
        float mod = 0.05f * _level;
        return criticalRate + mod;
    }
}