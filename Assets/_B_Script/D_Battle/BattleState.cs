using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Roulette,
    /// <summary>戦闘状態：QTE完了</summary>
    RouletteEnd,
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