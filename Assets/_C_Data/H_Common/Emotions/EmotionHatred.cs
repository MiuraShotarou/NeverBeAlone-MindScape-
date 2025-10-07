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

    public float ModifyAttack(int emotionLevel ,float attack)
    {
        switch(emotionLevel)
        {
            case 1:
                return attack + 1f;
            default:
                return attack;
        }
    }
    public float ModifyDefense(int emotionLevel ,float def)
    {
        switch(emotionLevel)
        {
            case 1:
                return def + 4f;
            default:
                return def;
        }
    }
    public float ModifyRegen(int emotionLevel ,float regen)
    {
        switch(emotionLevel)
        {
            case 1:
                return regen + 0.05f;
            default:
                return regen;
        }
    }
}