using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
public class TitleManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip[] _environmentClips;
    [SerializeField] AudioClip[] _seClips;
    List<AudioClip> _allAudioClips = new List<AudioClip>();
    int _audioClipIndex;
    TitleManager _titleManager;
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
        for (int i = 0; i < _seClips.Length; i++)
        {
            _allAudioClips.Add(_seClips[i]);
        }
        if (_AudioSource.clip == null
            &&
            _AllAudioClips.Count != 0)
        {
            _AudioSource.clip = _allAudioClips[0];
        }
    }
}
