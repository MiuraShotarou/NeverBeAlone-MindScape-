using UnityEngine;
using UnityEditor;
/// <summary>
/// AudioManagerのエディタ拡張。Inspectorからあらゆる音源を再生できるようにする。
/// </summary>
[CustomEditor(typeof(AudioManager))]
public class CustomAudioManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        AudioManager audioManager = (AudioManager)target;
        AudioSource audioSource = audioManager._AudioSource;
        EditorGUILayout.LabelField("サウンドを再生/停止します");
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
        EditorGUILayout.LabelField("サウンドのクリップを変更します");
        if (GUILayout.Button("SwichClip"))
        {
            if (audioManager._AllAudioClips.Count != 0
                &&
                audioSource.isPlaying)
            {
                audioSource.Stop();
                int audioClipIndex = audioManager._AllAudioClips.FindIndex(clip => clip == audioSource.clip);
                audioClipIndex = audioClipIndex >= audioManager._AllAudioClips.Count -1 ? 0 : audioClipIndex + 1;
                audioSource.clip = audioManager._AllAudioClips[audioClipIndex];
                audioSource.Play();
            }
            else if (audioManager._AllAudioClips.Count != 0)
            {
                int audioClipIndex = audioManager._AllAudioClips.FindIndex(clip => clip == audioSource.clip);
                audioClipIndex = audioClipIndex >= audioManager._AllAudioClips.Count -1 ? 0 : audioClipIndex + 1;
                audioSource.clip = audioManager._AllAudioClips[audioClipIndex];
            }
        }
        EditorGUILayout.LabelField("サウンドのループをオン/オフに切り替えます");
        if (GUILayout.Button("Loop / UnLoop"))
        {
            audioSource.loop = !audioSource.loop;
        }
    }
}