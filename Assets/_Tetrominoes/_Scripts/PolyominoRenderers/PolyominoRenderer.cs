using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyominoRenderer : MonoBehaviour
{
    private Dictionary<Vector2Int, MonominoRenderer> _monominoes = new Dictionary<Vector2Int, MonominoRenderer>();

    public Vector2 Centre { get; private set; }

    public MonominoRenderer GetMonomino(Vector2Int position) => _monominoes[position];

    public static PolyominoRenderer Instantiate(Polyomino polyomino, MonominoRenderer monominoPrefab) {
        var renderer = new GameObject("Polyomino").AddComponent<PolyominoRenderer>();
        int maxX = 0;
        int maxY = 0;

        foreach (Vector2Int position in polyomino.GetPositionsFromOrigin()) {
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);

            MonominoRenderer monomino = Instantiate(monominoPrefab, renderer.transform);
            monomino.transform.localPosition = new Vector3(position.x, 0, position.y);
            renderer._monominoes.Add(position, monomino);
        }

        renderer.Centre = new Vector2(maxX / 2f, maxY / 2f);
        return renderer;
    }
}
