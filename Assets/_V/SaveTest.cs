using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveManager saveManager;

    void Start()
    {
        // ����[�^�쐬f
        SaveData data = new SaveData
        {
            playerLevel = 5,
            playerName = "TestPlayer"
        };

        // �I�[�g�Z�[�u�Ăяo��
        saveManager.AutoSave(data);

        // ���[�h���Ċm�F
        SaveData loaded = saveManager.LoadGame();
        Debug.Log($"���[�h����: {loaded.playerName}, Lv {loaded.playerLevel}");
    }
}
