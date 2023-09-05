using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyominoRenderer : MonoBehaviour
{
    private Dictionary<Vector2Int, MonominoRenderer> _monominoes = new Dictionary<Vector2Int, MonominoRenderer>();

    private Transform _container;
    private Vector3 _centre;

    public MonominoRenderer GetMonomino(Vector2Int position) => _monominoes[position];

    // Each PolyominoRenderer houses a "container" which, in turn, contains the monominoes.
    // The "container" is placed such that the bottom-left of the bounding box of the monominoes is at the renderer's origin.
    public static PolyominoRenderer Instantiate(Polyomino polyomino, MonominoRenderer monominoPrefab, Transform parent) {
        var renderer = new GameObject("Polyomino").AddComponent<PolyominoRenderer>();
        renderer._container = new GameObject("Container").transform;
        int maxX = 0;
        int maxY = 0;

        foreach (Vector2Int position in polyomino.GetPositionsFromOrigin()) {
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);

            MonominoRenderer monomino = Instantiate(monominoPrefab, renderer._container);
            monomino.transform.localPosition = new Vector3(position.x, 0, position.y);
            renderer._monominoes.Add(position, monomino);
        }

        renderer._container.parent = renderer.transform;
        renderer.transform.parent = parent;
        renderer.transform.localScale = Vector3.zero;
        renderer.transform.localRotation = Quaternion.identity;

        renderer._centre = new Vector3(maxX / 2f, 0, maxY / 2f);
        renderer._container.localPosition = renderer._centre;
        foreach (MonominoRenderer monomino in renderer._monominoes.Values)
            monomino.transform.localPosition -= renderer._centre;

        return renderer;
    }

    public void SetLocalPosition(Vector3 position) => transform.localPosition = position;

    public void Rotate(Rotation direction) {
        switch (direction) {
            case Rotation.Clockwise: _container.Rotate(Vector3.up * 90); break;
            case Rotation.Counterclockwise: _container.Rotate(Vector3.up * -90); break;
            default: throw new System.InvalidOperationException($"Unexpected Rotation value: {direction}.");
        }
        _centre = new Vector3(_centre.z, 0, _centre.x);
        _container.localPosition = _centre;
    }
}
