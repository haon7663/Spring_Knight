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
    public Transform highlightTransform;

    public bool isJoom;
    public bool isHighLight;
    float realCineSize;
    float cinemacineSize;

    float tileSize, joomSize, deathSize, highLightSize;
    float mulJoomAspect = 1;
    [SerializeField] PolygonCollider2D cinemachineConfiner;
    [SerializeField] Transform canvasSize;
    Vector2[] confinerOffset = new Vector2[] { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(-1, -1), new Vector2(1, -1) };

    void Start()
    {
        tileSize = TileManager.Inst.tileSize;
        realCineSize = tileSize;
        cinevirtual.m_Lens.OrthographicSize = realCineSize;
        cinevirtual.Follow = tile;
        joomSize = 6.99f + (tileSize - 8)/1.4f;
        highLightSize = 4f;
        deathSize = 5;

        float fixedAspectRatio = 9f / 19f;
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        if (currentAspectRatio < fixedAspectRatio)
            mulJoomAspect = fixedAspectRatio / currentAspectRatio;

        cinemacineSize = tileSize * mulJoomAspect;
        realCineSize = cinemacineSize;
        cinevirtual.m_Lens.OrthographicSize = realCineSize;

        Invoke(nameof(SetConfiner), 0.1f);
    }
    void SetConfiner()
    {
        for (int i = 0; i < 4; i++)
        { 
            var myPoints = cinemachineConfiner.points;
            myPoints[i] = new Vector2(Screen.width / 2, Screen.height / 2) * confinerOffset[i] * 1.01f * canvasSize.transform.localScale * (Screen.height < 2280 ? 2280f / Screen.height : 1);
            cinemachineConfiner.points = myPoints;
        }
        cinevirtual.GetComponent<CinemachineConfiner2D>().InvalidateCache();
    }

    void LateUpdate()
    {
        if (!SettingManager.Inst.onCameraFollow) return;
        cinemacineSize = (GameManager.Inst.onDeath ? deathSize : isHighLight ? highLightSize : isJoom ? joomSize : tileSize) * mulJoomAspect;

        realCineSize = Mathf.Lerp(realCineSize, cinemacineSize, Time.deltaTime * 4);
        cinevirtual.m_Lens.OrthographicSize = realCineSize;

        cinevirtual.Follow = GameManager.Inst.onDeath ? player : isHighLight ? highlightTransform : isJoom ? player : tile;
    }
}
