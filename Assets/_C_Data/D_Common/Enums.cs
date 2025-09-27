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
[Serializable]
public enum RouletteStatus
{
    Stop, Rolling, Miss, Good, Excellent
}

/// <summary>
/// 効果適用種別
/// </summary>
[Serializable]
public enum ConditionActivationType
{
    Always,
    OnBattleStart,
    OnTurnStart,
    OnTurnEnd,
    OnAttackExecute,
    OnAttackHit
}

/// <summary>
/// 状態異常（バフ・デバフ）
/// </summary>
[Serializable]
[Flags]
public enum Condition
{
    None = 0,
    /// <summary>自傷</summary>
    Selfharm = 1 << 0,
    /// <summary>ドレイン</summary>
    Drain = 1 << 1,
    /// <summary>心蝕</summary>
    Gnaw = 1 << 2,
    /// <summary>縁結び</summary>
    Matchaking = 1 << 3,
    /// <summary>洗脳</summary>
    Brainwash = 1 << 4,
    /// <summary>喪失</summary>
    Loss = 1 << 5,
    /// <summary>レジスト</summary>
    Resist = 1 << 6,
    /// <summary></summary>アーマー
    Armor = 1 << 7,
    /// <summary>バリア</summary>
    Barrier = 1 << 8,
    /// <summary>シールド</summary>
    Shield = 1 << 9,
    /// <summary>免疫</summary>
    Immunity = 1 << 10,
    /// <summary>抗体</summary>
    Antibody = 1 << 11,
    /// <summary>スパイク</summary>
    Spike = 1 << 12,
    /// <summary>気力低下</summary>
    Depression = 1 << 13,
    /// <summary>カウンター</summary>
    Counter = 1 << 14,
    /// <summary>不安</summary>
    Anxiety = 1 << 15,
    /// <summary>束縛</summary>
    Restraint = 1 << 16,
    /// <summary>囮（おとり）</summary>
    Decoy = 1 << 17,
}
