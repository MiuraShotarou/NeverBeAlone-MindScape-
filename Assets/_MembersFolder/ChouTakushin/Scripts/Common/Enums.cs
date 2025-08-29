using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 感情属性
/// </summary>
[Serializable]
public enum Emotion
{
    /// <summary>感情属性：空虚</summary>
    Void,
    /// <summary>感情属性：怒り</summary>
    Anger,
    /// <summary>感情属性：怨念</summary>
    Grudge,
    /// <summary>感情属性：嫌悪</summary>
    Hatred,
    /// <summary>感情属性：猜疑</summary>
    Suspicion
}
/// <summary>
/// QTE判定結果
/// </summary>
public enum RouletteStatus
{
    Stop, Rolling, Miss, Good, Excellent
}
