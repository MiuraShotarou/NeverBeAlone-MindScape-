using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class EmotionBase
{
    protected string _name;
    protected Emotion _emotion;
    public Emotion Emotion
    {
        get { return _emotion; }
    }
    protected int _level;
}