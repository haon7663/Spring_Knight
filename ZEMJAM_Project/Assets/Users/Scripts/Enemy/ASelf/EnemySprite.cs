using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;

    Transform playerTransform;
    [SerializeField] bool doFlip;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        StartCoroutine(GracePeriod());
    }

    private IEnumerator GracePeriod()
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

    private void LateUpdate()
    {
        if(doFlip)
            m_SpriteRenderer.flipX = playerTransform.position.x - transform.position.x > 0;
    }
}
