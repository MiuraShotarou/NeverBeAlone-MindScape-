using System;
using UnityEngine;
/// <summary> ２つのオーディオソースが互いの再生停止を検知して、片方が再生をはじめるためのスクリプト /// </summary>
public class TestAudioLoop : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource1;
    [SerializeField] AudioSource _audioSource2;
    AudioSource[] _audioSources;
    private int _index;
    private int _Index { get => _index; set{ _index = value; if (_audioSources != null && _audioSources.Length <= _index){ _index = 0;}}}
    void Awake()
    {
        _audioSources = new []{_audioSource1, _audioSource2};
    }
    void Update()
    {
        if (!_audioSource2.isPlaying && !_audioSource1.isPlaying)
        {
            _audioSources[_Index].Play();
            _Index++;
        }
    }
}
