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
        _weakEmotion = Emotion.Suspicion;
        _resistantEmotion = Emotion.Hatred;
    }

    public float ModifyAttack(int emotionLevel, float attack)
    {
        switch(emotionLevel)
        {
            case 1:
                return attack + 5f;
                break;
        }
    }

    public float ModifyCriticalRate(int emotionLevel, float criticalRate)
    {
        switch(emotionLevel)
        {
            case 1:
                return criticalRate + 0.05f;
                break;
        }
    }
}