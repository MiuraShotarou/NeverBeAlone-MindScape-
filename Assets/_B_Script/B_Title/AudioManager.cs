using System.Collections.Generic;
using UnityEngine;
/// <summary> デバッグ機能を搭載したAudioManager /// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField, Header("再生に使用するオーディオ")] AudioSource _audioSource;
    [SerializeField, Header("再生したい環境音")] AudioClip[] _environmentClips;
    [SerializeField, Header("再生したいBGM")] AudioClip[] _bgmClips;
    [SerializeField, Header("再生したいSE")] AudioClip[] _seClips;
    List<AudioClip> _allAudioClips = new List<AudioClip>();
    int _audioClipIndex;
    public AudioSource _AudioSource => _audioSource;
    public List<AudioClip> _AllAudioClips => _allAudioClips;
    /// <summary>
    /// 初期化
    /// </summary>
    void Start()
    {
        for (int i = 0; i < _environmentClips.Length; i++)
        {
            _allAudioClips.Add(_environmentClips[i]);
        }
        for (int i = 0; i < _bgmClips.Length; i++)
        {
            _allAudioClips.Add(_bgmClips[i]);
        }
        for (int i = 0; i < _seClips.Length; i++)
        {
            _allAudioClips.Add(_seClips[i]);
        }
        if (_AudioSource.clip == null
            &&
            _AllAudioClips.Count != 0)
        {
            _AudioSource.clip = _allAudioClips[0];
            _AudioSource.Play();
        }
    }
}
