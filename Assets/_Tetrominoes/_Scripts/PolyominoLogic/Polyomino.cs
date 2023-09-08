using System;
using System.Linq;
using UnityEngine;

public class Polyomino
{
    private Vector2Int[] _relativeOccupiedPositions;
    private PolyominoRenderer _renderer;
    private int _rotationCount;

    public Polyomino(Vector2Int[] positions, MonominoRenderer monominoPrefab, Transform parent) {
        if (positions.Length == 0)
            throw new InvalidOperationException("No positions provided to Polyomino constructor.");
        if (positions.Length != positions.Distinct().Count())
            throw new InvalidOperationException($"Duplicate positions ({positions.ToDebugString()}) were provided to the Polyomino constructor.");
        if (!positions.AreContiguousPositions())
            throw new InvalidOperationException($"The positions provided to the Polyomino constructor ({positions.ToDebugString()}) are not all connected.");

        _relativeOccupiedPositions = positions.ToArray();
        SnapToOrigin();
        SetDefaultRotation();
        _renderer = PolyominoRenderer.Instantiate(this, monominoPrefab, parent);
    }

    // Any two polyominoes with the same 'shape' (differing only by rotation) will have the same shape string.
    public string Shape { get; private set; }

    public void SetParent(Transform parent) => _renderer.SetParent(parent);
    public void SetPosition(Vector3 position) => _renderer.SetLocalPosition(position);
    public void SetPositionByGeometricCentre(Vector3 position) => _renderer.SetLocalPositionByGeometricCentre(position);

    private void SetDefaultRotation() {
        var possibleFootprints = new string[4];

        for (int rotIx = 0; rotIx < 4; rotIx++) {
            var positions = new string[_relativeOccupiedPositions.Length];
            for (int posIx = 0; posIx < positions.Length; posIx++)
                positions[posIx] = $"{_relativeOccupiedPositions[posIx].x},{_relativeOccupiedPositions[posIx].y}";
            Array.Sort(positions);
            possibleFootprints[rotIx] = $"[{positions.Join()}]";
            MapRotation(Rotation.Clockwise);
        }

        string[] original = possibleFootprints.ToArray();
        Array.Sort(possibleFootprints);
        Shape = possibleFootprints[0];
        _rotationCount = (4 - Array.IndexOf(original, Shape)) % 4;
    }

    private void SnapToOrigin() {
        var bottomLeftCorner = new Vector2Int(_relativeOccupiedPositions.Select(pos => pos.x).Min(), _relativeOccupiedPositions.Select(pos => pos.y).Min());
        _relativeOccupiedPositions = _relativeOccupiedPositions.Select(pos => pos - bottomLeftCorner).ToArray();
    }

    private void MapRotation(Rotation direction) {
        switch (direction) {
            case Rotation.Clockwise: _relativeOccupiedPositions = _relativeOccupiedPositions.Select(pos => new Vector2Int(pos.y, -pos.x)).ToArray(); _rotationCount = (_rotationCount + 1) % 4; break;
            case Rotation.Counterclockwise: _relativeOccupiedPositions = _relativeOccupiedPositions.Select(pos => new Vector2Int(-pos.y, pos.x)).ToArray(); _rotationCount = (_rotationCount + 3) % 4; break;
            default: throw new InvalidOperationException($"Unexpected Rotation value: {direction}.");
        }
    }

    public void Rotate(Rotation direction) {
        MapRotation(direction);
        SnapToOrigin();
        _renderer.Rotate(direction);
    }

    public Vector2Int[] GetPositionsFromOrigin() => GetPositionsFromOrigin(Vector2Int.zero);
    public Vector2Int[] GetPositionsFromOrigin(Vector2Int origin) => _relativeOccupiedPositions.Select(pos => pos + origin).ToArray();

    public override string ToString() => ToString(origin: Vector2Int.zero);
    public string ToString(Vector2Int origin) => $"<Polyomino: {_relativeOccupiedPositions.Select(pos => pos + origin).ToDebugString()}>";
}