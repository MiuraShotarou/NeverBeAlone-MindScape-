using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateMessage : MonoBehaviour
{
    [SerializeField] TextAsset _csv;
    TextMeshProUGUI _tmp;
    string[] _textArray;
    void Start()
    {
        //普通に代入すると、csvの中身がそのまま出力されてしまう
        //string( or int )をキーに指定したDictionaryを作成する
        _tmp = GetComponent<TextMeshProUGUI>();
        _textArray = _csv.text.Split('\n');
        foreach (string text in _textArray)
        {
            // string[] values = .Split(',');
        }
        _tmp.text = _csv.text;
    }
    // TextAsset CSVLoadAddressable()
    // {
    //     //
    // }
}
