using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveManager saveManager;

    void Start()
    {
        // 仮データ作成
        SaveData data = new SaveData
        {
            playerLevel = 5,
            playerName = "TestPlayer"
        };

        // オートセーブ呼び出し
        saveManager.AutoSave(data);

        // ロードして確認
        SaveData loaded = saveManager.LoadGame();
        Debug.Log($"ロード結果: {loaded.playerName}, Lv {loaded.playerLevel}");
    }
}
