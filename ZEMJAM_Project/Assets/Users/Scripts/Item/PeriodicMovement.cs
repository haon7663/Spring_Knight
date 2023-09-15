using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeriodicMovement : MonoBehaviour
{
    Vector2 startPos;

    [SerializeField] float sinSpeed;
    [SerializeField] float sinValue;
    float sin;

    private void Start()
    {
        startPos = transform.position;
        sin = Random.Range(0f, 2 * Mathf.PI);
        sinSpeed = Random.Range(sinSpeed * 0.75f, sinSpeed * 1.25f);
        sinValue = Random.Range(sinValue * 0.9f, sinValue * 1.1f);
    }

    private void Update()
    {
        sin += Time.deltaTime * sinSpeed;
        transform.position = startPos + new Vector2(0, Mathf.Sin(sin)) * sinValue;
    }
}