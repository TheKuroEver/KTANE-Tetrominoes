using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MonominoRenderer : MonoBehaviour
{
    private MeshRenderer _renderer;

    public Color Colour {
        get { return _renderer.material.color; }
        set { _renderer.material.color = value; }
    }
    
#pragma warning disable IDE0051
    private void Awake() => _renderer = GetComponent<MeshRenderer>();
}