using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform mPlayer;
    public int m_Power;
    public int m_PlusDefence;
    public int m_MinerDefence;
    private SpriteRenderer mSpriteRenderer;
    private AudioSource mAudioSource;


    private void Start()
    {
        mAudioSource = GetComponent<AudioSource>();
        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        mSpriteRenderer = GetComponent<SpriteRenderer>();

        m_Power = Random.Range(m_Power - m_MinerDefence, m_Power + m_PlusDefence + 1);

        StartCoroutine(Inv());
    }
    internal static class YieldInstructionCache
    {
        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
        private static readonly Dictionary<float, WaitForSeconds> waitForSeconds = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!waitForSeconds.TryGetValue(seconds, out wfs))
                waitForSeconds.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }
    private IEnumerator Inv()
    {
        gameObject.layer = 9;
        for (int i = 0; i < 2; i++)
        {
            mSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
            mSpriteRenderer.color = new Color(1, 1, 1, 0.8f);
            yield return YieldInstructionCache.WaitForSeconds(0.2f);
        }
        mSpriteRenderer.color = new Color(1, 1, 1, 0.4f);
        yield return YieldInstructionCache.WaitForSeconds(0.2f);
        mSpriteRenderer.color = new Color(1, 1, 1, 1f);

        gameObject.layer = 7;

        yield return null;
    }

    private void Update()
    {
        mSpriteRenderer.flipX = mPlayer.position.x - transform.position.x > 0 ? true : false;
    }

    public void OnDamage()
    {
        GameManager.Gm.m_EnemyCount--;
        GameManager.Gm.m_Score++;
        if (GetComponent<EnemyDestroy>()) GetComponent<EnemyDestroy>().AfterDestroy();
        mAudioSource.Play();
        gameObject.layer = 9;
        mSpriteRenderer.enabled = false;
        Destroy(gameObject, 0.5f);
    }
}
