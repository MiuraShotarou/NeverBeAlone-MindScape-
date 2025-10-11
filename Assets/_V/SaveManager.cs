using System;
using System.IO;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

/// <summary>
/// JSON → Firebase への保存・読み込みを担当
/// 匿名サインインは初回のみで UID を保持、以降は同じ UID を使用
/// </summary>
public class SaveManager : MonoBehaviour
{
    string savePath;
    DatabaseReference _dbRef;
    FirebaseAuth _auth;
    string _userId;
    const string UIDKEY = "FirebaseUID"; // PlayerPrefs で保存

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        Debug.Log("ローカル保存パス: " + savePath);

        _auth = FirebaseAuth.DefaultInstance;
        _dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /// <summary>
    /// 初回のみ匿名サインインし、UID を保持する
    /// </summary>
    public void EnsureSignedIn(Action onSignedIn)
    {
        // すでに UID を PlayerPrefs に保持していればそれを利用
        if (PlayerPrefs.HasKey(UIDKEY))
        {
            _userId = PlayerPrefs.GetString(UIDKEY);
            Debug.Log("既存 UID を使用: " + _userId);
            onSignedIn?.Invoke();
            return;
        }

        // 初回のみ匿名サインイン
        _auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                _userId = _auth.CurrentUser.UserId;
                PlayerPrefs.SetString(UIDKEY, _userId);
                PlayerPrefs.Save();

                Debug.Log("Firebase Auth サインイン完了 UID: " + _userId);
                onSignedIn?.Invoke();
            }
            else
            {
                Debug.LogError("Firebase Auth サインイン失敗: " + task.Exception);
            }
        });
    }

    /// <summary>
    /// ローカルと Firebase に保存（戦闘前オートセーブ用）
    /// </summary>
    public void AutoSave(SaveData data)
    {
        SaveToLocal(data);

        if (string.IsNullOrEmpty(_userId))
        {
            Debug.LogWarning("Firebase UID 未取得、保存スキップ");
            return;
        }

        UploadDataToFirebase(_userId, data);
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
    void UploadDataToFirebase(string userId, SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        _dbRef.Child("gameData").Child(userId)
            .SetRawJsonValueAsync(json)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("Firebase に保存完了 UID: " + userId);
                else
                    Debug.LogError("Firebase 保存エラー: " + task.Exception);
            });
    }

    /// <summary>
    /// Firebase → JSON → ScriptableObject
    /// </summary>
    public void LoadFromFirebase(Action<SaveData> callback)
    {
        if (string.IsNullOrEmpty(_userId))
        {
            Debug.LogWarning("Firebase UID 未取得、ロードスキップ");
            callback?.Invoke(new SaveData());
            return;
        }

        _dbRef.Child("gameData").Child(_userId)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    SaveData data = JsonUtility.FromJson<SaveData>(task.Result.GetRawJsonValue());
                    SaveToLocal(data);
                    Debug.Log("Firebase からデータ取得 → JSON 保存完了");
                    callback?.Invoke(data);
                }
                else
                {
                    Debug.LogWarning("Firebase にデータなし UID: " + _userId);
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
