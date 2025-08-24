using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class TestRoad : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _roadText;
    private string savePath;
    /// <summary>
    /// 1:予め決められているセーブデータのパスを代入
    /// 2:セーブデータのパスを参考に、パス先に指定されているファイルの内容をすべて一行の文字列に変換する
    /// 3:TMPにセーブデータの内容を文字列で表示する
    /// </summary>
    public void DataRoad()
    {
        savePath = Path.Combine(Application.persistentDataPath, "TestSaveData.json");
        string json = File.ReadAllText(savePath);
        // SaveData data = JsonUtility.FromJson<SaveData>(json);
        _roadText.text = json;
        Debug.Log($"セーブデータのロード先のパスは{savePath}");
    }
}
