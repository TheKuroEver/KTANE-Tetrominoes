using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static string ToDebugString(this Vector2Int vector) => $"[{vector.x}, {vector.y}]";

    public static string ToDebugString(this IEnumerable<Vector2Int> vectors) => vectors.Select(v => v.ToDebugString()).Join(", ");

    public static bool IsAdjacentTo(this Vector2Int lhs, Vector2Int rhs) => Mathf.Abs(lhs.x - rhs.x) + Mathf.Abs(lhs.y - rhs.y) == 1;

    public static bool AreContiguousPositions(this IEnumerable<Vector2Int> positions) {
        var connectedSection = new List<Vector2Int> { positions.First() };
        var remainingPositions = positions.Skip(1).ToList();
        List<Vector2Int> newPositions;

        do {
            newPositions = remainingPositions.Where(pos => connectedSection.Any(secPos => pos.IsAdjacentTo(secPos))).ToList();
            connectedSection.AddRange(newPositions);
            remainingPositions = remainingPositions.Except(newPositions).ToList();
        } while (newPositions.Any());

        return connectedSection.Count == positions.ToArray().Length;
    }
}