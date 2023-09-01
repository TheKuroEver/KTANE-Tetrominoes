using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyominoSelector : MonoBehaviour
{
    [SerializeField] private MonominoRenderer _monominoPrefab;
    private Transform _holder;

    private void Awake() {
        // * This holder object should probably get its own class at some point.
        _holder = ((GameObject)Instantiate(new GameObject("Holder"), transform, instantiateInWorldSpace: false)).transform;
    }
}
