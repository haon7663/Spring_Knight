using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;

public class Movement : MonoBehaviour
{
    public static Movement instance;

    public SpriteRenderer m_SwordSpriteRenderer;

    private Rigidbody2D mRigidbody2D;
    private SpriteRenderer mSpriteRenderer;
    private Animator mAnimator;
    private Animator mSwordAnimator;
    private AudioSource mAudioSource;
    private AudioSource mSwordAudioSource;

    private Collison m_Collison;

    private Camera m_MainCamera;

    public Material DefaultMateral;
    public Material WhiteMateral;

    public GameObject m_Ray;
    public GameObject m_PowerBar;
    public GameObject m_After;
    public GameObject m_Spin;
    public GameObject m_Barrior;
    public Sprite[] m_ComboSprite;
    private SpriteRenderer m_SpinSpriteRenderer;

    public GameObject m_Combo;

    public LayerMask m_EnemyLayer;
    private Vector3 saveVelocity;
    private Vector2 LastVelocity;

    public Vector3 m_Dir;

    public int m_BounceCount;
    private float power;
    private float ableAngle;
    private float atkTime;
    private float m_TimeScale;

    public float m_Count = -1;
    public float m_BoostPower = 1;
    public float m_SlopeAngel;
    public float m_AfterImageCount;

    private float m_InvTime;
    private float filled = 0;

    public bool m_isInv;
    public bool m_Swing;
    public bool m_isAtk;

    private bool isHit = false;
    private bool isSpin = false;
    private bool onCol;

    private float mTimer;

    private void Start()
    {
        instance = this;
        Time.timeScale = 1f;
        mAudioSource = GetComponent<AudioSource>();
        mSwordAudioSource = m_SwordSpriteRenderer.GetComponent<AudioSource>();
        mSwordAnimator = m_SwordSpriteRenderer.GetComponent<Animator>();

        mRigidbody2D = GetComponent<Rigidbody2D>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpinSpriteRenderer = m_Spin.GetComponent<SpriteRenderer>();
        mSwordAnimator = m_SwordSpriteRenderer.GetComponent<Animator>();
        mAnimator = GetComponent<Animator>();
        m_Collison = GetComponent<Collison>();
        m_MainCamera = Camera.main;
    }


    private void Update()
    {
        if (GameManager.Inst.isSetting) return;

        mSpriteRenderer.material = m_InvTime > 0 ? WhiteMateral : DefaultMateral;
        if (m_InvTime > 0) m_InvTime -= Time.deltaTime;
        if (HealthManager.Inst.curhp <= 0) return;

        m_isAtk = atkTime > 0;
        if (atkTime > 0)
            atkTime -= Time.deltaTime;

        mSwordAnimator.SetBool("isAttack", atkTime > 0);

        CinemachineManager.Inst.isJoom = m_Count >= 0 && !isSpin;


        if(m_Count >= 0)
        {
            mTimer += Time.deltaTime;
            if(mTimer > m_AfterImageCount)
            {
                if (m_BounceCount >= 3)
                {
                    GameObject afterImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterImage.GetComponent<SpriteRenderer>().flipX = mSpriteRenderer.flipX;
                    afterImage.GetComponent<SpriteRenderer>().sprite = mSpriteRenderer.sprite;
                    mTimer = 0;
                }
                if (m_BounceCount >= 10)
                {
                    GameObject afterSpinImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterSpinImage.GetComponent<SpriteRenderer>().sprite = m_SpinSpriteRenderer.sprite;
                }
                else if(m_BounceCount >= 7)
                {
                    GameObject afterSwordImage = Instantiate(m_After, transform.position, Quaternion.identity);
                    afterSwordImage.GetComponent<SpriteRenderer>().sprite = m_SwordSpriteRenderer.sprite;
                    afterSwordImage.GetComponent<SpriteRenderer>().flipX = m_SwordSpriteRenderer.flipX;
                }
            }                
        }

        if (m_BounceCount >= 10 && m_Count >= 0)
        {
            if (!m_Spin.activeSelf)
            {
                mAnimator.SetBool("isSpin", true);
                mAnimator.SetTrigger("spin");
                m_Spin.SetActive(true);
            }
            m_Spin.transform.Rotate(0, 0, 2000 * Time.deltaTime);

            Collider2D enemy = Physics2D.OverlapCircle(transform.position, 2, m_EnemyLayer);
            if (enemy)
            {
                CinemachineShake.Instance.ShakeCamera(7, 0.5f);
                enemy.transform.GetComponent<Enemy>().OnDamage();
                enemy.transform.GetComponent<Defence>().DefenceBreak(enemy.transform.GetComponent<Defence>().defence);
                m_BoostPower = 1.75f;
                DOTween.To(() => m_BoostPower, x => m_BoostPower = x, 1f, 0.15f + m_Count * 0.1f).SetEase(Ease.OutQuint);
            }
        }
        else if(m_Spin.activeSelf)
        {
            m_Spin.SetActive(false);
            mAnimator.SetBool("isSpin", false);
        }

    }
    private void FixedUpdate()
    {
        if (isHit || GameManager.Inst.isSetting) return;
        if (m_Count < 0 || m_Collison.onCollision)
        {
            mRigidbody2D.velocity = Vector2.zero;
            if(!m_isAtk)
            {
                if (m_Collison.onRight) mSpriteRenderer.flipX = true;
                else if (m_Collison.onLeft) mSpriteRenderer.flipX = false;
            }
        }
        else
        {
            if (!m_isAtk)
            {
                if (mRigidbody2D.velocity.x > 0)
                    mSpriteRenderer.flipX = true;
                else if (mRigidbody2D.velocity.x < 0)
                    mSpriteRenderer.flipX = false;
            }
        }
        m_SwordSpriteRenderer.flipX = mSpriteRenderer.flipX;

        if (!isCoroutine && gameObject.layer == 3)
        {
            mRigidbody2D.velocity = saveVelocity * m_BoostPower;
            LastVelocity = saveVelocity;
        }
    }

