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
        var polyRenderer = PolyominoRenderer.Instantiate(new Polyomino(new Vector2Int[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, 1) }), _monominoPrefab);
        polyRenderer.transform.parent = _holder;
        polyRenderer.transform.localScale = Vector3.one;
        polyRenderer.transform.localRotation = Quaternion.identity;
        polyRenderer.transform.localPosition = Vector3.zero;
    }
}
