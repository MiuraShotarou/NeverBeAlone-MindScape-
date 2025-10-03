using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateMessage : MonoBehaviour
{
    [SerializeField] TextAsset _currentCSV;
    TextMeshProUGUI _tmp;
    void Start()
    {
        //普通に代入すると、csvの中身がそのまま出力されてしまう
        _tmp = GetComponent<TextMeshProUGUI>();
        string[] lines = _testMessage.text.Split('\n');
        foreach (string line in lines)
        {
            string[] values = line.Split(',');
        }
        _tmp.text = _testMessage.text;
    }

    TextAsset CSVLoad()
    {
        //
    }
}
