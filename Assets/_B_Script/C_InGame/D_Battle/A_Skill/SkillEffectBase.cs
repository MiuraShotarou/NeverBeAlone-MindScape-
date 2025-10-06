using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 状態異常（Condition）とは別に、スキルが保有しているステータス変動効果を内蔵しているスクリプト。
/// 継承先のクラスからどのインターフェイスを実装しているか確認することで、変動するステータスの種類を判断することが出来る。
/// 判断して、インターフェイスが実装されていれば、中のメソッドを呼び出し加算していく仕組みになっている。
/// </summary>
public abstract class SkillEffectBase : ScriptableObject
{
    [SerializeField]public string EffectName;
    [SerializeField]public ConditionActivationType ApplyType;
    // public abstract void ApplyEffect(BattleUnitBase unit, Action action);
    // public abstract void RemoveEffect(BattleUnitBase unit, Action action);
}