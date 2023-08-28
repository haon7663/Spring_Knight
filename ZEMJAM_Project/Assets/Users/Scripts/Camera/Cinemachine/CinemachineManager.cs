using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Inst { get; set; }
    void Awake() => Inst = this;


    public CinemachineVirtualCamera cinevirtual;

    public Transform player;
    public Transform tile;

    public bool isJoom;
    float realCineSize;
    float cinemacineSize;

    void LateUpdate()
    {
        var tileSize = TileManager.Inst.tileSize;
        var joomSize = tileSize * 0.85f;
        cinemacineSize = isJoom ? joomSize : tileSize;

        realCineSize = Mathf.Lerp(realCineSize, cinemacineSize, Time.deltaTime * 4);
        cinevirtual.m_Lens.OrthographicSize = realCineSize;

        cinevirtual.Follow = isJoom ? player : tile;
    }
}
