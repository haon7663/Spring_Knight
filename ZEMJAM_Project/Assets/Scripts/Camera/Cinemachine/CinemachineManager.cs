using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Inst { get; set; }
    public CinemachineVirtualCamera cinevirtual;

    public Transform player;
    public Transform tile;

    public bool isJoom;
    float real_CineSize;
    float CinemacineSize;

    void Awake() => Inst = this;

    void LateUpdate()
    {
        var tileSize = TileManager.Inst.tileSize;
        var joomSize = (1.5f + tileSize / 1.25f) / 1.9f;
        CinemacineSize = isJoom ? joomSize : tileSize;

        real_CineSize = Mathf.Lerp(real_CineSize, CinemacineSize, Time.deltaTime * 4);
        cinevirtual.m_Lens.OrthographicSize = real_CineSize;

        cinevirtual.Follow = isJoom ? player : tile;
    }
}
