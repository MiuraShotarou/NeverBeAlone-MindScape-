using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// ゲーム全体で保持する必要のあるデータを格納する。
/// </summary>
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }
    public PlayerData PlayerData;
    [Header("ゲーム内に登場する敵")]
    public EnemyData[] EnemyDataArray;

    private void Awake()
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
    
    private void Start()
    {
        _enemySpawnSpecification = File.ReadAllLines($"{Application.streamingAssetsPath}/W_CSV_Streaming/EnemySpawnSpecification.csv");
        // 
        // BattleUnitBase[] = 
    }

    public void Initialize(PlayerData playerData)
    {
        PlayerData = playerData;
    }
    public EnemyData[] GetEnemyData((int Infiltration, int EncounterCount) progress)
    {
        // 現在のprogressからなんの敵を出現させるべきかを決定する。
        // 敵の情報がBattleUnitEnemyBase[] に格納されていたとすると要素.name
        // _enemySpawnSpecification.
                                                ////<潜入○回目m>から何行下にあるかどうかで検索（取得）しなければならない
        // string[] enemyArray = //csvから取得した
        // BattleUnitEnemyBase[] AllBattleUnitEnemy;
        // List<BattleUnitEnemyBase> decideUnitEnemy = EnemyArray.Where(enemy => enemy.name.Contains(enemyArray));
        
        return null;
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
    //<出現する敵の情報>
    private string[] _enemySpawnSpecification;
}