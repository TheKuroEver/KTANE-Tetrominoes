using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rnd = UnityEngine.Random;

public class Holder : MonoBehaviour
{
    private void Awake() => StartCoroutine(IdleRotation());

    private IEnumerator IdleRotation() {
        var squareSize = 0.2f;
        var minSpeed = -2f;
        var maxSpeed = 2f;

        var xTime = 0f;
        var zTime = 0f;
        var xSpeed = Rnd.Range(minSpeed, maxSpeed);
        var ySpeed = Rnd.Range(minSpeed, maxSpeed);
        var nextXSpeed = Rnd.Range(minSpeed, maxSpeed);
        var nextYSpeed = Rnd.Range(minSpeed, maxSpeed);

        while (true) {
            for (int i = 0; i < 60; i++) {
                yield return null;
                var currentXSpeed = Mathf.Lerp(xSpeed, nextXSpeed, i / 60f);
                var currentYSpeed = Mathf.Lerp(ySpeed, nextYSpeed, i / 60f);

                xTime = (xTime + Time.deltaTime * currentXSpeed) % (2 * Mathf.PI);
                zTime = (zTime + Time.deltaTime * currentYSpeed) % (2 * Mathf.PI);

                var xPos = squareSize * Mathf.Sin(xTime);
                var zPos = squareSize * Mathf.Cos(zTime);
                transform.localRotation = Quaternion.FromToRotation(Vector3.up, new Vector3(xPos, 1, zPos));
            }
            xSpeed = nextXSpeed;
            ySpeed = nextYSpeed;
            nextXSpeed = Rnd.Range(minSpeed, maxSpeed);
            nextYSpeed = Rnd.Range(minSpeed, maxSpeed);
        }
    }
}
