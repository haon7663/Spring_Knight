using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    [Header("SpriteRenderers")]
    [SerializeField] SpriteRenderer m_SpriteRenderer;
    [SerializeField] SpriteRenderer m_SpinSpriteRenderer;

    [Header("Components")]
    [SerializeField] Movement m_Movement;
    [SerializeField] Collison m_Collison;
    [SerializeField] Rigidbody2D m_Rigidbody2D;

    [Header("Materials")]
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material whiteMaterial;

    [Header("AfterImage")]
    [SerializeField] GameObject afterImagePrf;

    float hitTimer, afterTimer;

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
        else if(!m_Movement.isAttacking)
        {
            if (m_Rigidbody2D.velocity.x > 0)
                m_SpriteRenderer.flipX = false;
            else if (m_Rigidbody2D.velocity.x < 0)
                m_SpriteRenderer.flipX = true;
        }

        if (m_Movement.count > 0)
        {
            var bounceCount = m_Movement.bouncedCount;
            afterTimer += Time.deltaTime;
            if (afterTimer > 0.05f)
            {
                if (bounceCount > 4)
                {
                    SummonAfterImage(m_SpriteRenderer);

                    afterTimer = 0;
                }
            }
        }
    }

    public void SetTransformFlip(Transform target)
    {
        m_SpriteRenderer.flipX = transform.position.x > target.position.x;
    }
    void SummonAfterImage(SpriteRenderer spriteRenderer)
    {
        SpriteRenderer afterImage = Instantiate(afterImagePrf, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
        afterImage.sprite = spriteRenderer.sprite;
        afterImage.flipX = spriteRenderer.flipX;
    }
    void SetMaterial()
    {
        m_SpriteRenderer.material = hitTimer > 0 ? whiteMaterial : defaultMaterial;
        if (hitTimer > 0) hitTimer -= Time.deltaTime;
    }

    public IEnumerator GracePerioding()
    {
        hitTimer = 0.1f;
        for (int i = 0; i < 5; i++)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, m_SpriteRenderer.color.a == 0.4f ? 0.8f : 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        m_SpriteRenderer.color = new Color(1, 1, 1, 1f);

        yield return null;
    }
}
