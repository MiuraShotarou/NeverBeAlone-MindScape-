using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionHatred : EmotionBase, IAttackModifier, IDefenseModifier, IRegenModifier
{
    public EmotionHatred(int level)
    {
        _name = "嫌悪";
        _emotion = Emotion.Hatred;
        _level = level;
        _weakEmotion = Emotion.Anger;
        _resistantEmotion = Emotion.Grudge;
    }

    public float ModifyAttack(float attack)
    {
        float mod = 1 * _level;
        return attack + mod;
    }
    public float ModifyDefense(float def)
    {
        float mod = 4 * _level;
        return def + mod;
    }
    public float ModifyRegen(float regen)
    {
        float mod = 0.05f * _level;
        return regen + mod;
    }
}