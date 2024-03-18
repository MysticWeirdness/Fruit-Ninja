using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    private Vector2 resetPos;
    private Vector2 startPos;
    private float elapsedTime = 0;
    private float timeInSeconds = 30;

    private void Awake()
    {
        startPos = new Vector2 (1, 5);
        resetPos = new Vector2 (-21, -25);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        Scroll(elapsedTime);
    }
    private void Scroll(float time)
    {
        float interpretedValue = time % timeInSeconds;
        float interpolationValue = interpretedValue / timeInSeconds;
        transform.position = Vector3.Lerp(startPos, resetPos, interpolationValue);
    }
}
