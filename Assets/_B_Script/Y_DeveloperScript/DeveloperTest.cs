using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor.DeviceSimulation;

public class DeveloperTest : MonoBehaviour
{
    // UnityEditor.DeviceSimulation.ScreenSimulation.CalculateSafeAreaAndCutouts();//KeyNotFound
    // UnityEditor.DeviceSimulation.DeviceSimulatorMain.InitSimulation();
    void Start()
    {
        string assemblyPath = @"C:\Program Files\Unity\Hub\Editor\2022.3.61f1\Editor\Data\Managed\UnityEngine\UnityEditor.DeviceSimulatorModule.dll";
        // var assembly = typeof(UnityEditor.Editor).Assembly;
        var assembly = Assembly.LoadFile(assemblyPath);
        if (assembly != null)
        {
            // Debug.Log(assembly.Location);
        }
        var screenSimulation = assembly.GetType("UnityEditor.DeviceSimulation.ScreenSimulation");
        if (screenSimulation != null)
        {
            
        }
    }
}
