using UnityEngine;

public class SaveTest : MonoBehaviour
{
    public SaveManager saveManager;

    void Start()
    {
        // ���f�[�^�쐬
        SaveData data = new SaveData
        {
            PlayerLevel = 5,
            PlayerName = "TestPlayer"
        };

        // �I�[�g�Z�[�u�Ăяo��
        //saveManager.AutoSave(data);

        // ���[�h���Ċm�F
        //SaveData loaded = saveManager.LoadGame();
        //Debug.Log($"���[�h����: {loaded.playerName}, Lv {loaded.playerLevel}");
    }
}
