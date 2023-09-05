using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyominoRenderer : MonoBehaviour
{
    private const float s_rotationTime = .1f;
    private const float s_translationTime = .1f;

    private Dictionary<Vector2Int, MonominoRenderer> _monominoes = new Dictionary<Vector2Int, MonominoRenderer>();

    private Transform _container;
    private Vector3 _centre;

    private Coroutine _containerRotationAnimation;
    private Coroutine _containerTranslationAnimation;
    private Coroutine _parentTranslationAnimation;

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
        renderer.transform.localPosition = Vector3.zero;
        renderer.transform.localScale = Vector3.one;
        renderer.transform.localRotation = Quaternion.identity;

        renderer._centre = new Vector3(maxX / 2f, 0, maxY / 2f);
        renderer._container.localPosition = renderer._centre;
        foreach (MonominoRenderer monomino in renderer._monominoes.Values)
            monomino.transform.localPosition -= renderer._centre;

        return renderer;
    }

    public void SetLocalPosition(Vector3 position) => AnimateTranslation(position);

    public void Rotate(Rotation direction) {
        switch (direction) {
            case Rotation.Clockwise: AnimateRotation(90); break;
            case Rotation.Counterclockwise: AnimateRotation(-90); break;
            default: throw new System.InvalidOperationException($"Unexpected Rotation value: {direction}.");
        }
        _centre = new Vector3(_centre.z, 0, _centre.x);
        StopCoroutineIfNotNull(_containerTranslationAnimation);
        _containerTranslationAnimation = StartCoroutine(AnimateInterpolatableProperty((value) => _container.localPosition = value, _container.localPosition, _centre, Vector3.Lerp, s_translationTime));
    }

    private void AnimateTranslation(Vector3 newPosition) {
        StopCoroutineIfNotNull(_parentTranslationAnimation);
        _parentTranslationAnimation = StartCoroutine(AnimateInterpolatableProperty((value) => transform.localPosition = value, transform.localPosition, newPosition, Vector3.Lerp, s_translationTime));
    }

    private void AnimateRotation(float clockwiseAngle) {
        var oldRotation = _container.localRotation;
        var newRotation = _container.localRotation * Quaternion.AngleAxis(clockwiseAngle, Vector3.up);
        StopCoroutineIfNotNull(_containerRotationAnimation);
        _containerRotationAnimation = StartCoroutine(AnimateInterpolatableProperty((value) => _container.localRotation = value, oldRotation, newRotation, Quaternion.Lerp, s_rotationTime));
    }

    private IEnumerator AnimateInterpolatableProperty<T>(Action<T> setter, T oldValue, T newValue, Func<T, T, float, T> lerper, float totalTime) {
        var elapsedTime = 0f;
        while (elapsedTime < totalTime) {
            setter.Invoke(lerper.Invoke(oldValue, newValue, elapsedTime / totalTime));
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        setter.Invoke(newValue);
    }

    private void StopCoroutineIfNotNull(Coroutine coroutine) {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
