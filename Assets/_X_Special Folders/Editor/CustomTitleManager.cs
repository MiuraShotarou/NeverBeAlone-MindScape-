using UnityEngine;
using UnityEditor;
    
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