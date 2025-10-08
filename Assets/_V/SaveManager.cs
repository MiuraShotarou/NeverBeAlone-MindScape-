using UnityEngine;
using System.IO;
// using Firebase.Database;
// using Firebase.Extensions;

/// <summary>
/// �Q�[���f�[�^�̕ۑ��ƃ��[�h��S������}�l�[�W���[�B
/// �퓬�O�I�[�g�Z�[�u�͏�Ƀ��[�J��Json�ɕۑ��B
/// �f�[�^�ڍs���̂�Firebase����擾���ă��[�J���ɏ㏑������B
/// </summary>
public class SaveManager : MonoBehaviour
{
    private string savePath; // json�����锠

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savedata.json");
        Debug.Log("�ۑ��p�X: " + savePath);
    }

    /// <summary>
    /// �퓬�O�ɌĂԃI�[�g�Z�[�u�B��Ƀ��[�J��Json�ɕۑ��B
    /// </summary>
    public void AutoSave(SaveData data)
    {
        SaveToLocal(data);
        Debug.Log("�퓬�O�I�[�g�Z�[�u�����i���[�J��Json�j");
    }

    /// <summary>
    /// �Q�[���J�n���ɌĂԃ��[�h�B
    /// Json�����݂���Γǂݍ��݁A�Ȃ���ΐV�K�f�[�^��Ԃ��B
    /// </summary>
    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("�Q�[���J�n���Ƀ��[�J��Json���烍�[�h");
            return data;
        }

        return new SaveData();
    }

    /// <summary>
    /// Json�t�@�C���Ƀf�[�^��ۑ��i�㏑���j�B
    /// </summary>
    private void SaveToLocal(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
    public SaveData Load()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (!File.Exists(path))
        {
            Debug.Log("SaveData�t�@�C�������݂��܂���");
            return null;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log("SaveData�����[�h���܂���: " + path);
        return data;
    }

    /// <summary>
    /// ���[�J��Json��Firebase�ɃA�b�v���[�h�i�f�[�^�ڍs�p�j
    /// </summary>
    public void UploadDataToFirebase(string userId)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("���[�J���f�[�^�����݂��܂���B�A�b�v���[�h���X�L�b�v���܂��B");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        string jsonToSave = JsonUtility.ToJson(data, true);

        // DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        // dbRef.Child("gameData").Child(userId)
        //     .SetRawJsonValueAsync(jsonToSave)
        //     .ContinueWithOnMainThread(task =>
        //     {
        //         if (task.IsCompleted)
        //             Debug.Log("���[�J���f�[�^��Firebase�ɃA�b�v���[�h���܂���");
        //         else
        //             Debug.LogError("Firebase�A�b�v���[�h���s: " + task.Exception);
        //     });
    }

    /// <summary>
    /// �f�[�^�ڍs���ɌĂԁB
    /// Firebase����擾�����f�[�^�����[�J��Json�ɏ㏑���B
    /// </summary>

    public void MigrateDataFromFirebase(string userId)
    {
        // FirebaseDatabase.DefaultInstance.RootReference
        //     .Child("gameData").Child(userId)
        //     .GetValueAsync().ContinueWithOnMainThread(task =>
        //     {
        //         if (task.IsCompleted)
        //         {
        //             var snapshot = task.Result;
        //             if (snapshot.Exists)
        //             {
        //                 SaveData data = JsonUtility.FromJson<SaveData>(snapshot.GetRawJsonValue());
        //                 SaveToLocal(data);
        //                 Debug.Log("Firebase����f�[�^�ڍs�����i���[�J��Json�㏑���j");
        //             }
        //             else
        //             {
        //                 Debug.LogWarning($"Firebase�Ƀ��[�U�[ {userId} �̃f�[�^�����݂��܂���");
        //             }
        //         }
        //         else
        //         {
        //             Debug.LogError("Firebase�f�[�^�擾�Ɏ��s: " + task.Exception);
        //         }
        //     });
    }


}

/// <summary>
/// �ۑ�����Q�[���f�[�^�̍\��
/// </summary>
[System.Serializable]
public class SaveData
{
    // Save�������X�e�[�^�X���������ɑ��
    public int playerExp;
    public int playerLevel;
    public string playerName;
    public int playerHp;
    public Vector3 playerPosition;
}
