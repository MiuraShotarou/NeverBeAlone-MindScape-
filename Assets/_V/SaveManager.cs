using UnityEngine;
using System.IO;
using Firebase.Database;
using Firebase.Extensions;

/// <summary>
/// ゲームデータの保存とロードを担当するマネージャー。
/// 戦闘前オートセーブは常にローカルJsonに保存。
/// データ移行時のみFirebaseから取得してローカルに上書きする。
/// </summary>
public class SaveManager : MonoBehaviour
{
    private string savePath; // jsonを入れる箱

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        Debug.Log("保存パス: " + savePath);
    }

    /// <summary>
    /// 戦闘前に呼ぶオートセーブ。常にローカルJsonに保存。
    /// </summary>
    public void AutoSave(SaveData data)
    {
        SaveToLocal(data);
        Debug.Log("戦闘前オートセーブ完了（ローカルJson）");
    }

    /// <summary>
    /// ゲーム開始時に呼ぶロード。
    /// Jsonが存在すれば読み込み、なければ新規データを返す。
    /// </summary>
    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("ゲーム開始時にローカルJsonからロード");
            return data;
        }

        return new SaveData();
    }

    /// <summary>
    /// Jsonファイルにデータを保存（上書き）。
    /// </summary>
    private void SaveToLocal(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
    /// <summary>
    /// ローカルJsonをFirebaseにアップロード（データ移行用）
    /// </summary>
    public void UploadDataToFirebase(string userId)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("ローカルデータが存在しません。アップロードをスキップします。");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        string jsonToSave = JsonUtility.ToJson(data, true);

        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        dbRef.Child("gameData").Child(userId)
            .SetRawJsonValueAsync(jsonToSave)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("ローカルデータをFirebaseにアップロードしました");
                else
                    Debug.LogError("Firebaseアップロード失敗: " + task.Exception);
            });
    }

    /// <summary>
    /// データ移行時に呼ぶ。
    /// Firebaseから取得したデータをローカルJsonに上書き。
    /// </summary>

    public void MigrateDataFromFirebase(string userId)
    {
        FirebaseDatabase.DefaultInstance.RootReference
            .Child("gameData").Child(userId)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    var snapshot = task.Result;
                    if (snapshot.Exists)
                    {
                        SaveData data = JsonUtility.FromJson<SaveData>(snapshot.GetRawJsonValue());
                        SaveToLocal(data);
                        Debug.Log("Firebaseからデータ移行完了（ローカルJson上書き）");
                    }
                    else
                    {
                        Debug.LogWarning($"Firebaseにユーザー {userId} のデータが存在しません");
                    }
                }
                else
                {
                    Debug.LogError("Firebaseデータ取得に失敗: " + task.Exception);
                }
            });
    }


}

/// <summary>
/// 保存するゲームデータの構造
/// </summary>
[System.Serializable]
public class SaveData
{
    // Saveしたいステータス等をここに代入
    public int playerLevel;
    public string playerName;
    public int playerHp;
    public Vector3 playerPosition;
}
