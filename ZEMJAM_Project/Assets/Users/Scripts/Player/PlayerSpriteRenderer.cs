using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    [Header("SpriteRenderers")]
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] SpriteRenderer m_SwordSpriteRenderer;
    [SerializeField] SpriteRenderer m_AfterSpriteRenderer;

    [Header("Components")]
    [SerializeField] Movement m_Movement;
    [SerializeField] Collison m_Collison;

    [Header("Materials")]
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material whiteMaterial;

    float hitTime;

    void Update()
    {
        if (GameManager.Inst.isSetting) return;

        SetMaterial();

        if (m_Movement.count <= 0 && m_Collison.onCollision)
        {
            if (m_Collison.onRight) 
                m_SpriteRenderer.flipX = true;
            else if (m_Collison.onLeft) 
                m_SpriteRenderer.flipX = false;
        }
        m_SwordSpriteRenderer.flipX = m_SpriteRenderer.flipX;
    }

    void SetMaterial()
    {
        m_SpriteRenderer.material = hitTime > 0 ? whiteMaterial : defaultMaterial;
        if (hitTime > 0) hitTime -= Time.deltaTime;
    }

    public IEnumerator GracePerioding()
    {
        hitTime = 0.1f;
        for (int i = 0; i < 5; i++)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, m_SpriteRenderer.color.a == 0.4f ? 0.8f : 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        m_SpriteRenderer.color = new Color(1, 1, 1, 1f);

        yield return null;
    }
}
