using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class VectorExtensions
{
    public static string ToDebugString(this Vector2Int vector) => $"[{vector.x}, {vector.y}]";

    public static string ToDebugString(this IEnumerable<Vector2Int> vectors) => vectors.Select(v => v.ToDebugString()).Join(", ");

    public static bool IsAdjacentTo(this Vector2Int lhs, Vector2Int rhs) => (lhs.x == rhs.x && Mathf.Abs(lhs.y - rhs.y) == 1) || (lhs.y == rhs.y && Mathf.Abs(lhs.x - rhs.x) == 1);
}