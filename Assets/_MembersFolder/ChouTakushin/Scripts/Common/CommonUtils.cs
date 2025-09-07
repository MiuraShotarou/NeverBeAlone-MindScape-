using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム全体で使える共通処理
/// </summary>
public static class CommonUtils
{
    public static float GetDamageScale(Emotion source, Emotion target)
    {
        // TODO 未実装
        return 1;
    }

    /// <summary>
    /// 共通部品：決まったフォーマットでログを出力する
    /// フォーマット：{クラス名}.{メソッド名}: {メッセージ}
    /// </summary>
    /// <param name="t">呼び出す側のobject自身</param>
    /// <param name="methodName">呼び出す側のメソッド名</param>
    /// <param name="message">ユーザー指定のメッセージ</param>
    /// <typeparam name="T"></typeparam>
    public static void LogDebugLine<T>(T t, string methodName, string message)
    {
        Debug.Log(t.GetType() + "." + methodName + ": " + message);
    }

    public static float CalcFinalDamage(BattleUnitBase unit)
    {
        float rst;
        
        // rst = (unit.AttackBase + unit.AttackModified)
        return 0f;
    }
    public static float CalcFinalDefense(BattleUnitBase unit)
    {
        return 0f;
    }

    public static float GetDamageBlur()
    {
        return Random.Range(0.95f, 1.05f);
    }
}
