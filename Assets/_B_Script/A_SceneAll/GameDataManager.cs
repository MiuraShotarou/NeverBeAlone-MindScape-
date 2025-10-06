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
        if (Instance != null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void Initalize(PlayerData playerData)
    {
        PlayerData = playerData;
    }
}