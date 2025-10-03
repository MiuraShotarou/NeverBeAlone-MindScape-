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
    
    public float ModifyAttack(float attack)
    {
        float mod = 5 * _level;
        return attack + mod;
    }
    public float ModifyDex(float dex)
    {
        float mod = 1 * _level;
        return dex + mod;
    }
}