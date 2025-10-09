using System.IO;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class SaveManager : MonoBehaviour
{
    private string savePath;
    private DatabaseReference _dbRef;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        Debug.Log("ローカル保存パス: " + savePath);
        _dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /// <summary>
    /// BattleUnitPlayer → ScriptableObject → JSON → Firebase
    /// </summary>
    public void SaveToAll(string userId, SaveData data)
    {
        SaveToLocal(data);
        UploadDataToFirebase(userId, data);
    }

    /// <summary>
    /// JSON をローカルに保存
    /// </summary>
    public void SaveToLocal(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("JSONに保存完了: " + savePath);
    }

    /// <summary>
    /// Firebase にアップロード
    /// </summary>
    public void UploadDataToFirebase(string userId, SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        _dbRef.Child("gameData").Child(userId)
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("Firebase に保存完了");
                else
                    Debug.LogError("Firebase 保存エラー: " + task.Exception);
            });
    }

    /// <summary>
    /// Firebase → JSON → ScriptableObject
    /// </summary>
    public void LoadFromFirebase(string userId, System.Action<SaveData> callback)
    {
        _dbRef.Child("gameData").Child(userId)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    var snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        SaveData data = JsonUtility.FromJson<SaveData>(snapshot.GetRawJsonValue());
                        SaveToLocal(data);
                        Debug.Log("Firebase からデータ取得 → JSON 保存完了");
                        callback?.Invoke(data);
                    }
                    else
                    {
                        Debug.LogWarning("Firebase にデータなし: " + userId);
                        callback?.Invoke(new SaveData());
                    }
                }
                else
                {
                    Debug.LogError("Firebase 取得エラー: " + task.Exception);
                    callback?.Invoke(new SaveData());
                }
            });
    }

    /// <summary>
    /// JSON をローカルから読み込み
    /// </summary>
    public SaveData LoadFromLocal()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("JSONファイルなし: " + savePath);
            return new SaveData();
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log("JSONからロード完了: " + savePath);
        return data;
    }
}

[System.Serializable]
public class SaveData
{
    public int playerExp;
    public int playerLevel;
    public string playerName;
    public int playerHp;
    public int encountCount;
}
