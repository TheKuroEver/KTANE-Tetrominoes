using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Polyomino
{
    private Vector2Int[] _relativeOccupiedPositions;

    public Polyomino(Vector2Int[] positions) {
        if (positions.Length == 0)
            throw new System.InvalidOperationException("No positions provided to Polyomino constructor.");
        if (positions.Length != positions.Distinct().Count())
            throw new System.InvalidOperationException($"Duplicate positions ({positions.ToDebugString()}) were provided to the Polyomino constructor.");
        if (!positions.AreContiguousPositions())
            throw new System.InvalidOperationException($"The positions provided to the Polyomino constructor ({positions.ToDebugString()}) are not all connected.");
        _relativeOccupiedPositions = positions.ToArray();
        SnapToOrigin();
    }

    private void SnapToOrigin() {
        var bottomLeftCorner = new Vector2Int(_relativeOccupiedPositions.Select(pos => pos.x).Min(), _relativeOccupiedPositions.Select(pos => pos.y).Min());
        _relativeOccupiedPositions = _relativeOccupiedPositions.Select(pos => pos - bottomLeftCorner).ToArray();
    }

    public void Rotate(Rotation direction) {
        switch (direction) {
            case Rotation.Clockwise: _relativeOccupiedPositions = _relativeOccupiedPositions.Select(pos => new Vector2Int(pos.y, -pos.x)).ToArray(); break;
            case Rotation.Counterclockwise: _relativeOccupiedPositions = _relativeOccupiedPositions.Select(pos => new Vector2Int(-pos.y, pos.x)).ToArray(); break;
            default: throw new System.InvalidOperationException($"Unexpected Rotation value: {direction}.");
        }
        SnapToOrigin();
    }

    public Vector2Int[] GetPositionsFromOrigin(Vector2Int origin) => _relativeOccupiedPositions.Select(pos => pos + origin).ToArray();

    public override string ToString() => ToString(origin: Vector2Int.zero);
    public string ToString(Vector2Int origin) => $"<Polyomino: {_relativeOccupiedPositions.Select(pos => pos + origin).ToDebugString()}>";
}