﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolyominoRenderer : MonoBehaviour
{
    private const float s_animatedLerpTime = .1f;

    private Dictionary<Vector2Int, MonominoRenderer> _monominoes = new Dictionary<Vector2Int, MonominoRenderer>();

    private Transform _container;
    private Vector3 _centre;

    private Coroutine _containerScaleAnimation;
    private Coroutine _containerRotationAnimation;
    private Coroutine _containerTranslationAnimation;
    private Coroutine _parentTranslationAnimation;

    public void SetParent(Transform parent) => transform.parent = parent;
    public void SetLocalScale(Vector3 scale) => AnimateScale(scale);

    public void SetLocalPosition(Vector3 position) => AnimateTranslation(position);

    public void SetLocalPositionByGeometricCentre(Vector3 position) => SetLocalPosition(position - _centre);

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
        renderer.transform.localScale = Vector3.zero;
        renderer.transform.localRotation = Quaternion.identity;

        renderer._centre = new Vector3(maxX / 2f, 0, maxY / 2f);
        renderer._container.localPosition = renderer._centre;
        foreach (MonominoRenderer monomino in renderer._monominoes.Values)
            monomino.transform.localPosition -= renderer._centre;

        return renderer;
    }

    public MonominoRenderer GetMonomino(Vector2Int position) => _monominoes[position];

    public void Rotate(Rotation direction) {
        switch (direction) {
            case Rotation.Clockwise: AnimateRotation(90); break;
            case Rotation.Counterclockwise: AnimateRotation(-90); break;
            default: throw new InvalidOperationException($"Unexpected Rotation value: {direction}.");
        }
        _centre = new Vector3(_centre.z, 0, _centre.x);
        StopCoroutineIfNotNull(_containerTranslationAnimation);
        _containerTranslationAnimation = StartCoroutine(AnimateVector3Property((value) => _container.localPosition = value, _container.localPosition, _centre));
    }

    private void AnimateScale(Vector3 newScale) {
        StopCoroutineIfNotNull(_containerScaleAnimation);
        _containerScaleAnimation = StartCoroutine(AnimateVector3Property((value) => transform.localScale = value, transform.localScale, newScale));
    }

    private void AnimateTranslation(Vector3 newPosition) {
        StopCoroutineIfNotNull(_parentTranslationAnimation);
        _parentTranslationAnimation = StartCoroutine(AnimateVector3Property((value) => transform.localPosition = value, transform.localPosition, newPosition));
    }

    private void AnimateRotation(float clockwiseAngle) {
        var oldRotation = _container.localRotation;
        var newRotation = _container.localRotation * Quaternion.AngleAxis(clockwiseAngle, Vector3.up);
        StopCoroutineIfNotNull(_containerRotationAnimation);
        _containerRotationAnimation = StartCoroutine(AnimateQuaternionProperty((value) => _container.localRotation = value, oldRotation, newRotation));
    }

    private IEnumerator AnimateVector3Property(Action<Vector3> setter, Vector3 oldValue, Vector3 newValue) {
        return AnimateInterpolatableProperty(setter, oldValue, newValue, Vector3.Lerp);
    }

    private IEnumerator AnimateQuaternionProperty(Action<Quaternion> setter, Quaternion oldValue, Quaternion newValue) {
        return AnimateInterpolatableProperty(setter, oldValue, newValue, Quaternion.Lerp);
    }

    private IEnumerator AnimateInterpolatableProperty<T>(Action<T> setter, T oldValue, T newValue, Func<T, T, float, T> lerper) {
        var elapsedTime = 0f;
        while (elapsedTime < s_animatedLerpTime) {
            setter.Invoke(lerper.Invoke(oldValue, newValue, elapsedTime / s_animatedLerpTime));
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
