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
        _weakEmotion = Emotion.Grudge;
        _resistantEmotion = Emotion.Anger;
    }

    public float ModifyAttack(int emotionLevel, float attack)
    {
        switch(emotionLevel)
        {
            case 1:
                return attack + 2f;
                break;
        }
    }
    public float ModifyDex(int emotionLevel, float dex)
    {
        switch(emotionLevel)
        {
            case 1:
                return dex + 2f;
                break;
        }
    }
    public float ModifyEvadeRate(int emotionLevel, float evadeRate)
    {
        switch(emotionLevel)
        {
            case 1:
                return evadeRate + 0.1f;
                break;
        }
    }
}