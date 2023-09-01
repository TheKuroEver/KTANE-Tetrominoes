using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyominoRenderer : MonoBehaviour
{
    private MonominoRenderer[] _monominoes;

    public static PolyominoRenderer Instantiate(Transform parent, Vector2Int position, Polyomino polyomino, MonominoRenderer monominoPrefab) {
        var renderer = new GameObject("Polyomino").AddComponent<PolyominoRenderer>();
        renderer.transform.parent = parent;
        renderer.transform.localScale = Vector3.one;
        renderer.transform.localPosition = new Vector3(position.x, 0, position.y);

        foreach (Vector2Int pos in polyomino.GetPositionsFromOrigin()) {
            var monomino = Instantiate(monominoPrefab, renderer.transform);
            monomino.transform.localPosition = new Vector3(pos.x, 0, pos.y);
        }

        return renderer;
    }
}
