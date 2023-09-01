using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolyominoGrid
{
    private Vector2Int[] _gridPositions;
    private List<Vector2Int> _occupiedPositions;
    private Dictionary<Polyomino, Vector2Int> _polyominoOrigins;

    public PolyominoGrid(Vector2Int[] positions) {
        if (positions.Length == 0)
            throw new InvalidOperationException("No positions provided to PolyominoGrid constructor.");
        if (positions.Length != positions.Distinct().Count())
            throw new InvalidOperationException($"Duplicate positions ({positions.ToDebugString()}) were provided to the PolyominoGrid constructor.");
        if (!positions.AreContiguousPositions())
            throw new InvalidOperationException($"The positions provided to the PolyominoGrid constructor ({positions.ToDebugString()}) are not all connected.");

        var bottomLeftCorner = new Vector2Int(positions.Select(pos => pos.x).Min(), positions.Select(pos => pos.y).Min());
        _gridPositions = positions.Select(pos => pos - bottomLeftCorner).ToArray();
    }

    public bool TryAdd(Polyomino polyomino, Vector2Int position) {
        Vector2Int[] polyominoPositions = polyomino.GetPositionsFromOrigin(position);

        if (polyominoPositions.Any(pos => !_gridPositions.Contains(pos) || _occupiedPositions.Contains(pos)))
            return false;

        _occupiedPositions.AddRange(polyominoPositions);
        _polyominoOrigins.Add(polyomino, position);
        return true;
    }

    public void Remove(Polyomino polyomino) {
        if (!_polyominoOrigins.ContainsKey(polyomino))
            throw new InvalidOperationException("Tried to remove a polyomino which was not present in the grid.");

        foreach (Vector2Int position in polyomino.GetPositionsFromOrigin(_polyominoOrigins[polyomino]))
            _occupiedPositions.Remove(position);
        _polyominoOrigins.Remove(polyomino);
    }
}