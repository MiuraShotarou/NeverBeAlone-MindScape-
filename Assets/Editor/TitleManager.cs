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

[CustomEditor(typeof(TitleManager))]
public class CustomTitleManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TitleManager titleManager = (TitleManager)target;
        AudioSource audioSource = titleManager._AudioSource;
        if (GUILayout.Button("Play / StopAudio")
            &&
            audioSource.clip)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            else
            {
                audioSource.Play();
            }
        }
        if (GUILayout.Button("SwichClip"))
        {
            if (titleManager._AllAudioClips.Count != 0
                &&
                audioSource.isPlaying)
            {
                audioSource.Stop();
                int audioClipIndex = titleManager._AllAudioClips.FindIndex(clip => clip == audioSource.clip);
                audioClipIndex = audioClipIndex >= titleManager._AllAudioClips.Count -1 ? 0 : audioClipIndex + 1;
                audioSource.clip = titleManager._AllAudioClips[audioClipIndex];
                audioSource.Play();
            }
        }
    }
}