    public void Dash(float power, float angle)
    {
        m_Count = power;
        m_BounceCount = 0;

        mAudioSource.Play();
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.position += -transform.right * 0.5f;
        mRigidbody2D.velocity = -transform.right * 17;
        saveVelocity = -transform.right * 17;
        transform.rotation = Quaternion.Euler(0, 0, 0);

        m_BoostPower = 2.25f;
        DOTween.To(() => m_BoostPower, x => m_BoostPower = x, 1f, 0.4f + power * 0.25f).SetEase(Ease.OutQuint);
    }

    private bool isCoroutine = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && m_Count >= 0)
        {
            //Time.timeScale = 0.15f;
            if (!isCoroutine) StartCoroutine(Attack(collision));
        }
        else if (collision.transform.CompareTag("Wall") && m_Count > 0)
        {
            onCol = true;
            mAudioSource.Play();
            var speed = LastVelocity.magnitude;
            var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);

            mRigidbody2D.velocity = dir * Mathf.Max(speed, 0f);
            saveVelocity = dir * Mathf.Max(speed, 0f);

            ComboPlus();
            m_Count--;
        }
        else if (collision.transform.CompareTag("Wall"))
        {
            foreach (GameObject enemy in SummonManager.Inst.enemyList)
            {
                if (enemy && enemy.GetComponent<EnemyDashSign>())
                    enemy.GetComponent<EnemyDashSign>().AfterDash();
            }
            onCol = true;
            mRigidbody2D.velocity = Vector2.zero;
            saveVelocity = Vector2.zero;
            m_SlopeAngel = Vector2.Angle(collision.contacts[0].normal, Vector2.up);
            m_Count--;
        }
        else if (collision.transform.CompareTag("Damage") && m_Count >= 0)
        {
            //Time.timeScale = 0.5f;
            if (!m_isInv) HealthManager.Inst.OnDamage();
            StartCoroutine(InvTime());
            StartCoroutine(Hit(collision.transform));
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall") && mRigidbody2D.velocity == Vector2.zero)
        {
            m_Count = -1;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            onCol = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            collision.GetComponent<Item>().UseItem();
        }
    }
    private void ComboPlus()
    {
        SpriteRenderer combo = Instantiate(m_Combo, transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();

        combo.sprite = m_ComboSprite[m_BounceCount > 14 ? 14 : m_BounceCount];
        combo.transform.position += new Vector3(transform.position.x > 0 ? -0.75f : 0.75f, transform.position.y > 0 ? -0.7f : 1f);
        combo.transform.localScale = new Vector3(1f + (float)m_BounceCount / 25f, 1f + (float)m_BounceCount / 25f, 1);

        m_BounceCount++;
    }
    private IEnumerator Attack(Collision2D collision)
    {
        Collision2D saveCollision = collision;
        int N = m_BounceCount - collision.transform.GetComponent<Enemy>().defence + 1;
        var speed = LastVelocity.magnitude;
        var dir = Vector2.Reflect(LastVelocity.normalized, collision.contacts[0].normal);

        isCoroutine = true;
        if (N >= 0)
        {
            if (N >= 3) ScoreManager.Inst.KillScore();
            m_Swing = true;
            atkTime = 0.4f;
            mSpriteRenderer.flipX = saveCollision.transform.position.x > transform.position.x ? true : false;

            for (float i = 0; i < 0.22f; i += Time.deltaTime)
            {
                if (!saveCollision.transform.CompareTag("Enemy")) break;
                mRigidbody2D.velocity = Vector2.zero;
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }            
            if(saveCollision.transform.CompareTag("Enemy"))
            {
                mAnimator.SetTrigger("attack");
                mSwordAudioSource.Play();
                mSwordAnimator.SetTrigger("swing");
                CinemachineShake.Instance.ShakeCamera(5, 0.5f);
                saveCollision.transform.GetComponent<Enemy>().OnDamage();
                saveCollision.transform.GetComponent<Defence>().DefenceBreak(collision.transform.GetComponent<Defence>().defence);
                ComboPlus();
            }

        }
        else
        {
            if (!m_isInv) HealthManager.Inst.OnDamage();
            StartCoroutine(InvTime());
            StartCoroutine(Hit(saveCollision.transform));
        }
        mRigidbody2D.velocity = dir * Mathf.Max(speed, 0f);
        saveVelocity = dir * Mathf.Max(speed, 0f);

        m_BoostPower = 1.75f;
        DOTween.To(() => m_BoostPower, x => m_BoostPower = x, 1f, 0.15f + m_Count * 0.1f).SetEase(Ease.OutQuint);
        isCoroutine = false;
    }
    public void takeMirror(Transform target)
    {
        Debug.Log("mirror");
        m_BoostPower = 1.75f;
        m_Count = m_BounceCount;
    }
    private IEnumerator InvTime()
    {
        m_Barrior.SetActive(false);
        m_isInv = true;
        m_InvTime = 0.1f;
        CinemachineShake.Instance.ShakeCamera(2, 0.5f);
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
        m_isInv = false;

        yield return null;
    }
    public void InvItem()
    {
        m_isInv = true;
        mSpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        m_Barrior.SetActive(true);
    }
    bool Onemore = false;
    public IEnumerator Hit(Transform target)
    {
        isHit = true;
        m_Count = -1;
        
        gameObject.layer = 8;

        mAnimator.SetBool("isHit", true);
        mSpriteRenderer.flipX = target.position.x - transform.position.x > 0;
        mRigidbody2D.velocity = Vector2.zero;
        mRigidbody2D.AddForce(new Vector2(target.position.x - transform.position.x > 0 ? -0.5f : 0.5f, 1), ForceMode2D.Impulse);
        mRigidbody2D.gravityScale = 5;
        if(HealthManager.Inst.curhp <= 0)
        {
            while (!m_Collison.onDown)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        else
        {
            while (!m_Collison.onCollision || !onCol)
            {
                yield return YieldInstructionCache.WaitForFixedUpdate;
            }
        }
        mRigidbody2D.gravityScale = 0;
        mAnimator.SetBool("isHit", false);
        isHit = false;

        LastVelocity = Vector2.zero;
        saveVelocity = Vector2.zero;

        gameObject.layer = 3;
        if (HealthManager.Inst.curhp <= 0)
        {
            mAnimator.SetBool("isDeath", true);
            if(m_Collison.onDown && !Onemore)
            {
                Onemore = true;
                Fade.instance.Fadein();
                Invoke(nameof(death), 0.5f);
            }
        }
    }
    private void death()
    {
        SceneManager.LoadScene("Death");
    }
}
