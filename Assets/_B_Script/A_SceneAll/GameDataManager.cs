using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ゲーム全体で保持する必要のあるデータを格納する。
/// </summary>
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }
    public PlayerData PlayerData;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initalize(PlayerData playerData)
    {
        PlayerData = playerData;
    }
    //<ダメージ計算パラメーター>
    public float WeekAttackScale = 0.1f;
    public float ResistAttacktScale = -0.2f;
    public float EmotionMatchScale = 0.4f;
    public float CalmWeekAttackScale = 0f;
    public float ExcitedWeekAttackScale = 0f;
    public float FunWeekAttackScale = 0.1f;
    public float AwakeningWeekAttackScale = 0.2f;
    public float CalmTensionDownRate = 0f;
    public float ExcitedTensionDownRate = 0.1f;
    public float FunTensionDownRate = 0.2f;
    public float AwakeningTensionDownRate = 0.5f;
}