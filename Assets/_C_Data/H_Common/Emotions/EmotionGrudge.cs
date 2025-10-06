using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionGrudge : EmotionBase, IAttackModifier, IDexModifier
{
    public EmotionGrudge(int level)
    {
        _name = "怨念";
        _emotion = Emotion.Grudge;
        _level = level;
        _weakEmotion = Emotion.Hatred;
        _resistantEmotion = Emotion.Suspicion;
    }
    
    public float ModifyAttack(int emotionLevel ,float attack)
    {
        switch(emotionLevel)
        {
            case 1:
                return attack + 5f;
                break;
        }
    }
    public float ModifyDex(int emotionLevel ,float dex)
    {
        switch(emotionLevel)
        {
            case 1:
                return dex + 1f;
                break;
        }
    }
}