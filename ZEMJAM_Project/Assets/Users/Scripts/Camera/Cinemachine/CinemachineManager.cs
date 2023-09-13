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

    float tileSize, joomSize;

    void Start()
    {
        tileSize = TileManager.Inst.tileSize;
        realCineSize = tileSize;
        cinevirtual.m_Lens.OrthographicSize = realCineSize;
        cinevirtual.Follow = tile;
    }

    void LateUpdate()
    {
        tileSize = TileManager.Inst.tileSize;
        joomSize = tileSize * 0.8f;
        cinemacineSize = isJoom ? joomSize : tileSize;

        realCineSize = Mathf.Lerp(realCineSize, cinemacineSize, Time.deltaTime * 4);
        cinevirtual.m_Lens.OrthographicSize = realCineSize;

        cinevirtual.Follow = isJoom ? player : tile;
    }
}
