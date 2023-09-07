using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySprite : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;
    Transform playerTransform;

    public bool doFlip;

    void Awake()
    {
        if (TryGetComponent(out SpriteRenderer sprite))
            m_SpriteRenderer = sprite;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        StartCoroutine(GracePeriod());
    }

    void LateUpdate()
    {
        if(doFlip)
            m_SpriteRenderer.flipX = playerTransform.position.x - transform.position.x > 0;
    }

    IEnumerator GracePeriod()
    {
        gameObject.layer = 9;
        for (int i = 0; i < 5; i++)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, m_SpriteRenderer.color.a == 0.4f ? 0.8f : 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        m_SpriteRenderer.color = new Color(1, 1, 1, 1f);
        gameObject.layer = 7;
        yield return null;
    }

    public IEnumerator DeathFade()
    {
        doFlip = false;
        m_SpriteRenderer.sortingOrder = 0;

        m_SpriteRenderer.DOColor(Color.gray, 0.1f).SetEase(Ease.Linear);
        yield return YieldInstructionCache.WaitForSeconds(1.5f);

        m_SpriteRenderer.DOFade(0, 0.5f).SetEase(Ease.Linear);
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        yield return null;
    }
}
