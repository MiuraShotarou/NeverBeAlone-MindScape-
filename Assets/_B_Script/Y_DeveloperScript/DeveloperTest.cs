using UnityEngine;

public class DeveloperTest : MonoBehaviour
{
    [SerializeField, Header("ループ用音源をセットしてください")]
    AudioClip _audioClip;
    AudioSource _audioSource;

    bool _isDone;
    // UnityEditor.DeviceSimulation.ScreenSimulation.CalculateSafeAreaAndCutouts();//KeyNotFound
    // UnityEditor.DeviceSimulation.DeviceSimulatorMain.InitSimulation();
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        _isDone = false;
        // string assemblyPath = @"C:\Program Files\Unity\Hub\Editor\2022.3.61f1\Editor\Data\Managed\UnityEngine\UnityEditor.DeviceSimulatorModule.dll";
        // // var assembly = typeof(UnityEditor.Editor).Assembly;
        // var assembly = Assembly.LoadFile(assemblyPath);
        // if (assembly != null)
        // {
        //     // Debug.Log(assembly.Location);
        // }
        // var screenSimulation = assembly.GetType("UnityEditor.DeviceSimulation.ScreenSimulation");
        // if (screenSimulation != null)
        // {
        //     
        // }
    }

    void Update()
    {
        if (!_audioSource.isPlaying && !_isDone)
        {
            _audioSource.clip = _audioClip;
            _audioSource.Play();
            _audioSource.loop = true;
            _isDone = true;
        }
    }
}
