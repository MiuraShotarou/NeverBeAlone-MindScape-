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
/// テンションランク（興奮度・エネルギーレベル）
/// </summary>
[Serializable]
public enum TensionRunk
{
    /// <summary>平静状態</summary>
    Calm,
    /// <summary>興奮状態</summary>
    Excited,
    /// <summary>楽しい状態</summary>
    Joyful,
    /// <summary>覚醒状態</summary>
    Awakened
}
/// <summary>
/// QTE判定結果
/// </summary>
[Serializable]
public enum ResultQTE
{
    //stop, rolling 削除の方向性を検討
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
/// <summary>
/// 戦闘時フェーズ
/// </summary>
[Serializable]
public enum BattleState
{
    /// <summary>戦闘状態：待機</summary>
    Idle,
    /// <summary>戦闘状態：初期化</summary>
    Init,
    /// <summary>戦闘状態：戦闘開始</summary>
    Intro,
    /// <summary>戦闘状態：QTE中</summary>
    QTE,
    /// <summary>戦闘状態：QTE完了</summary>
    QTEFinished,
    /// <summary>戦闘状態：ターン開始</summary>
    TurnStart,
    /// <summary>戦闘状態：コマンド待ち</summary>
    WaitForCommand,
    /// <summary>戦闘状態：コマンド待ち</summary>
    WaitForTargetSelect,
    /// <summary>戦闘状態：行動実行中</summary>
    Busy,
    /// <summary>戦闘状態：Unit死活判定</summary>
    JudgeUnitSurvive,
    /// <summary>ターン終了判定</summary>
    JudgeTurnEnd,
    /// <summary>戦闘状態：ターン終了</summary>
    TurnEnd,
    /// <summary>戦闘状態：戦闘終了</summary>
    BattleEnd,
    /// <summary>戦闘状態：勝利</summary>
    Victory,
    /// <summary>戦闘状態：GameOver</summary>
    GameOver
}
