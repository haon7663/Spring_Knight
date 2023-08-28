using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer m_SpriteRenderer;

    public int defence;
    [SerializeField] int minDefence;
    [SerializeField] int maxDefence;
    [SerializeField] bool doFlip;

    Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (maxDefence == 0) defence = minDefence;
        else defence = Random.Range(minDefence, maxDefence + 1);
        StartCoroutine(Inv());
    }

    private IEnumerator Inv()
    {
        gameObject.layer = 9;
        for (int i = 0; i < 2; i++)
        {
            m_SpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
            m_SpriteRenderer.color = new Color(1, 1, 1, 0.8f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        m_SpriteRenderer.color = new Color(1, 1, 1, 0.4f);
        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        m_SpriteRenderer.color = new Color(1, 1, 1, 1f);

        gameObject.layer = 7;

        yield return null;
    }

    private void LateUpdate()
    {
        if(doFlip)
            m_SpriteRenderer.flipX = playerTransform.position.x - transform.position.x > 0;
    }

    public void OnDamage()
    {
        ScoreManager.Inst.KillScore();
        if (GetComponent<EnemyDestroy>()) GetComponent<EnemyDestroy>().AfterDestroy();

        SummonManager.Inst.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
