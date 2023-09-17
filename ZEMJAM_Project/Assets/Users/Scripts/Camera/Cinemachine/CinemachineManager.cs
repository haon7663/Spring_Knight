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
    public Transform playerFollow;
    public Transform tile;

    public bool isJoom;
    float realCineSize;
    float cinemacineSize;

    float tileSize, joomSize, deathSize;

    void Start()
    {
        tileSize = TileManager.Inst.tileSize;
        realCineSize = tileSize;
        cinevirtual.m_Lens.OrthographicSize = realCineSize;
        cinevirtual.Follow = tile;
        joomSize = tileSize * 0.85f;
        deathSize = tileSize * 0.7f;
    }

    void LateUpdate()
    {
        if (!SettingManager.Inst.onCameraFollow) return;
        cinemacineSize = GameManager.Inst.onDeath ? deathSize : isJoom ? joomSize : tileSize;

        realCineSize = Mathf.Lerp(realCineSize, cinemacineSize, Time.deltaTime * 4);
        cinevirtual.m_Lens.OrthographicSize = realCineSize;

        cinevirtual.Follow = GameManager.Inst.onDeath ? player : isJoom ? playerFollow : tile;
    }
}
