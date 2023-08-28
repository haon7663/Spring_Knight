using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Sine : MonoBehaviour
{
    private Vector2 StartPos;

    private float sine;

    private void Start()
    {
        StartPos = transform.position;
    }

    private void Update()
    {
        sine += Time.deltaTime*3;
        transform.position = StartPos + new Vector2(0, Mathf.Sin(sine)) * 0.1f;
    }
}
