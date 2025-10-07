using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionVoid : EmotionBase
{
    public EmotionVoid(int level)
    {
        _name = "空虚";
        _emotion = Emotion.Void;
        _level = level;
    }
}