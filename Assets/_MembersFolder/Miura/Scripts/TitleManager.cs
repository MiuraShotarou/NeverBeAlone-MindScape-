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
    [SerializeField] AudioClip _currentClip;
    TitleManager _titleManager;
    public AudioSource _AudioSource => _audioSource;
    AudioClip[] _allAudioClips;
    void Start()
    {
        if (_environmentClips.Length != 0) {_allAudioClips.Concat(_environmentClips);}
        if (_seClips.Length != 0) {_allAudioClips.Concat(_seClips);}
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
            if (!audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            else
            {
                audioSource.Play();
            }
            //ボタンを押すと_currentAudioClipが再生される
            //別のボタンを押すと、次のクリップが再生される → いや、指定できるほうが良いかもな
            //
        }
        if (GUILayout.Button("SwichClip"))
        {
            
        }
    }
}
