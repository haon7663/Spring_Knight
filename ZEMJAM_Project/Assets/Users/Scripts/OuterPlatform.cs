using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterPlatform : MonoBehaviour
{
    public float m_Ratio;
    public Vector2 m_Pos;

    private Camera m_MainCamera;

    private void Start()
    {
        m_MainCamera = Camera.main;

        transform.localPosition = m_Pos * m_Ratio * TileManager.Inst.tileSize + m_Pos * 2.5f;
    }
}
