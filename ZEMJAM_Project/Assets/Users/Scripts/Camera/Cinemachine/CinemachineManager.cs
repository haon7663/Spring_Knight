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

    float tileSize, joomSize, deathSize;

    void Start()
    {
        tileSize = TileManager.Inst.tileSize;
        realCineSize = tileSize -0.01f;
        cinevirtual.m_Lens.OrthographicSize = realCineSize;
        cinevirtual.Follow = tile;
        joomSize = 6.99f;
        deathSize = 5;
    }

    void LateUpdate()
    {
        if (!SettingManager.Inst.onCameraFollow) return;
        cinemacineSize = GameManager.Inst.onDeath ? deathSize : isJoom ? joomSize : tileSize;

        realCineSize = Mathf.Lerp(realCineSize, cinemacineSize, Time.deltaTime * 4);
        cinevirtual.m_Lens.OrthographicSize = realCineSize;

        cinevirtual.Follow = GameManager.Inst.onDeath || isJoom ? player : tile;
    }
}
