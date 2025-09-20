using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Material _material = default;
    Renderer _renderer = default;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().sharedMaterial;
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float minY = _renderer.bounds.min.y;
        float maxY = _renderer.bounds.max.y;
        //Debug.Log(minY);
        //Debug.Log(maxY);
        //Debug.Log(_material.GetVector("_YBound"));
        _material.SetVector("_YBound", new Vector2(minY, maxY));
    }
}