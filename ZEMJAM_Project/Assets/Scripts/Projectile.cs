using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Space]
    [Header("Float Setting")]
    public float mSpeed;
    public float mDamage;
    public float mDestroyTime;
    public float mCritical_Percent;
    public float mCritical_Damage;

    [Space]
    [Header("Bool Setting")]
    public bool isPenetrate;
    public bool isExtra = false;

    [Space]
    [Header("Particle")]
    public float mParticle_DestroyTime;

    private SpriteRenderer mSpriteRenderer;
    private Rigidbody2D mRigidbody2D;
    private TrailRenderer mTrailRenderer;

    private Transform mEnemy;

    public bool isAtk = false;
    public LayerMask enemyLayer;

    private float mDestroyTimer;
    private float mTrailTime;

    private void Awake()
    {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
        mTrailRenderer = GetComponent<TrailRenderer>();
    }
    private void Start()
    {
        isAtk = false;
    }
    private void OnEnable()
    {
        isAtk = false;
        mDestroyTimer = 0;
        if (mTrailRenderer)
        {
            mTrailTime = mTrailRenderer.time;
            mTrailRenderer.time = 0;
        }
        StartCoroutine(SetEnable());
    }
    IEnumerator SetEnable()
    {
        transform.localScale = new(1, 1, 1);
        mSpriteRenderer.enabled = true;
        mRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        //mRigidbody2D.AddForce(transform.right * mSpeed, ForceMode2D.Force);

        yield return new WaitForFixedUpdate();
        if (mTrailRenderer) mTrailRenderer.time = mTrailTime;
        yield return null;
    }
    private void FixedUpdate()
    {
        if (mRigidbody2D.bodyType == RigidbodyType2D.Dynamic) mRigidbody2D.velocity = transform.right * mSpeed * Time.deltaTime;
        mDestroyTimer += Time.deltaTime;
        if (mDestroyTimer > mDestroyTime) OnFalse();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAtk)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.GetComponent<Enemy>().OnDamage();
            }
        }
    }
    private void OnFalse()
    {
        gameObject.SetActive(false);
    }
}

