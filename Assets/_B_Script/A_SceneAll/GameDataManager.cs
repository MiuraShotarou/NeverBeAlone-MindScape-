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
    [HideInInspector] public float WeekAttackScale = 0.1f;
    [HideInInspector] public float ResistAttacktScale = -0.2f;
    [HideInInspector] public float EmotionMatchScale = 0.4f;
    [HideInInspector] public float CalmWeekAttackScale = 0f;
    [HideInInspector] public float ExcitedWeekAttackScale = 0f;
    [HideInInspector] public float FunWeekAttackScale = 0.1f;
    [HideInInspector] public float AwakeningWeekAttackScale = 0.2f;
    [HideInInspector] public float CalmTensionDownRate = 0f;
    [HideInInspector] public float ExcitedTensionDownRate = 0.1f;
    [HideInInspector] public float FunTensionDownRate = 0.2f;
    [HideInInspector] public float AwakeningTensionDownRate = 0.5f;
}